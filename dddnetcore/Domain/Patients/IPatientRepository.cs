using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Patients;
using System;

namespace DDDSample1.DataAnnotations.Patients
{
    public interface IPatientRepository : IRepository<Patient, MedicalRecordNumber>
    {
        Task<string> LastPatientCreatedAsync();
        Task<Patient> GetByEmailAsync(string email);
        Task<Patient> GetByUserIdAsync(string username);
        Task<Patient> UpdateAsync(Patient patient);
    }
}