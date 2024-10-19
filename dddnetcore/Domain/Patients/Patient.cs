using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Patients{
    public class Patient : Entity<MedicalRecordNumber>, IAggregateRoot{
        public AppointmentHistory AppointmentHistory{get; private set;}
        public DateOfBirth DateOfBirth{get; private set;}
        public EmergencyContact EmergencyContact{get; private set;}
        public Gender Gender{get; private set;}
        public MedicalConditions MedicalConditions{get; private set;}
        public PatientContactInformation ContactInformation{get; private set;}
        public PatientFirstName FirstName{get; private set;}
        public PatientLastName LastName{get; private set;}
        public PatientFullName FullName{get; private set;}

        public User User {get; private set;}
        public Username Username {get; private set;}
        
        public Patient(AppointmentHistory appointmentHistory, DateOfBirth dateOfBirth, EmergencyContact emergencyContact,
            Gender gender, MedicalConditions medicalConditions, PatientContactInformation contactInformation, 
            PatientFirstName firstName, PatientLastName lastName, PatientFullName fullName, User user){
            this.AppointmentHistory = appointmentHistory;
            this.DateOfBirth = dateOfBirth;
            this.EmergencyContact = emergencyContact;
            this.Gender = gender;
            this.MedicalConditions = medicalConditions;
            this.ContactInformation = contactInformation;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = fullName;
            this.User = user;
        }

        public Patient(){
        }
        

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (Patient)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode(){
            return Id.GetHashCode();
        }

        public void ChangeFirstName(PatientFirstName newPatientFirstName){
            ArgumentNullException.ThrowIfNull(newPatientFirstName);
            this.FirstName = newPatientFirstName;
        }

        public void ChangeLastName(PatientLastName newPatientLastName){
            ArgumentNullException.ThrowIfNull(newPatientLastName);
            this.LastName = newPatientLastName;
        }

        public void ChangeFullName(PatientFullName newPatientFullName){
            ArgumentNullException.ThrowIfNull(newPatientFullName);
            this.FullName = newPatientFullName;
        }

        public void ChangeEmail(PatientEmail newPatientEmail){
            ArgumentNullException.ThrowIfNull(newPatientEmail);
            ContactInformation.ChangeEmail(newPatientEmail);
        }

        public void ChangePhoneNumber(PatientPhone newPatientPhoneNumber){
            ArgumentNullException.ThrowIfNull(newPatientPhoneNumber);
            ContactInformation.ChangePhoneNumber(newPatientPhoneNumber);
        }

        public void UpdateUser(User newUser){
            ArgumentNullException.ThrowIfNull(newUser);
            this.User = newUser;
        }
    }
}