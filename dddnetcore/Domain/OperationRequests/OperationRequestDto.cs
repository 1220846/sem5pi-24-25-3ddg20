using System;
using DDDSample1.Domain.Staffs;
using Microsoft.Net.Http.Headers;

namespace DDDSample1.Domain.Users
{
    public class OperationRequestDto
    {
        public Guid Id  {get;set;}
        public string DoctorId {get; set;}

        public string OperationTypeId {get;set;}

        public string MedicalRecordNumber{get;set;}

        public string Deadline {get;set;}

        public string Priority {get;set;}

        public string Status{get;set;}
    }
}
