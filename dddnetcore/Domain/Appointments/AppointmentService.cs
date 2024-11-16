using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.SurgeryRooms;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Appointments
{
    public class AppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentRepository _repo;
        private readonly ISurgeryRoomRepository _surgeryRoomRepo;
        private readonly IOperationRequestRepository _operationRequestRepo;
        private readonly IOperationTypeRepository _operationTypeRepo;
        public AppointmentService(IUnitOfWork unitOfWork, IAppointmentRepository repo, ISurgeryRoomRepository surgeryRoomRepository, IOperationRequestRepository operationRequestRepository, IOperationTypeRepository operationTypeRepository)
        {

            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._surgeryRoomRepo = surgeryRoomRepository;
            this._operationRequestRepo = operationRequestRepository;
            this._operationTypeRepo = operationTypeRepository;
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
                        Type = EnumDescription.GetEnumDescription(appointment.SurgeryRoom.RoomType),
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
                    DateAndTime = appointment.DateAndTime.DateAndTime
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
                    Type = EnumDescription.GetEnumDescription(appointment.SurgeryRoom.RoomType),
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
                DateAndTime = appointment.DateAndTime.DateAndTime
            };
        }

        public async Task<AppointmentDto> AddAsync(CreatingAppointmentDto dto)
        {

            var surgeryRoom = await _surgeryRoomRepo.GetByIdAsync(new RoomNumber(dto.SurgeryRoomId)) ?? throw new NullReferenceException("Not Found Surgery Room: " + dto.SurgeryRoomId);

            var operationRequest = await _operationRequestRepo.GetByIdAsync(new OperationRequestId(dto.OperationRequestId)) ?? throw new NullReferenceException("Not Found Operation Request: " + dto.SurgeryRoomId);
            
            var operationType = await this._operationTypeRepo.GetByIdAsync(new OperationTypeId(operationRequest.OperationTypeId.Value)) ?? throw new NullReferenceException("Not Found Operation Type: " + operationRequest.OperationTypeId.Value);

            var appointment = new Appointment(surgeryRoom, operationRequest, new AppointmentDateAndTime(DateTime.Parse(dto.DateAndTime)));

            await this._repo.AddAsync(appointment);

            await this._unitOfWork.CommitAsync();

            return new AppointmentDto
            {
                Id = appointment.Id.AsGuid(),
                SurgeryRoomDto = new SurgeryRoomDto
                {
                    Number = appointment.SurgeryRoom.Id.Value,
                    Type = EnumDescription.GetEnumDescription(appointment.SurgeryRoom.RoomType),
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
                DateAndTime = appointment.DateAndTime.DateAndTime
            };
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
                        Type = EnumDescription.GetEnumDescription(appointment.SurgeryRoom.RoomType),
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
                    DateAndTime = appointment.DateAndTime.DateAndTime
                };

                listDto.Add(appointmentDto);
            }
            return listDto;
        }
    }
}