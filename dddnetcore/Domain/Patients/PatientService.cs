using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.DataAnnotations.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Patients;

namespace dddnetcore.Domain.Patients
{
    public class PatientService
    {
        IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _repo;

        public PatientService(IUnitOfWork unitOfWork, IPatientRepository repo) {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
        }   

        public async Task<PatientDto> GetByIdAsync(MedicalRecordNumber id)
        {
            var patient = await this._repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Not Found Patient with Id: {id}");
            return new PatientDto(patient);
        }
        public async Task<PatientDto> AddAsync(CreatingPatientDto dto)
        {

            if (dto.FirstName != null && dto.LastName != null && dto.FullName != null && dto.Email != null &&
            dto.PhoneNumber != null && dto.DateOfBirth != null && dto.EmergencyContact != null && dto.Gender != null)
            {
                string month = DateTime.Now.ToString("yyyyMM");
                string lastId = await _repo.LastPatientCreatedAsync();
                int newId = 1;
                if (!string.IsNullOrEmpty(lastId)){
                    string lastIdMonth = lastId[..6];
                    string lastIdNumber = lastId.Substring(6,6);
                    if (lastIdMonth == month){
                        newId = int.Parse(lastIdNumber) + 1;
                    }
                }
                string numberFormatted = newId.ToString("D6");
                string medicalRecordNumber = month + numberFormatted;
                var gender = Enum.Parse<Gender>(dto.Gender.ToUpper());
                var patient = new Patient(
                    new MedicalRecordNumber(medicalRecordNumber),
                    new AppointmentHistory(""),
                    new DateOfBirth(DateTime.Parse(dto.DateOfBirth)),
                    new EmergencyContact(dto.EmergencyContact),
                    gender,
                    new MedicalConditions(""),
                    new PatientContactInformation(new PatientEmail(dto.Email), new PatientPhone(dto.PhoneNumber)),
                    new PatientFirstName(dto.FirstName),
                    new PatientLastName(dto.LastName),
                    new PatientFullName(dto.FullName),
                    null
                );
                await this._repo.AddAsync(patient);
                await this._unitOfWork.CommitAsync();
                return new PatientDto(patient);
            }

            throw new ArgumentNullException("Missing data for patient creation!");
        }
    }
}