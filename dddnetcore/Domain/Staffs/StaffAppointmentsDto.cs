using System.Collections.Generic;
using dddnetcore.Domain.Appointments;

namespace dddnetcore.Domain.Staffs
{
    public class StaffAppointmentsDto
    {
        public string StaffID {get;set;}
        public string Day {get;set;}
        public List<AppointmentStaffDto> AppointmentsStaff {get;set;}
    }
}