using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Appointments{

    public class CreatingAppointmentDto{

        public string SurgeryRoomId {get;set;}
        public string OperationRequestId {get;set;}
        public string DateAndTime {get;set;}
        public List<string> StaffsIds {get;set;}

    }
}