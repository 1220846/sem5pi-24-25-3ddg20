using System;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.DataAnnotations.Patients;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Users;
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

        public async Task<int> CountNewPatientsMonthAsync(string targetMonth)
        {
            var count = await _context.Patients.Where(p => p.Id.AsString().Substring(0, 6) == targetMonth).CountAsync();

            return count;
        }

        public async Task<Patient> GetByEmailAsync(string email)
        {
            var patients = await _context.Patients.ToListAsync();
    
            var patient = patients.SingleOrDefault(p => p.ContactInformation != null && 
                p.ContactInformation.Email != null && p.ContactInformation.Email.Email == email);

            return patient;
        }

        public async Task<Patient> GetByUserIdAsync(string username)
        {
            var patient = await _context.Patients.Where(p => p.Username == new Username(username)).SingleOrDefaultAsync();       

            return patient;
        }

        public async Task<Patient> UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            
            await _context.SaveChangesAsync();

            return patient;
        }
    }
}