using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using System.Reflection.Emit;
using DDDSample1.Domain.OperationTypes;
using System;

namespace DDDSample1.Domain.Users
{
    public class UserService{

        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IUserRepository _repo;

        public UserService(IUnitOfWork unitOfWork, IUserRepository repo){

            this._unitOfWork = unitOfWork;
            this._repo = repo;
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
            
            return  new UserDto { Username = user.Id.Name, Email=user.Email.Address,Role=user.Role.ToString()};
        }
    }
}