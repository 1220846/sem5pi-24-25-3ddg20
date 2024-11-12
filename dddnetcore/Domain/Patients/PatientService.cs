using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.DataAnnotations.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Emails;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.SystemLogs;
using Microsoft.AspNetCore.Mvc;
using Auth0.ManagementApi;
using DDDSample1.Domain.Auth;

namespace dddnetcore.Domain.Patients
{
    public class PatientService
    {
        IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _repo;
        private readonly IUserRepository _repoUser;
        private readonly IEmailService _emailService;
        private readonly IAnonymizedPatientDataRepository _repoAnonymizedPatientData;
        private readonly ISystemLogRepository _repoSystemLog;
        private readonly AuthenticationService _authenticationService;
        private readonly ManagementApiClient _managementApiClient;
        private readonly string  _domain;
        private readonly string  _clientId;
        private readonly string  _clientSecret;
        private readonly string _accessToken;

        public PatientService(IUnitOfWork unitOfWork, IPatientRepository repo, IUserRepository repoUser, IEmailService emailService, IAnonymizedPatientDataRepository anonymizedPatientDataRepository, ISystemLogRepository systemLogRepository, AuthenticationService authenticationService) {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._repoUser = repoUser;
            this._emailService = emailService;
            this._repoSystemLog = systemLogRepository;
            this._repoAnonymizedPatientData = anonymizedPatientDataRepository;

            this._authenticationService = authenticationService;
            _domain = Environment.GetEnvironmentVariable("Auth0_Domain");
            _clientId = Environment.GetEnvironmentVariable("Auth0_ClientId");
            _clientSecret = Environment.GetEnvironmentVariable("Auth0_ClientSecret");
            _accessToken = _authenticationService.GetAccessToken(_domain,$"https://{_domain}/api/v2/", _clientId, _clientSecret).Result;
            _managementApiClient = new ManagementApiClient(_accessToken, _domain);
        }   

        public async Task<PatientDto> GetByIdAsync(MedicalRecordNumber id)
        {
            var patient = await this._repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Not Found Patient with Id: {id}");
            return new PatientDto(patient);
        }

        public async Task<PatientDto> GetByEmail(string patientEmail)
        {
            var patient = await this._repo.GetByEmailAsync(patientEmail) ?? throw new NullReferenceException($"Not Found Patient with email: {patientEmail}");
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

        public async Task<PatientDto> EditPatientAsync(string id, EditingPatientDto dto){
            bool changedContactInfo = false;
            Patient patient = await _repo.GetByIdAsync(new MedicalRecordNumber(id)) ?? throw new NullReferenceException("Patient not found");
            PatientEmail previousEmail = patient.ContactInformation.Email;
            var patientChanges=new List<string>();
            
            if (dto.AppointmentHistory != null){
                patientChanges.Add($"Appointment history was updated");
                patient.ChangeAppointmentHistory(new AppointmentHistory(dto.AppointmentHistory));
            }
            if (dto.MedicalConditions != null){
                patientChanges.Add($"Medical conditions / allergies was updated");
                patient.ChangeMedicalConditions(new MedicalConditions(dto.MedicalConditions));
            }
            if (dto.FirstName != null){
                patientChanges.Add($"First Name was updated from  {patient.FirstName.Name} to {dto.FirstName}");
                patient.ChangeFirstName(new PatientFirstName(dto.FirstName));
            }
            if (dto.LastName != null){
                patientChanges.Add($"Last Name was updated from  {patient.LastName.Name} to {dto.LastName}");
                patient.ChangeLastName(new PatientLastName(dto.LastName));
            }
            if (dto.FullName != null){
                patientChanges.Add($"Full Name was updated from  {patient.FullName.Name} to {dto.FullName}");
                patient.ChangeFullName(new PatientFullName(dto.FullName));
            }
            if (dto.Email != null){
                patientChanges.Add($"Email was updated from  {patient.ContactInformation.Email.Email} to {dto.Email}");
                patient.ChangeEmail(new PatientEmail(dto.Email));
                changedContactInfo = true;
            }

            if (dto.PhoneNumber != null){
                patientChanges.Add($"Phone Number was updated from  {patient.ContactInformation.PhoneNumber.PhoneNumber} to {dto.PhoneNumber}");
                patient.ChangePhoneNumber(new PatientPhone(dto.PhoneNumber));
                changedContactInfo = true;
            }

            await this._repo.UpdateAsync(patient);
            if (patientChanges.Count>0){
                string logMessage = string.Join(", ", patientChanges);
                await this._repoSystemLog.AddAsync(new SystemLog(Operation.UPDATE, Entity.PATIENT, logMessage, patient.Id.Id));
            }
            await this._unitOfWork.CommitAsync();

            if (changedContactInfo) {

                var to = new List<string>{previousEmail.Email};
                var subject = "Contact information change.";
                var body = $@"
                <p>Dear {patient.FullName.Name},Your contact information has been changed:</p>
                <p>Email Address: {patient.ContactInformation.Email.Email}.</p>
                <p>Phone Number: {patient.ContactInformation.PhoneNumber.PhoneNumber}.</p>";
                
                await _emailService.SendEmailAsync(to, subject, body);
            }
            return new PatientDto(patient);
        }

        
        public async Task<PatientDto> DeletePatientAsync(string id)
        {
            var patient = await _repo.GetByIdAsync(new MedicalRecordNumber(id))?? throw new NullReferenceException("Not Found patient with: " + id);

            User user = null;
            if (patient.User!=null)
                user = await _repoUser.GetByIdAsync(patient.User.Id);

            var anonymizedPatientData = new AnonymizedPatientData(
                CalculateAgeRange(patient.DateOfBirth), 
                EnumDescription.GetEnumDescription(patient.Gender),
                patient.MedicalConditions.Conditions,
                patient.AppointmentHistory.History);

            await this._repoAnonymizedPatientData.AddAsync(anonymizedPatientData);

            this._repo.Remove(patient);
            string patientLogMessage = $"Patient '{id}' was deleted for GDPR compliance.";
            await this._repoSystemLog.AddAsync(new SystemLog(Operation.DELETE, Entity.PATIENT, patientLogMessage, patient.Id.Id));
            if(user!=null){
                this._repoUser.Remove(user);

                try{

                    await _managementApiClient.Users.DeleteAsync($"auth0|{user.Id.Name}");

                }catch (Exception ex){
                    throw new Exception($"Error when deleting user '{user.Id.AsString}': {ex.Message}", ex);
                }

                string userLogMessage = $"User associated with patient '{id}' was deleted for GDPR compliance.";
                await this._repoSystemLog.AddAsync(new SystemLog(Operation.DELETE, Entity.USER, userLogMessage, user.Id.Name));
            }

            await this._unitOfWork.CommitAsync();

            if (user!=null){
                var to = new List<string> { user.Email.Address };
                var subject = "Account Deleted";
                var body = "Your account has been deleted";
            
                await _emailService.SendEmailAsync(to,subject,body);
            }
            return new PatientDto(patient);
        }
        private string CalculateAgeRange(DateOfBirth dateOfBirth){
            var age = DateTime.Now.Year - dateOfBirth.Date.Year;

            return age switch{
                < 18 => "Under 18",
                >= 18 and <= 35 => "18-35",
                > 35 and <= 50 => "36-50",
                > 50 and <= 65 => "51-65",
                _ => "65+"
            };
        }

        internal async Task<ActionResult<IEnumerable<PatientDto>>> GetPatientsAsync(string firstName = null, string lastName = null, string fullName = null, string email = null, string birthDate = null, string phoneNumber = null, string id = null, string gender = null, int pageNumber = 1, int pageSize = 10)
        {
            try {
            List<Patient> patients = await this._repo.GetPatientsAsync(firstName, lastName, fullName, email, birthDate, phoneNumber, id, gender, pageNumber, pageSize);
            
            List<PatientDto> patientDto = patients.ConvertAll<PatientDto>(patient => new PatientDto(patient));
            
            return patientDto;
            } catch (BusinessRuleValidationException) {
                return new List<PatientDto>();
            }
        }

        public async Task<int> GetPatientsCountAsync()
        {
            try
            {
                return await _repo.GetPatientsCountAsync(); // Método que faz a contagem no repositório
            }
            catch (Exception ex)
            {
                throw new Exception("Error counting patients", ex);
            }
        }
    }
}