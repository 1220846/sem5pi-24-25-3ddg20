using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dddnetcore.Domain.Patients
{
    public class CreatingPatientDto
    {
        public string Address{get; set;}
        public string PostalCode{get; set;}
        public string DateOfBirth{get; set;}
        public string EmergencyContact{get; set;}
        public string Gender{get; set;}
        public string Email {get; set;}
        public string PhoneNumber {get; set;}
        public string FirstName{get; set;}
        public string LastName{get; set;}
        public string FullName{get; set;}
    }
}