using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Appointments;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnetcore.Infraestructure.AvailabilitySlots
{
    public class AppointmentRepository : BaseRepository<Appointment, AppointmentId>, IAppointmentRepository 
    {
        private readonly DDDSample1DbContext _context;

        public AppointmentRepository(DDDSample1DbContext context):base(context.Appointments) {
            _context = context;
        }

        public async Task<List<Appointment>> GetAppointmentsByMedicalRecordNumberAsync(string medicalRecordNumber)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.OperationRequest)
                .Where(a => a.OperationRequest.MedicalRecordNumber.Value == medicalRecordNumber)
                .ToListAsync();
        }
    }
}