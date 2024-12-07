using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.Appointments;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.DataAnnotations.Staffs;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.Emails;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.SystemLogs;
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
        private readonly ISystemLogRepository _repoSystemLog;
        private readonly IEmailService _emailService;
        private readonly IOperationTypeRepository _operationTypeRepo;
        private readonly IAppointmentRepository _appointmentRepo;

        public StaffService(IUnitOfWork unitOfWork, IStaffRepository repo, IAvailabilitySlotRepository availabilitySlotRepo, ISpecializationRepository specializationRepo, IUserRepository userRepo, ISystemLogRepository repoSystemLog, IEmailService emailService, IOperationTypeRepository operationTypeRepository, IAppointmentRepository appointmentRepository)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._availabilitySlotRepo = availabilitySlotRepo;
            this._specializationRepo = specializationRepo;
            this._repoSystemLog = repoSystemLog;
            this._userRepo = userRepo;
            this._emailService = emailService;
            this._operationTypeRepo = operationTypeRepository;
            this._appointmentRepo = appointmentRepository;
        }

        public async Task<StaffDto> GetByIdAsync(string id)
        {
            //var staff = await this._repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Not Found Staff with Id: {id}");
            //return new StaffDto(staff);

            List<StaffDto> dto = await GetStaffsAsync(id: id);

            return dto.FirstOrDefault();
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
                    user,
                    StaffStatus.ACTIVE
                );
                await this._repo.AddAsync(staff);
                await this._unitOfWork.CommitAsync();
                return new StaffDto(staff);
            }

            throw new ArgumentNullException("Missing data for staff creation!");
        }

        public async Task<List<StaffDto>> GetStaffsAsync(string firstName = null, string lastName = null, string fullName = null, string email = null, Guid? specializationId = null,
        string phoneNumber = null, string id = null, string licenseNumber = null,
        string status = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                List<Staff> staffs = await this._repo.GetStaffsAsync(firstName, lastName, fullName, email, specializationId, phoneNumber, id, licenseNumber, status, pageNumber, pageSize);

                List<StaffDto> staffsDto = staffs.ConvertAll<StaffDto>(staff => new StaffDto(staff));

                return staffsDto;
            }
            catch (BusinessRuleValidationException)
            {
                return new List<StaffDto>();
            }
        }

        public async Task<int> GetStaffsCountAsync() {
            try {
                return await _repo.GetStaffsCountAsync();
            } catch (Exception e) {
                throw new Exception("Error counting staffs", e);
            }
        }
        
        public async Task<StaffDto> EditStaffAsync(string id, EditingStaffDto dto) 
        {
            bool changedContactInfo = false;
            Staff staff = (await _repo.GetStaffsAsync(id: id)).FirstOrDefault() ?? throw new NullReferenceException("Staff not found");
            StaffEmail previousEmail = staff.ContactInformation.Email;

            List<string> changeLog = [];

            if (dto.SpecializationId.HasValue)
            {
                Specialization specialization = await _specializationRepo.GetByIdAsync(new SpecializationId(dto.SpecializationId.Value)) ?? throw new NullReferenceException("Specialization not found");
                changeLog.Add($"Staff changed specialization from {staff.Specialization.Name} to {specialization.Name}");
                staff.ChangeSpecialization(specialization);
            }
            if (dto.ToRemoveAvailabilitySlotId.HasValue)
            {
                AvailabilitySlot availabilitySlot = await _availabilitySlotRepo.GetByIdAsync(new AvailabilitySlotId(dto.ToRemoveAvailabilitySlotId.Value)) ?? throw new NullReferenceException("Availability slot not found");
                changeLog.Add($"Staff removed availability slot {availabilitySlot.StartTime.Time}-{availabilitySlot.EndTime.Time}");
                staff.RemoveAvailabilitySlot(availabilitySlot);
                this._availabilitySlotRepo.Remove(availabilitySlot);
            }
            if (dto.NewAvailabilitySlotStartTime != null && dto.NewAvailabilitySlotEndTime != null)
            {
                StartTime startTime = new(DateTimeOffset.FromUnixTimeSeconds((long)dto.NewAvailabilitySlotStartTime).DateTime);
                EndTime endTime = new(DateTimeOffset.FromUnixTimeSeconds((long)dto.NewAvailabilitySlotEndTime).DateTime);
                AvailabilitySlot availabilitySlot = new(startTime, endTime);
                changeLog.Add($"Staff added availability slot {availabilitySlot.StartTime.Time}-{availabilitySlot.EndTime.Time}");
                staff.AddAvailabilitySlot(availabilitySlot);
                await this._availabilitySlotRepo.AddAsync(availabilitySlot);
            }
            if (dto.Email != null)
            {
                changedContactInfo = true;
                changeLog.Add($"Staff changed email from {staff.ContactInformation.Email.Email} to {dto.Email}");
                staff.ContactInformation.ChangeEmail(new StaffEmail(dto.Email));
            }
            if (dto.PhoneNumber != null)
            {
                changedContactInfo = true;
                changeLog.Add($"Staff changed phone number from {staff.ContactInformation.PhoneNumber.PhoneNumber} to {dto.PhoneNumber}");
                staff.ContactInformation.ChangePhoneNumber(new StaffPhone(dto.PhoneNumber));
            }

            await this._repo.UpdateAsync(staff);

            if (changeLog.Count > 0)
                await this._repoSystemLog.AddAsync(new SystemLog(Operation.UPDATE, Entity.STAFF, string.Join(",", changeLog), staff.Id.Id));

            await this._unitOfWork.CommitAsync();

            if (changedContactInfo)
            {

                var to = new List<string> { previousEmail.Email };
                var subject = "Contact information change.";
                var body = $@"
                <p>Dear {staff.FullName.Name},Your contact information has been changed:</p>
                <p>Email Address: {staff.ContactInformation.Email.Email}.</p>
                <p>Phone Number: {staff.ContactInformation.PhoneNumber.PhoneNumber}.</p>";

                await _emailService.SendEmailAsync(to, subject, body);
            }

            return new StaffDto(staff);
        }

        public async Task<StaffDto> RemoveAsync(string id)
        {
            Staff staff = (await _repo.GetStaffsAsync(id: id)).FirstOrDefault() ?? throw new NullReferenceException("Staff not found");
            staff.Deactivate();
            await this._repo.UpdateAsync(staff);
            await this._unitOfWork.CommitAsync();
            return new StaffDto(staff);
        }
        public async Task<List<StaffOperationTypesDto>> GetStaffsOperationTypesAsync()
        {
            var staffs = await this._repo.GetAllAsync();
            var operationTypes = await this._operationTypeRepo.GetOperationTypesAsync();

            var result = new List<StaffOperationTypesDto>();

            foreach (var staff in staffs)
            {

                var matchingOperations = operationTypes
                    .Where(op => op.OperationTypeSpecializations
                        .Any(ots => ots.Specialization.Id == staff.Specialization.Id))
                    .Select(op => op.Name.Name)
                    .ToList();
                ;

                Console.WriteLine(staff.Specialization.Name.Name);
                var staffDto = new StaffOperationTypesDto
                {
                    StaffID = staff.Id.Value,
                    Role = EnumDescription.GetEnumDescription(staff.User.Role),
                    Specialization = staff.Specialization.Name.Name,
                    OperationTypesName = matchingOperations
                };

                result.Add(staffDto);
            }

            return result;
        }

        public async Task<List<StaffAppointmentsDto>> GetStaffAppointmentsAsync() {
            var staffs = await this._repo.GetAllAsync();

            List<StaffAppointmentsDto> staffAppointmentsList = new List<StaffAppointmentsDto>();

            foreach (var staff in staffs)
            {
                var appointmentsStaff = await this._appointmentRepo.GetByStaffIdAsync(staff.Id);

                var appointmentsByDay = appointmentsStaff.GroupBy(a => a.DateAndTime.DateAndTime.Date).ToList();

                foreach (var appointmentsDay in appointmentsByDay)
                {
                    var staffAppointmentsDto = new StaffAppointmentsDto
                    {
                        StaffID = staff.Id.Value.ToString(),  
                        Day = appointmentsDay.Key.ToString("yyyyMMdd"), 
                        AppointmentsStaff = new List<AppointmentStaffDto>()
                    };

                    foreach (var appointment in appointmentsDay)
                    {
                        var operationType = await _operationTypeRepo.GetByIdAsync(appointment.OperationRequest.OperationTypeId);

                        if (operationType != null)
                        {
                            var startMinutes = appointment.DateAndTime.DateAndTime.Hour * 60 + appointment.DateAndTime.DateAndTime.Minute;
                            var operationDuration = operationType.EstimatedDuration.Minutes;

                            var appointmentStaffDto = new AppointmentStaffDto
                            {
                                AppointmentId = appointment.Id.AsGuid(), 
                                StartTime = startMinutes,  
                                EndTime = startMinutes + operationDuration
                            };

                            staffAppointmentsDto.AppointmentsStaff.Add(appointmentStaffDto);
                        }
                    }

                    staffAppointmentsList.Add(staffAppointmentsDto);
                }
            }
            return staffAppointmentsList;
        }
        public async Task<List<StaffDto>> GetAllAsync()
        {
            try
            {
                List<Staff> staffs = await this._repo.GetAllAsync();

                List<StaffDto> staffsDto = staffs.ConvertAll<StaffDto>(staff => new StaffDto(staff));

                return staffsDto;
            }
            catch (BusinessRuleValidationException)
            {
                return new List<StaffDto>();
            }
        }
    }
}