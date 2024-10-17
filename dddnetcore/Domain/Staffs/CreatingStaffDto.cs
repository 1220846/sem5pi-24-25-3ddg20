using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Specializations;

namespace dddnetcore.Domain.Staffs
{
    public class CreatingStaffDto
    {
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string FullName {get;set;}
        public string Email {get;set;}
        public string PhoneNumber {get;set;}
        public string LicenseNumber {get;set;}
        public string SpecializationId {get;set;}
        public string UserEmail {get;set;}
    }
}