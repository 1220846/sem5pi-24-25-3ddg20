using DDDSample1.Domain.AppointmentsStaffs;
using DDDSample1.Infrastructure.Shared;
using System.Threading.Tasks;
using DDDSample1.Domain.Staffs;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DDDSample1.Domain.Appointments;

namespace DDDSample1.Infrastructure.AppointmentsStaffs
{
    public class AppointmentStaffRepository : BaseRepository<AppointmentStaff,AppointmentStaffId> ,IAppointmentStaffRepository{
        
        private readonly DDDSample1DbContext _context;
        public AppointmentStaffRepository(DDDSample1DbContext context):base(context.AppointmentsStaffs)
        {
            _context = context;
        }

        public async Task<bool> IsStaffAvailableAsync(StaffId staffId, DateTime startTime, DateTime endTime)
        {

            var appointmentStaffs = await _context.AppointmentsStaffs
                .Include(a => a.Appointment)
                .Where(a => a.Staff.Id == staffId)
                .ToListAsync();

            if (appointmentStaffs == null || !appointmentStaffs.Any())
            {
                return true; 
            }

            var operationTypeIds = appointmentStaffs
                .Where(a => a.Appointment != null &&
                            a.Appointment.Status != AppointmentStatus.CANCELED &&
                            a.Appointment.Status != AppointmentStatus.COMPLETED &&
                            a.Appointment.OperationRequest != null)
                .Select(a => a.Appointment.OperationRequest.OperationTypeId)
                .Distinct()
                .ToList();

            var operationTypes = await _context.OperationTypes
                .Where(o => operationTypeIds.Contains(o.Id))
                .ToDictionaryAsync(o => o.Id, o => o.EstimatedDuration);

            foreach (var appointmentStaff in appointmentStaffs)
            {
                var appointment = appointmentStaff.Appointment;

                if (appointment == null ||
                    appointment.Status == AppointmentStatus.CANCELED ||
                    appointment.Status == AppointmentStatus.COMPLETED)
                {
                    continue;
                }

                var operationRequest = appointment.OperationRequest;

                if (operationRequest == null || !operationTypes.ContainsKey(operationRequest.OperationTypeId))
                {
                    throw new NullReferenceException("OperationType not found for an operation request.");
                }

                var duration = operationTypes[operationRequest.OperationTypeId];

                var staffAppointmentStartTime = appointment.DateAndTime.DateAndTime;
                var staffAppointmentEndTime = staffAppointmentStartTime.AddMinutes(duration.Minutes);

                if (startTime < staffAppointmentEndTime && endTime > staffAppointmentStartTime)
                {
                    return false; 
                }
            }
            return true;
        }
    }
}