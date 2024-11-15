using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.SurgeryRooms;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Appointments
{
    public class AppointmentService{
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IUnitOfWork unitOfWork, IAppointmentRepository repo){

            this._unitOfWork = unitOfWork;
            this._repo = repo;
        }

        public async Task<List<AppointmentDto>> GetAllAsync() {
            var list = await this._repo.GetAllAsync();
            
            List<AppointmentDto> listDto = list.ConvertAll<AppointmentDto>(appointment => 
                new AppointmentDto{Id = appointment.Id.AsGuid(),
                SurgeryRoomDto = new SurgeryRoomDto {
                    Number = appointment.SurgeryRoom.Id.AsString(),
                    Type = EnumDescription.GetEnumDescription(appointment.SurgeryRoom.RoomType),
                    Capacity = appointment.SurgeryRoom.RoomCapacity.Capacity,
                    CurrentStatus = EnumDescription.GetEnumDescription(appointment.SurgeryRoom.CurrentStatus),
                    MaintenanceSlots = appointment.SurgeryRoom.MaintenanceSlots.MaintenanceSlots,
                    AssignedEquipment = appointment.SurgeryRoom.AssignedEquipment.AssignedEquipment
                }, 
                OperationRequestDto = new OperationRequestDto{Id = appointment.OperationRequest.Id.AsGuid(),
                        DoctorId = appointment.OperationRequest.StaffId.Id, 
                        OperationTypeId = appointment.OperationRequest.OperationTypeId.Value, 
                        MedicalRecordNumber= appointment.OperationRequest.MedicalRecordNumber.Id,
                        Deadline=appointment.OperationRequest.DeadlineDate.Date.ToString(), 
                        Priority = appointment.OperationRequest.Priority.ToString(), 
                        Status = appointment.OperationRequest.Status.ToString()},
                Status = EnumDescription.GetEnumDescription(appointment.Status),
                DateAndTime = appointment.DateAndTime.DateAndTime
                });
            return listDto;
        }
    }
}