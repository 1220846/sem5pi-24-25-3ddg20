using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.DataAnnotations.Staffs;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Users;

namespace dddnetcore.Domain.Staffs
{
    public class StaffService
    {
        IUnitOfWork _unitOfWork;
        private readonly IStaffRepository _repo;
        private readonly IAvailabilitySlotRepository _availabilitySlotRepo;
        private readonly ISpecializationRepository _specializationRepo;
        private readonly IUserRepository _userRepo;

        public StaffService(IUnitOfWork unitOfWork, IStaffRepository repo, IAvailabilitySlotRepository availabilitySlotRepo, ISpecializationRepository specializationRepo, IUserRepository userRepo) {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._availabilitySlotRepo = availabilitySlotRepo;
            this._specializationRepo = specializationRepo;
            this._userRepo = userRepo;
        }   

        public async Task<StaffDto> GetByIdAsync(StaffId id)
        {
            var staff = await this._repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Not Found Staff with Id: {id}");
            return new StaffDto(staff);
        }

        public async Task<StaffDto> AddAsync(CreatingStaffDto dto)
        {

            if (dto.FirstName != null && dto.LastName != null && dto.FullName != null && dto.Email != null &&
            dto.LicenseNumber != null && dto.SpecializationId != null && dto.UserEmail != null)
            {
                DDDSample1.Domain.Users.User user = await _userRepo.GetByIdAsync(new Username(dto.UserEmail)) ?? throw new NullReferenceException("User not found!");
                Specialization specialization = await _specializationRepo.GetByIdAsync(new SpecializationId(dto.SpecializationId)) ?? throw new NullReferenceException("Specialization not found!");

                var staff = new Staff(
                    user.Id.Name.Split("@")[0],
                    new StaffFirstName(dto.FirstName),
                    new StaffLastName(dto.LastName),
                    new StaffFullName(dto.FullName),
                    new StaffContactInformation(
                        new StaffEmail(dto.Email),
                        new StaffPhone(dto.PhoneNumber)
                    ),
                    new LicenseNumber(dto.LicenseNumber),
                    [],
                    specialization,
                    user
                );
                await this._repo.AddAsync(staff);
                await this._unitOfWork.CommitAsync();
                return new StaffDto(staff);
            }

            throw new ArgumentNullException("Missing data for staff creation!");
        }

        public async Task<List<StaffDto>> GetStaffsAsync(string firstName = null, string lastName = null, string fullName = null, string email = null, Guid? specializationId = null,
        string phoneNumber = null, string id = null, string licenseNumber = null, int pageNumber = 1, int pageSize = 10) {
            try {
            List<Staff> staffs = await this._repo.GetStaffsAsync(firstName, lastName, fullName, email, specializationId, phoneNumber, id, licenseNumber, pageNumber, pageSize);
            
            List<StaffDto> staffsDto = staffs.ConvertAll<StaffDto>(staff => new StaffDto(staff));
            
            return staffsDto;
            } catch (BusinessRuleValidationException) {
                return new List<StaffDto>();
            }
        }
    }
}