using System;

namespace dddnetcore.Domain.Patients
{
    public class EditingPatientDto{
        public string Email {get;set;}
        public string PhoneNumber {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string FullName {get;set;}
        public string Address {get;set;}
        public string PostalCode {get;set;}
        public string MedicalConditions {get;set;}
    }
}