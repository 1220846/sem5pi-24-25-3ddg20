using System;
using System.Collections.Generic;

namespace dddnetcore.Domain.Appointments
{
    public class AppointmentStaffDto
    {
        public Guid AppointmentId {get;set;}
        public int StartTime {get;set;}
        public int EndTime {get;set;}
    }
}