using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.Staffs;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.DataAnnotations.Staffs;
using DDDSample1.Domain.AppointmentsStaffs;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.RoomTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.SurgeryRooms;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure.AppointmentsStaffs;

namespace DDDSample1.Domain.Appointments
{
    public class AppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentRepository _repo;
        private readonly ISurgeryRoomRepository _surgeryRoomRepo;
        private readonly IOperationRequestRepository _operationRequestRepo;
        private readonly IOperationTypeRepository _operationTypeRepo;
        private readonly IStaffRepository _staffRepo;
        private readonly IAppointmentStaffRepository _appointmentStaffRepo;
        public AppointmentService(IUnitOfWork unitOfWork, IAppointmentRepository repo, ISurgeryRoomRepository surgeryRoomRepository, IOperationRequestRepository operationRequestRepository, IOperationTypeRepository operationTypeRepository,IStaffRepository staffRepository, IAppointmentStaffRepository appointmentStaffRepository)
        {

            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._surgeryRoomRepo = surgeryRoomRepository;
            this._operationRequestRepo = operationRequestRepository;
            this._operationTypeRepo = operationTypeRepository;
            this._staffRepo = staffRepository;
            this._appointmentStaffRepo = appointmentStaffRepository; 
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();

            List<AppointmentDto> listDto = new List<AppointmentDto>();

            foreach (var appointment in list)
            {
                var operationRequest = await this._operationRequestRepo.GetByIdAsync(new OperationRequestId(appointment.OperationRequest.Id.Value));

                var operationType = await this._operationTypeRepo.GetByIdAsync(operationRequest.OperationTypeId);

                var appointmentDto = new AppointmentDto
                {
                    Id = appointment.Id.AsGuid(),
                    SurgeryRoomDto = new SurgeryRoomDto
                    {
                        Number = appointment.SurgeryRoom.Id.Value,
                        RoomType = new RoomTypeDto {Id = appointment.SurgeryRoom.RoomType.Id.AsGuid(),Name = appointment.SurgeryRoom.RoomType.Name.Name},
                        Capacity = appointment.SurgeryRoom.RoomCapacity.Capacity,
                        CurrentStatus = EnumDescription.GetEnumDescription(appointment.SurgeryRoom.CurrentStatus),
                        MaintenanceSlots = appointment.SurgeryRoom.MaintenanceSlots.MaintenanceSlots,
                        AssignedEquipment = appointment.SurgeryRoom.AssignedEquipment.AssignedEquipment
                    },
                    OperationRequestDto = new OperationRequestWithAllDataDto
                    {
                        Id = appointment.OperationRequest.Id.AsGuid(),
                        DoctorId = appointment.OperationRequest.StaffId.Id,
                        OperationType = new OperationTypeDto
                        {
                            Id = operationType.Id.AsGuid(),
                            Name = operationType.Name.Name,
                            EstimatedDuration = operationType.EstimatedDuration.Minutes,
                            SurgeryTime = operationType.SurgeryTime.Minutes,
                            AnesthesiaTime = operationType.AnesthesiaTime.Minutes,
                            CleaningTime = operationType.CleaningTime.Minutes
                        },
                        MedicalRecordNumber = appointment.OperationRequest.MedicalRecordNumber.Id,
                        Deadline = appointment.OperationRequest.DeadlineDate.Date.ToString(),
                        Priority = appointment.OperationRequest.Priority.ToString(),
                        Status = appointment.OperationRequest.Status.ToString()
                    },
                    Status = EnumDescription.GetEnumDescription(appointment.Status),
                    DateAndTime = appointment.DateAndTime.DateAndTime,
                };

                listDto.Add(appointmentDto);
            }
            return listDto;
        }

        public async Task<AppointmentDto> GetByIdAsync(AppointmentId id)
        {
            var appointment = await this._repo.GetByIdAsync(id);

            if (appointment == null)
                return null;

            var operationRequest = await this._operationRequestRepo.GetByIdAsync(appointment.OperationRequest.Id);

            var operationType = await this._operationTypeRepo.GetByIdAsync(operationRequest.OperationTypeId);

            return new AppointmentDto
            {
                Id = appointment.Id.AsGuid(),
                SurgeryRoomDto = new SurgeryRoomDto
                {
                    Number = appointment.SurgeryRoom.Id.Value,
                    RoomType = new RoomTypeDto {Id = appointment.SurgeryRoom.RoomType.Id.AsGuid(),Name = appointment.SurgeryRoom.RoomType.Name.Name},
                    Capacity = appointment.SurgeryRoom.RoomCapacity.Capacity,
                    CurrentStatus = EnumDescription.GetEnumDescription(appointment.SurgeryRoom.CurrentStatus),
                    MaintenanceSlots = appointment.SurgeryRoom.MaintenanceSlots.MaintenanceSlots,
                    AssignedEquipment = appointment.SurgeryRoom.AssignedEquipment.AssignedEquipment
                },
                OperationRequestDto = new OperationRequestWithAllDataDto
                {
                    Id = appointment.OperationRequest.Id.AsGuid(),
                    DoctorId = appointment.OperationRequest.StaffId.Id,
                    OperationType = new OperationTypeDto
                    {
                        Id = operationType.Id.AsGuid(),
                        Name = operationType.Name.Name,
                        EstimatedDuration = operationType.EstimatedDuration.Minutes,
                    },
                    MedicalRecordNumber = appointment.OperationRequest.MedicalRecordNumber.Id,
                    Deadline = appointment.OperationRequest.DeadlineDate.Date.ToString(),
                    Priority = appointment.OperationRequest.Priority.ToString(),
                    Status = appointment.OperationRequest.Status.ToString()
                },
                Status = EnumDescription.GetEnumDescription(appointment.Status),
                DateAndTime = appointment.DateAndTime.DateAndTime,
            };
        }

        public async Task<AppointmentDto> AddAsync(CreatingAppointmentDto dto)
        {

            var surgeryRoom = await _surgeryRoomRepo.GetByIdAsync(new RoomNumber(dto.SurgeryRoomId)) ?? throw new NullReferenceException("Not Found Surgery Room: " + dto.SurgeryRoomId);

            var operationRequest = await _operationRequestRepo.GetByIdAsync(new OperationRequestId(dto.OperationRequestId)) ?? throw new NullReferenceException("Not Found Operation Request: " + dto.SurgeryRoomId);
            
            var operationType = await this._operationTypeRepo.GetByIdAsync(new OperationTypeId(operationRequest.OperationTypeId.Value)) ?? throw new NullReferenceException("Not Found Operation Type: " + operationRequest.OperationTypeId.Value);
            
            var startTime = DateTime.Parse(dto.DateAndTime);
            var endTime = startTime.AddMinutes(operationType.EstimatedDuration.Minutes);
    
            var isRoomAvailable = await _surgeryRoomRepo.IsRoomAvailableAsync(surgeryRoom.Id, startTime, endTime);

            if (!isRoomAvailable)
            {
                throw new BusinessRuleValidationException("The surgery room is not available for the selected time.");
            }

            // Validate Staff Specialization
            await ValidateStaffSpecializationsAsync(dto.StaffsIds, operationType);

            var isStaffAvailable = true;
            foreach (var staffId in dto.StaffsIds)
            {
                var staffIdObj = new StaffId(staffId);
                isStaffAvailable &= await _appointmentStaffRepo.IsStaffAvailableAsync(staffIdObj, startTime, endTime);
            }

            if (!isStaffAvailable)
            {
                throw new BusinessRuleValidationException("One or more staff members are not available for the selected time.");
            }

            var appointment = new Appointment(surgeryRoom, operationRequest, new AppointmentDateAndTime(DateTime.Parse(dto.DateAndTime)));

            await this._repo.AddAsync(appointment);

            foreach (var staffId in dto.StaffsIds)
            {
                var staff = await _staffRepo.GetByIdAsync(new StaffId(staffId)) 
                            ?? throw new NullReferenceException("Staff not found: " + staffId);

                var appointmentStaff = new AppointmentStaff(appointment, staff);
                await _appointmentStaffRepo.AddAsync(appointmentStaff);
            }
            
            // Update status of operation Request to scheduled
            operationRequest.ChangeStatus(OperationRequestStatus.SCHEDULED);
            await this._operationRequestRepo.UpdateAsync(operationRequest);

            await this._unitOfWork.CommitAsync();

            return new AppointmentDto
            {
                Id = appointment.Id.AsGuid(),
                SurgeryRoomDto = new SurgeryRoomDto
                {
                    Number = appointment.SurgeryRoom.Id.Value,
                    RoomType = new RoomTypeDto {Id = appointment.SurgeryRoom.RoomType.Id.AsGuid(),Name = appointment.SurgeryRoom.RoomType.Name.Name},
                    Capacity = appointment.SurgeryRoom.RoomCapacity.Capacity,
                    CurrentStatus = EnumDescription.GetEnumDescription(appointment.SurgeryRoom.CurrentStatus),
                    MaintenanceSlots = appointment.SurgeryRoom.MaintenanceSlots.MaintenanceSlots,
                    AssignedEquipment = appointment.SurgeryRoom.AssignedEquipment.AssignedEquipment
                },
                OperationRequestDto = new OperationRequestWithAllDataDto
                {
                    Id = appointment.OperationRequest.Id.AsGuid(),
                    DoctorId = appointment.OperationRequest.StaffId.Id,
                    OperationType = new OperationTypeDto
                    {
                        Id = operationType.Id.AsGuid(),
                        Name = operationType.Name.Name,
                        EstimatedDuration = operationType.EstimatedDuration.Minutes,
                    },
                    MedicalRecordNumber = appointment.OperationRequest.MedicalRecordNumber.Id,
                    Deadline = appointment.OperationRequest.DeadlineDate.Date.ToString(),
                    Priority = appointment.OperationRequest.Priority.ToString(),
                    Status = appointment.OperationRequest.Status.ToString()
                },
                Status = EnumDescription.GetEnumDescription(appointment.Status),
                DateAndTime = appointment.DateAndTime.DateAndTime,
            };
        }
        private async Task ValidateStaffSpecializationsAsync(List<string> staffIds, OperationType operationType)
        {
            var requiredSpecializations = operationType.OperationTypeSpecializations;

            var staffSpecializationCount = new Dictionary<Specialization, int>();

            foreach (var staffId in staffIds)
            {
                var staff = await _staffRepo.GetByIdAsync(new StaffId(staffId))
                    ?? throw new NullReferenceException($"Staff with ID {staffId} not found.");

                var staffSpecialization = staff.Specialization;

                var requiredSpecialization = requiredSpecializations
                    .FirstOrDefault(rs => rs.Specialization.Id == staffSpecialization.Id);

                if (requiredSpecialization == null)
                {
                    throw new BusinessRuleValidationException($"Staff {staffId} does not have the required specialization.");
                }
                if (!staffSpecializationCount.ContainsKey(staffSpecialization))
                {
                    staffSpecializationCount[staffSpecialization] = 1;
                }
                else
                {
                    staffSpecializationCount[staffSpecialization]++;
                }
            }

            foreach (var requiredSpecialization in requiredSpecializations)
            {
                if (staffSpecializationCount.ContainsKey(requiredSpecialization.Specialization))
                {
                    if (staffSpecializationCount[requiredSpecialization.Specialization] < requiredSpecialization.NumberOfStaff.Number)
                    {
                        throw new BusinessRuleValidationException($"Not enough staff for the specialization {requiredSpecialization.Specialization.Name.Name}. Required: {requiredSpecialization.NumberOfStaff.Number}, Available: {staffSpecializationCount[requiredSpecialization.Specialization]}");
                    }
                }
                else
                {
                    throw new BusinessRuleValidationException($"No staff available for the specialization {requiredSpecialization.Specialization.Name.Name}.");
                }
            }
        }

        public async Task<List<AppointmentDto>> GetByPatientIdAsync(string medicalRecordNumber)
        {
            var list = await this._repo.GetByPatientIdAsync(new MedicalRecordNumber(medicalRecordNumber));

            List<AppointmentDto> listDto = new List<AppointmentDto>();

            foreach (var appointment in list)
            {
                var operationRequest = await this._operationRequestRepo.GetByIdAsync(new OperationRequestId(appointment.OperationRequest.Id.Value));

                var operationType = await this._operationTypeRepo.GetByIdAsync(operationRequest.OperationTypeId);

                var appointmentDto = new AppointmentDto
                {
                    Id = appointment.Id.AsGuid(),
                    SurgeryRoomDto = new SurgeryRoomDto
                    {
                        Number = appointment.SurgeryRoom.Id.Value,
                        RoomType = new RoomTypeDto {Id = appointment.SurgeryRoom.RoomType.Id.AsGuid(),Name = appointment.SurgeryRoom.RoomType.Name.Name},
                        Capacity = appointment.SurgeryRoom.RoomCapacity.Capacity,
                        CurrentStatus = EnumDescription.GetEnumDescription(appointment.SurgeryRoom.CurrentStatus),
                        MaintenanceSlots = appointment.SurgeryRoom.MaintenanceSlots.MaintenanceSlots,
                        AssignedEquipment = appointment.SurgeryRoom.AssignedEquipment.AssignedEquipment
                    },
                    OperationRequestDto = new OperationRequestWithAllDataDto
                    {
                        Id = appointment.OperationRequest.Id.AsGuid(),
                        DoctorId = appointment.OperationRequest.StaffId.Id,
                        OperationType = new OperationTypeDto
                        {
                            Id = operationType.Id.AsGuid(),
                            Name = operationType.Name.Name,
                            EstimatedDuration = operationType.EstimatedDuration.Minutes,
                            SurgeryTime = operationType.SurgeryTime.Minutes,
                            AnesthesiaTime = operationType.AnesthesiaTime.Minutes,
                            CleaningTime = operationType.CleaningTime.Minutes
                        },
                        MedicalRecordNumber = appointment.OperationRequest.MedicalRecordNumber.Id,
                        Deadline = appointment.OperationRequest.DeadlineDate.Date.ToString(),
                        Priority = appointment.OperationRequest.Priority.ToString(),
                        Status = appointment.OperationRequest.Status.ToString()
                    },
                    Status = EnumDescription.GetEnumDescription(appointment.Status),
                    DateAndTime = appointment.DateAndTime.DateAndTime,
                };

                listDto.Add(appointmentDto);
            }
            return listDto;
        }
    }
}