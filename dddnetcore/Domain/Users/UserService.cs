using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using System.Reflection.Emit;
using DDDSample1.Domain.OperationTypes;
using System;
using DDDSample1.Domain.Auth;
using Microsoft.Extensions.Configuration;
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

namespace DDDSample1.Domain.Users
{
    public class UserService{

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repo;
        private readonly IPatientRepository _repoPatient;
        private readonly AuthenticationService _authenticationService;
        private readonly ManagementApiClient _managementApiClient;
        private readonly string _accessToken;
        private readonly string  _domain;
        private readonly string  _audience;
        private readonly string  _clientId;
        private readonly string  _clientSecret;
        private readonly string  _connection;
        private readonly string  _namespace;
        private readonly HttpClient _httpClient;

        public UserService(IUnitOfWork unitOfWork, IUserRepository repo,IPatientRepository repoPatient, AuthenticationService authenticationService){

            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._authenticationService = authenticationService;
            this._repoPatient = repoPatient;

            _domain = Environment.GetEnvironmentVariable("Auth0_Domain");
            _audience = Environment.GetEnvironmentVariable("Auth0_Audience");
            _clientId = Environment.GetEnvironmentVariable("Auth0_ClientId");
            _clientSecret = Environment.GetEnvironmentVariable("Auth0_ClientSecret");
            _connection = Environment.GetEnvironmentVariable("Auth0_Connection");
            _namespace = Environment.GetEnvironmentVariable("Auth0_Namespace_Roles");

            _accessToken = _authenticationService.GetAccessToken(_domain,$"https://{_domain}/api/v2/", _clientId, _clientSecret).Result;

            _managementApiClient = new ManagementApiClient(_accessToken, _domain);
            _httpClient = new HttpClient();

        }

        public async Task<UserDto> GetByIdAsync(String username)
        {
            var user = await this._repo.GetByIdAsync(new Username(username));
            
            if(user == null)
                return null;

            return new UserDto { Username = user.Id.Name, Email=user.Email.Address,Role=user.Role.ToString()};
        }

        public async Task<UserDto> AddBackofficeUserAsync(CreatingUserDto dto)
        {
            var role = Enum.Parse<Role>(dto.Role.ToUpper());

            String year = DateTime.Now.Year.ToString();

            int count = await _repo.CountBackofficeUsersAsync();
            int number = count + 1; 

            string numberFormatted = number.ToString("D5");
            string usernameValue = $"{year}{numberFormatted}";

            var user = new User(Username.Create(role,usernameValue),new Email(dto.Email), role);

            await this._repo.AddAsync(user);

            try{
                var userCreateRequest = new UserCreateRequest { UserId = user.Id.Name,
                                                                Email = dto.Email,
                                                                Password = dto.Password,
                                                                Connection = _connection,
                                                                AppMetadata = new Dictionary<string, object> {
                                                                    { "roles", new string[] { EnumDescription.GetEnumDescription(role) } }}
                                                                };

                await _managementApiClient.Users.CreateAsync(userCreateRequest);

                var requestBody = new
                {
                    client_id =_clientId,
                    email = dto.Email,
                    connection = _connection
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"https://{_domain}/dbconnections/change_password", content);

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            await this._unitOfWork.CommitAsync();

            return new UserDto { Username = user.Id.Name, Email=user.Email.Address,Role=user.Role.ToString()};
        }

        public async Task<UserDto>AddUserPatientAsync(CreatingUserPatientDto creatingUserPatientDto){

            var patient = await _repoPatient.GetByEmailAsync(creatingUserPatientDto.Email) ?? throw new NullReferenceException("Not Found Patient: " + creatingUserPatientDto.Email);

            var user = new User(new Username(creatingUserPatientDto.Email),new Email(creatingUserPatientDto.Email),Role.PATIENT);

            await this._repo.AddAsync(user);

            patient.UpdateUser(user);

            await this._repoPatient.UpdateAsync(patient);

            // Add user in auth0
            try{
                var userCreateRequest = new UserCreateRequest { UserId = creatingUserPatientDto.Email,
                                                                Email = creatingUserPatientDto.Email,
                                                                Password = creatingUserPatientDto.Password,
                                                                Connection = _connection,
                                                                AppMetadata = new Dictionary<string, object> {
                                                                    { "roles", new string[] { EnumDescription.GetEnumDescription(Role.PATIENT) } }},
                                                                    EmailVerified = false
                                                                };

                await _managementApiClient.Users.CreateAsync(userCreateRequest);

            } catch (Exception ex) {
                throw new Exception($"Error creating user: " + ex.Message);
            }

            await this._unitOfWork.CommitAsync();

            return  new UserDto { Username = user.Id.Name, Email=user.Email.Address,Role=user.Role.ToString()};
        }

        public async Task<LoginDto> LoginAsync(LoginRequestDto loginRequestDto) {
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

            if (tokenResponse.IsSuccessStatusCode) {
                var tokenResponseBody = await tokenResponse.Content.ReadAsStringAsync();
                var tokenResult = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(tokenResponseBody);
                string loginToken = tokenResult.GetProperty("access_token").GetString();

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(loginToken);
                
                var roles = jwtToken.Claims.Where(c => c.Type == $"{_namespace}/roles").Select(c => c.Value).ToList();

                return new LoginDto {LoginToken = loginToken, Roles = roles};
            } else {
                var error = await tokenResponse.Content.ReadAsStringAsync();
                throw new Exception($"Error retrieving access token: {error}");
            }
        }

        public async Task<UserDto> UpdateUserPatientAsync(string username,UpdateUserPatientDto updateUserPatientDto){
            
            var user = await _repo.GetByIdAsync(new Username(username)) ?? throw new NullReferenceException("Not Found user with: " + username);

            // TODO: Verify if patient exists
            
            if(updateUserPatientDto.FirstName != null)
                //patient.ChangeFirstName(new PatientFirstName(updateUserPatientDto.FirstName));

            if(updateUserPatientDto.LastName != null)
                //patient.ChangeLastName(new PatientLastName(updateUserPatientDto.LastName));

            if(updateUserPatientDto.FullName != null)
                //patient.ChangeFullName(new PatientFullName(updateUserPatientDto.FullName));

            if(updateUserPatientDto.Email != null)
                user.ChangeEmail(new Email(updateUserPatientDto.Email));
                //patient.ChangeEmail(new PatientEmail(updateUserPatientDto.Email));

            if(updateUserPatientDto.PhoneNumber != null){
                //patient.ChangePhoneNumber(new PatientPhone(updateUserPatientDto.PhoneNumber));
            }
            await this._repo.UpdateAsync(user);

            //await this._repoPatient.UpdateAsync(patient);
            try{
                var userUpdateRequest = new UserUpdateRequest {
                                                                Email = updateUserPatientDto.Email,
                                                                Connection = _connection,
                                                                };

                CancellationToken cancellationToken = default;
                await _managementApiClient.Users.UpdateAsync($"auth0|{username}",userUpdateRequest,cancellationToken);
                
                if (!string.IsNullOrEmpty(updateUserPatientDto.Password)){
                    var passwordUpdateRequest = new UserUpdateRequest
                    {
                        Password = updateUserPatientDto.Password,
                        Connection = _connection
                    };

                    await _managementApiClient.Users.UpdateAsync($"auth0|{username}", passwordUpdateRequest, CancellationToken.None);

                    var verifyEmailJobRequest = new VerifyEmailJobRequest{
                        UserId = $"auth0|{username}",
                        ClientId = _clientId  
                    };

                    await _managementApiClient.Jobs.SendVerificationEmailAsync(verifyEmailJobRequest);

                    }

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            await this._unitOfWork.CommitAsync();

            return  new UserDto { Username = user.Id.Name, Email=user.Email.Address,Role=user.Role.ToString()};
        }
    }
}