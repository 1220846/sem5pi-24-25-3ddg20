using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;

namespace DDDSample1.Domain.AppointmentsStaffs
{
    public interface IAppointmentStaffRepository : IRepository<AppointmentStaff, AppointmentStaffId>
    {
        Task<bool> IsStaffAvailableAsync(StaffId staffId, DateTime startTime, DateTime endTime, Guid? excludedAppointmentId = null);

    }
}