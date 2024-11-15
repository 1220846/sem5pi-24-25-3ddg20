using System;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.SurgeryRooms;

namespace DDDSample1.Domain.Appointments{

    public class AppointmentDto{
        public Guid Id  {get;set;}
        public SurgeryRoomDto SurgeryRoomDto {get;set;}
        public OperationRequestDto OperationRequestDto {get;set;}
        public string Status {get;set;}
        public DateTime DateAndTime {get;set;}
    }
}