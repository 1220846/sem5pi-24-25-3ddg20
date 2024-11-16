using System;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.SurgeryRooms;

namespace DDDSample1.Domain.Appointments{

    public class AppointmentDto{
        public Guid Id  {get;set;}
        public SurgeryRoomDto SurgeryRoomDto {get;set;}
        public OperationRequestWithAllDataDto OperationRequestDto {get;set;}
        public string Status {get;set;}
        public DateTime DateAndTime {get;set;}
    }
}