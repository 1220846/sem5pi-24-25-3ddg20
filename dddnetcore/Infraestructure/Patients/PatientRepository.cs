using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.DataAnnotations.Patients;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
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

        public async Task<string> LastPatientCreatedAsync()
        {
            var patients = await _context.Patients.ToListAsync();
             var lastPatient = patients.OrderByDescending(p => p.Id.Value).FirstOrDefault();


            return lastPatient?.Id?.Id;
        }

        public async Task<Patient> GetByEmailAsync(string email)
        {
            
            var patient = await _context.Patients.Include(p => p.ContactInformation).Where(p => p.ContactInformation != null &&p.ContactInformation.Email != null &&
            p.ContactInformation.Email.Equals(new PatientEmail(email))).SingleOrDefaultAsync();

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

        public async Task<List<Patient>> GetPatientsAsync(string firstName = null, string lastName = null, string fullName = null, string email = null, string birthDate = null, string phoneNumber = null, string id = null, string gender = null, int pageNumber = 1, int pageSize = 10)
        {
            try {
                var query = _context.Patients.AsQueryable();
    
                if (!string.IsNullOrEmpty(id))
                    query = query.Where(patient => patient.Id.Equals(new MedicalRecordNumber(id)));
                if (!string.IsNullOrEmpty(firstName))
                    query = query.Where(patient => patient.FirstName.Equals(new PatientFirstName(firstName)));
                if (!string.IsNullOrEmpty(lastName))
                    query = query.Where(patient => patient.LastName.Equals(new PatientLastName(lastName)));
                if (!string.IsNullOrEmpty(fullName))
                    query = query.Where(patient => patient.FullName.Equals(new PatientFullName(fullName)));
                if (!string.IsNullOrEmpty(email))
                    query = query.Where(patient => patient.ContactInformation.Email.Equals(new PatientEmail(email)));
                if (!string.IsNullOrEmpty(phoneNumber))
                    query = query.Where(patient => patient.ContactInformation.PhoneNumber.Equals(new PatientPhone(phoneNumber)));
                if (!string.IsNullOrEmpty(birthDate))
                    query = query.Where(patient => patient.DateOfBirth.Equals(new DateOfBirth(DateTime.Parse(birthDate))));           
                if (!string.IsNullOrEmpty(gender))
                    query = query.Where(patient => patient.Gender.Equals(Enum.Parse<Gender>(gender.ToUpper())));

                query = query.Include(o => o.User);

                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return await query.ToListAsync();

            } catch (BusinessRuleValidationException) {
                throw new BusinessRuleValidationException("Filters badly formatted!");
            }
        }
    }
}