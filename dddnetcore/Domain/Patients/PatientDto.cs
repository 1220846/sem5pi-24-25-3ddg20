using System;
using System.Collections.Generic;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Users;


namespace dddnetcore.Domain.Patients
{
    public class PatientDto
    {
        public string Id {get;set;}
        public string AppointmentHistory{get; set;}
        public string DateOfBirth{get; set;}
        public string EmergencyContact{get; set;}
        public string Gender{get; set;}
        public string MedicalConditions{get; set;}
        public string PhoneNumber {get;set;}
        public string Email {get;set;}
        public string FirstName{get; set;}
        public string LastName{get; set;}
        public string FullName{get; set;}

        public PatientDto(Patient patient) {
            this.Id = patient.Id.Id;
            this.FirstName = patient.FirstName.Name;
            this.LastName = patient.LastName.Name;
            this.FullName = patient.FullName.Name;
            this.Email = patient.ContactInformation.Email.Email;
            this.PhoneNumber = patient.ContactInformation.PhoneNumber.PhoneNumber;
            this.AppointmentHistory = patient.AppointmentHistory.History;
            this.EmergencyContact = patient.EmergencyContact.PhoneNumber;
            this.Gender = EnumDescription.GetEnumDescription(patient.Gender);
            this.MedicalConditions = patient.MedicalConditions.Conditions;
            this.DateOfBirth = patient.DateOfBirth.Date.ToString("dd/MM/yyyy");
        }
    }
}