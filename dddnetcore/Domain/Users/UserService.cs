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

namespace DDDSample1.Domain.Users
{
    public class UserService{

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repo;
        private readonly AuthenticationService _authenticationService;
        private readonly ManagementApiClient _managementApiClient;
        private readonly string _accessToken;
        private readonly string  _domain;
        private readonly string  _audience;
        private readonly string  _clientId;
        private readonly string  _clientSecret;
        private readonly string  _connection;
        public UserService(IUnitOfWork unitOfWork, IUserRepository repo, AuthenticationService authenticationService){

            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._authenticationService = authenticationService;

            _domain = Environment.GetEnvironmentVariable("Auth0_Domain");
            _audience = Environment.GetEnvironmentVariable("Auth0_Audience");
            _clientId = Environment.GetEnvironmentVariable("Auth0_ClientId");
            _clientSecret = Environment.GetEnvironmentVariable("Auth0_ClientSecret");
            _connection = Environment.GetEnvironmentVariable("Auth0_Connection");

            _accessToken = _authenticationService.GetAccessToken(_domain,$"https://{_domain}/api/v2/", _clientId, _clientSecret).Result;

            _managementApiClient = new ManagementApiClient(_accessToken, _domain);
        }

        public async Task<UserDto> GetByIdAsync(String username)
        {
            var user = await this._repo.GetByIdAsync(new Username(username));
            
            if(user == null)
                return null;

            return new UserDto { Username = user.Id.Name, Email=user.Email.Address,Role=user.Role.ToString()};
        }

        public async Task<UserDto> addBackofficeUserAsync(CreatingUserDto dto)
        {
            var role = Enum.Parse<Role>(dto.Role.ToUpper());

            String year = DateTime.Now.Year.ToString();

            int count = await _repo.CountBackofficeUsersAsync();
            int number = count + 1; 

            string numberFormatted = number.ToString("D4");
            string usernameValue = $"{year}{numberFormatted}";

            var user = new User(Username.Create(role,usernameValue),new Email(dto.Email), role);

            Console.Write(user.Id);

            await this._repo.AddAsync(user);

            await this._unitOfWork.CommitAsync();

            return new UserDto { Username = user.Id.Name, Email=user.Email.Address,Role=user.Role.ToString()};
        }

        public async Task<UserDto>AddUserPatientAsync(CreatingUserPatientDto creatingUserPatientDto){

            // TODO: Verify if have patient record

            var user = new User(new Username(creatingUserPatientDto.Email),new Email(creatingUserPatientDto.Email),Role.PATIENT);

            await this._repo.AddAsync(user);

            //TODO: Dar update do user

            // Add user in auth0
            try{
                var userCreateRequest = new UserCreateRequest { UserId = creatingUserPatientDto.Email,
                                                                Email = creatingUserPatientDto.Email,
                                                                Password = creatingUserPatientDto.Password,
                                                                Connection = _connection,
                                                                UserMetadata = new Dictionary<string, object> {
                                                                    { "roles", new string[] { EnumDescription.GetEnumDescription(Role.PATIENT) } }},
                                                                    EmailVerified = false
                                                                };

                await _managementApiClient.Users.CreateAsync(userCreateRequest);

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            await this._unitOfWork.CommitAsync();

            return  new UserDto { Username = user.Id.Name, Email=user.Email.Address,Role=user.Role.ToString()};
        }
    }
}