using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using System;
using DDDSample1.Domain.Auth;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using System.Net.Http;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using DDDSample1.DataAnnotations.Patients;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Emails;
using DDDSample1.Domain.SystemLogs;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace DDDSample1.Domain.Users
{
    public class UserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repo;
        private readonly IPatientRepository _repoPatient;
        private readonly IAnonymizedPatientDataRepository _repoAnonymizedPatientData;
        private readonly ISystemLogRepository _repoSystemLog;
        private readonly AuthenticationService _authenticationService;
        private readonly ManagementApiClient _managementApiClient;
        private readonly IEmailService _emailService;
        private readonly string _accessToken;
        private readonly string _domain;
        private readonly string _audience;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _connection;
        private readonly string _namespace;
        private readonly HttpClient _httpClient;

        public UserService(IUnitOfWork unitOfWork, IUserRepository repo, IPatientRepository repoPatient, IAnonymizedPatientDataRepository anonymizedPatientDataRepository, AuthenticationService authenticationService, IEmailService emailService, ISystemLogRepository systemLogRepository)
        {

            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._authenticationService = authenticationService;
            this._repoPatient = repoPatient;
            this._repoAnonymizedPatientData = anonymizedPatientDataRepository;
            this._emailService = emailService;
            this._repoSystemLog = systemLogRepository;

            _domain = Environment.GetEnvironmentVariable("Auth0_Domain");
            _audience = Environment.GetEnvironmentVariable("Auth0_Audience");
            _clientId = Environment.GetEnvironmentVariable("Auth0_ClientId");
            _clientSecret = Environment.GetEnvironmentVariable("Auth0_ClientSecret");
            _connection = Environment.GetEnvironmentVariable("Auth0_Connection");
            _namespace = Environment.GetEnvironmentVariable("Auth0_Namespace_Roles");

            _accessToken = _authenticationService.GetAccessToken(_domain, $"https://{_domain}/api/v2/", _clientId, _clientSecret).Result;

            _managementApiClient = new ManagementApiClient(_accessToken, _domain);
            _httpClient = new HttpClient();

        }

        public async Task<UserDto> GetByIdAsync(String username)
        {
            var user = await this._repo.GetByIdAsync(new Username(username));

            if (user == null)
                return null;

            return new UserDto { Username = user.Id.Name, Email = user.Email.Address, Role = user.Role.ToString() };
        }

        public async Task<UserDto> AddBackofficeUserAsync(CreatingUserDto dto)
        {
            var role = Enum.Parse<Role>(dto.Role.ToUpper());

            String year = DateTime.Now.Year.ToString();

            int count = await _repo.CountBackofficeUsersAsync();
            int number = count + 1;

            string numberFormatted = number.ToString("D5");
            string usernameValue = $"{year}{numberFormatted}";

            var user = new User(Username.Create(role, usernameValue), new Email(dto.Email), role);

            await this._repo.AddAsync(user);

            try
            {
                var userCreateRequest = new UserCreateRequest
                {
                    UserId = user.Id.Name,
                    Email = dto.Email,
                    Password = dto.Password,
                    Connection = _connection,
                    AppMetadata = new Dictionary<string, object> {
                                                                    { "roles", new string[] { EnumDescription.GetEnumDescription(role) } }}
                };

                await _managementApiClient.Users.CreateAsync(userCreateRequest);

                var requestBody = new
                {
                    client_id = _clientId,
                    email = dto.Email,
                    connection = _connection
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"https://{_domain}/dbconnections/change_password", content);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            await this._unitOfWork.CommitAsync();

            return new UserDto { Username = user.Id.Name, Email = user.Email.Address, Role = user.Role.ToString() };
        }

        public async Task<UserDto> AddUserPatientAsync(CreatingUserPatientDto creatingUserPatientDto)
        {

            try
            {
                var patientEmail = new PatientEmail(creatingUserPatientDto.Email);
            }
            catch (BusinessRuleValidationException ex)
            {
                throw new BusinessRuleValidationException(ex.Message);
            }

            var patient = await _repoPatient.GetByEmailAsync(creatingUserPatientDto.Email) ?? throw new NullReferenceException("Not Found Patient: " + creatingUserPatientDto.Email);

            var user = new User(new Username(creatingUserPatientDto.Email), new Email(creatingUserPatientDto.Email), Role.PATIENT);

            await this._repo.AddAsync(user);

            patient.UpdateUser(user);

            await this._repoPatient.UpdateAsync(patient);

            // Add user in auth0
            try
            {
                var userCreateRequest = new UserCreateRequest
                {
                    UserId = creatingUserPatientDto.Email,
                    Email = creatingUserPatientDto.Email,
                    Password = creatingUserPatientDto.Password,
                    Connection = _connection,
                    AppMetadata = new Dictionary<string, object> {
                                                                    { "roles", new string[] { EnumDescription.GetEnumDescription(Role.PATIENT) } }},
                    EmailVerified = false
                };

                await _managementApiClient.Users.CreateAsync(userCreateRequest);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating user: " + ex.Message);
            }

            await this._unitOfWork.CommitAsync();

            return new UserDto { Username = user.Id.Name, Email = user.Email.Address, Role = user.Role.ToString() };
        }

        public async Task<LoginDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var tokenEndpoint = $"https://{_domain}/oauth/token";

            var tokenRequestBody = new Dictionary<string, string> {
                { "grant_type", "password" },
                { "username", loginRequestDto.Username },
                { "password", loginRequestDto.Password },
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "audience", _audience },
                { "connection", _connection }};

            var requestContent = new FormUrlEncodedContent(tokenRequestBody);

            var tokenResponse = await _httpClient.PostAsync(tokenEndpoint, requestContent);

            if (tokenResponse.IsSuccessStatusCode)
            {
                var tokenResponseBody = await tokenResponse.Content.ReadAsStringAsync();
                var tokenResult = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(tokenResponseBody);
                string loginToken = tokenResult.GetProperty("access_token").GetString();

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(loginToken);

                var emailVerifiedClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == $"{_namespace}/email_verified");

                if (emailVerifiedClaim == null || emailVerifiedClaim.Value != "true")
                {
                    throw new BusinessRuleValidationException("Email not verified. Please check your email before logging in.");
                }

                var roles = jwtToken.Claims.Where(c => c.Type == $"{_namespace}/roles").Select(c => c.Value).ToList();

                return new LoginDto { LoginToken = loginToken, Roles = roles };
            }
            else
            {
                var error = await tokenResponse.Content.ReadAsStringAsync();

                var user = await _repo.GetByEmail(new Email(loginRequestDto.Username));

                Console.WriteLine(user.Id.Name);
                if (error.Contains("too_many_attempts") && user.Role != Role.PATIENT)
                {

                    var admins = await _repo.GetByRole(Role.ADMIN);

                    List<string> adminsEmails = new List<string>();
                    foreach (var admin in admins)
                    {
                        adminsEmails.Add(admin.Email.Address);
                    }
                    string emailSubject = "Alert: Maximum Attempts Exceeded";
                    string emailBody = $@"
                            <p>Dear Admin,</p>
                            <p>We would like to inform you that the user <strong>{loginRequestDto.Username}</strong> has exceeded the maximum number of allowed login attempts.</p>
                            <p>Please review the situation and take the necessary steps.</p>
                            <p>Best regards,<br>SARM Team</p>";

                    await _emailService.SendEmailAsync(adminsEmails, emailSubject, emailBody);

                }
                throw new Exception($"Error retrieving access token: {error}");
            }
        }

        public async Task<bool> ResetPassword(RequestResetPasswordDto requestResetPasswordDto)
        {
            var resetPasswordUrl = $"https://{_domain}/dbconnections/change_password";

            var requestBody = new
            {
                client_id = _clientId,
                email = requestResetPasswordDto.Email,
                connection = _connection
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(resetPasswordUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<UserDto> UpdateUserPatientAsync(string username, UpdateUserPatientDto updateUserPatientDto)
        {

            var user = await _repo.GetByIdAsync(new Username(username)) ?? throw new NullReferenceException("Not Found user with: " + username);

            var patient = await _repoPatient.GetByUserIdAsync(username) ?? throw new NullReferenceException("Not Found patient with: " + username);

            var userChanges = new List<string>();
            var patientChanges = new List<string>();

            if (updateUserPatientDto.FirstName != null)
            {
                patientChanges.Add($"FirstName changed from {patient.FirstName.Name} to {updateUserPatientDto.FirstName}");
                patient.ChangeFirstName(new PatientFirstName(updateUserPatientDto.FirstName));
            }

            if (updateUserPatientDto.LastName != null)
            {
                patientChanges.Add($"LastName changed from {patient.LastName.Name} to {updateUserPatientDto.LastName}");
                patient.ChangeLastName(new PatientLastName(updateUserPatientDto.LastName));
            }

            if (updateUserPatientDto.FullName != null)
            {
                patientChanges.Add($"FullName changed from {patient.FullName.Name} to {updateUserPatientDto.FullName}");
                patient.ChangeFullName(new PatientFullName(updateUserPatientDto.FullName));
            }

            if (updateUserPatientDto.Email != null)
            {
                userChanges.Add($"Email changed from {user.Email.Address} to {updateUserPatientDto.Email}");
                patientChanges.Add($"Email changed from {patient.ContactInformation.Email.Email} to {updateUserPatientDto.Email}");
                user.ChangeEmail(new Email(updateUserPatientDto.Email));
                patient.ChangeEmail(new PatientEmail(updateUserPatientDto.Email));
            }

            if (updateUserPatientDto.PhoneNumber != null)
            {
                patientChanges.Add($"PhoneNumber changed from {patient.ContactInformation.PhoneNumber.PhoneNumber} to {updateUserPatientDto.PhoneNumber}");
                patient.ChangePhoneNumber(new PatientPhone(updateUserPatientDto.PhoneNumber));
            }

            if (updateUserPatientDto.Address != null || updateUserPatientDto.PostalCode != null)
            {
                string[] location = patient.Address.Location.Split(':');
                updateUserPatientDto.Address ??= location[0];
                updateUserPatientDto.PostalCode ??= location[1];
                patientChanges.Add($"Address was updated from {patient.Address.Location} to {updateUserPatientDto.Address}");
                patient.ChangeAddress(new Address(updateUserPatientDto.Address + ":" + updateUserPatientDto.PostalCode));
            }

            await this._repo.UpdateAsync(user);

            await this._repoPatient.UpdateAsync(patient);

            try
            {
                var userUpdateRequest = new UserUpdateRequest
                {
                    Email = updateUserPatientDto.Email,
                    Connection = _connection,
                };

                CancellationToken cancellationToken = default;
                await _managementApiClient.Users.UpdateAsync($"auth0|{username}", userUpdateRequest, cancellationToken);

                if (!string.IsNullOrEmpty(updateUserPatientDto.Password))
                {
                    Console.WriteLine($"Updating password for user: {username}");
                    var passwordUpdateRequest = new UserUpdateRequest
                    {
                        Password = updateUserPatientDto.Password,
                        Connection = _connection
                    };
                    await _managementApiClient.Users.UpdateAsync($"auth0|{username}", passwordUpdateRequest, CancellationToken.None);

                    var verifyEmailJobRequest = new VerifyEmailJobRequest
                    {
                        UserId = $"auth0|{username}",
                        ClientId = _clientId
                    };

                    await _managementApiClient.Jobs.SendVerificationEmailAsync(verifyEmailJobRequest);

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating password: " + ex.Message);
            }

            if (userChanges.Count > 0)
            {
                string userLog = string.Join(", ", userChanges);
                await this._repoSystemLog.AddAsync(new SystemLog(Operation.UPDATE, Entity.USER, userLog, user.Id.Name));
            }

            if (patientChanges.Count > 0)
            {
                string patientLog = string.Join(", ", patientChanges);
                await this._repoSystemLog.AddAsync(new SystemLog(Operation.UPDATE, Entity.PATIENT, patientLog, patient.Id.Id));
            }

            await this._unitOfWork.CommitAsync();

            return new UserDto { Username = user.Id.Name, Email = user.Email.Address, Role = user.Role.ToString() };
        }

        public async Task<String> RequestDeleteUserPatientAsync(string username)
        {
            var user = await _repo.GetByIdAsync(new Username(username));
            if (user == null)
            {
                throw new NullReferenceException("Not Found user with: " + username);
            }

            var to = new List<string> { user.Email.Address };
            var subject = "Confirm Account Deletion";
            var body = $@"
                        <p>You requested to delete your account. Please confirm by clicking the button below:</p>
                        <p>
                            <a href='http://localhost:5000/api/users/patients/confirm-delete/{username}'
                            style='background-color: #4CAF50; color: white; padding: 10px 20px; text-align: center;
                                    text-decoration: none; display: inline-block; font-size: 16px; border-radius: 5px;'>
                                Confirm Account Deletion
                            </a>
                        </p>";


            await _emailService.SendEmailAsync(to, subject, body);

            return "Email to confirm account deletion";
        }

        public async Task<UserDto> DeleteUserPatientAsync(string username)
        {

            var user = await _repo.GetByIdAsync(new Username(username)) ?? throw new NullReferenceException("Not Found user with: " + username);

            var patient = await _repoPatient.GetByUserIdAsync(username) ?? throw new NullReferenceException("Not Found patient with: " + username);

            var anonymizedPatientData = new AnonymizedPatientData(
                CalculateAgeRange(patient.DateOfBirth),
                EnumDescription.GetEnumDescription(patient.Gender),
                patient.MedicalConditions.Conditions
                );

            await this._repoAnonymizedPatientData.AddAsync(anonymizedPatientData);

            this._repo.Remove(user);
            this._repoPatient.Remove(patient);

            try
            {

                await _managementApiClient.Users.DeleteAsync($"auth0|{username}");

            }
            catch (Exception ex)
            {
                throw new Exception($"Error when deleting user '{username}': {ex.Message}", ex);
            }

            string userLogMessage = $"User '{username}' was deleted for GDPR compliance.";
            string patientLogMessage = $"Patient associated with user '{username}' was deleted for GDPR compliance.";

            await this._repoSystemLog.AddAsync(new SystemLog(Operation.DELETE, Entity.USER, userLogMessage, user.Id.Name));

            await this._repoSystemLog.AddAsync(new SystemLog(Operation.DELETE, Entity.PATIENT, patientLogMessage, patient.Id.Id));

            await this._unitOfWork.CommitAsync();

            var to = new List<string> { user.Email.Address };
            var subject = "Account Deleted";
            var body = "Your account has been successfully deleted";

            await _emailService.SendEmailAsync(to, subject, body);

            return new UserDto { Username = user.Id.Name, Email = user.Email.Address, Role = user.Role.ToString() };
        }

        public async Task<UserDto> GetLoggedInUser(string authorization)
        {

            if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
            {
                return null;
            }
            var token = authorization.Substring("Bearer ".Length).Trim();

            var username = _authenticationService.GetLoggedInUsername(token);

            Console.WriteLine(username);
            Console.WriteLine(token);

            return await this.GetByIdAsync(username);
        }

        private string CalculateAgeRange(DateOfBirth dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Date.Year;

            return age switch
            {
                < 18 => "Under 18",
                >= 18 and <= 35 => "18-35",
                > 35 and <= 50 => "36-50",
                > 50 and <= 65 => "51-65",
                _ => "65+"
            };
        }

        public async Task<LoginDto> RegisterOrLoginWithGoogle(string token)
        {
        
            try{
                var userInfo = await ValidateTokenAndGetUserInfo(token);

                var email = userInfo["email"];
                var sub = userInfo["sub"];

                var patient = await _repoPatient.GetByEmailAsync(email) ?? 
                    throw new NullReferenceException("Not Found Patient: " + email);

                var existingUser = await _repo.GetByEmail(new Email(email));

                if (existingUser != null) {

                    var roles = await GetRolesFromAuth0Async(sub);
                    if (!roles.Any())
                    {
                        roles = new List<string> { EnumDescription.GetEnumDescription(Role.PATIENT) };
                        await UpdateUserRolesInAuth0(sub, roles);
                    }

                    return new LoginDto { LoginToken = token, Roles = roles };
                }

                var newUser = new User(new Username(email), new Email(email), Role.PATIENT);
                await _repo.AddAsync(newUser);
                patient.UpdateUser(newUser);
                await _repoPatient.UpdateAsync(patient);

                var newRoles = new List<string> { EnumDescription.GetEnumDescription(Role.PATIENT) };
                await UpdateUserRolesInAuth0(sub, newRoles);

                await _unitOfWork.CommitAsync();

                return new LoginDto { LoginToken = token, Roles = newRoles };

            } catch (Exception ex){

                    throw new Exception($"Error during Google login: {ex.Message}");
            }
        }

        private async Task UpdateUserRolesInAuth0(string sub, List<string> roles)
        {
            var userUpdateRequest = new UserUpdateRequest {

                AppMetadata = new Dictionary<string, object>
                {
                    { "roles", roles }
                }
            };

            await _managementApiClient.Users.UpdateAsync(sub, userUpdateRequest);
        }

        public async Task<Dictionary<string, string>> ValidateTokenAndGetUserInfo(string token){

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = await Task.Run(() => handler.ReadJwtToken(token));

            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var sub = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sub))
            {
                throw new BusinessRuleValidationException("Email or Subject ID is missing in the token.");
            }

            return new Dictionary<string, string>
            {
                { "email", email },
                { "sub", sub }
            };
        }

        public async Task<List<string>> GetRolesFromAuth0Async(string sub) {
            
            try {
                var user = await _managementApiClient.Users.GetAsync(sub);

                if (user.AppMetadata.ContainsKey("roles"))
                {
                    var rolesClaim = user.AppMetadata["roles"] as JArray;

                    if (rolesClaim != null)
                    {
                        return rolesClaim.Select(r => r.ToString()).ToList();
                    }
                }

                Console.WriteLine("No roles found in Auth0 user metadata.");
                return new List<string>(); 

            } catch (Exception ex){

                Console.WriteLine($"Error fetching roles from Auth0: {ex.Message}");
                return new List<string>();
            }
        }
    }
}