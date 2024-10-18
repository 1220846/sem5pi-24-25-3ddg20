using System;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.DataAnnotations.Patients;
using DDDSample1.Domain.Patients;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.Patients
{
    public class PatientRepository : BaseRepository<Patient,MedicalRecordNumber> , IPatientRepository{

        private readonly DDDSample1DbContext _context;
        
        public PatientRepository(DDDSample1DbContext context):base(context.Patients)
        {
            _context = context;
        }

        public async Task<int> CountNewPatientsMonthAsync(DateTime date)
        {
            var targetMonth = date.ToString("yyyyMM");
            var count = await _context.Patients.Where(p => p.Id.AsString().Substring(0, 6) == targetMonth).CountAsync();

            return count;
        }

        
    }
}