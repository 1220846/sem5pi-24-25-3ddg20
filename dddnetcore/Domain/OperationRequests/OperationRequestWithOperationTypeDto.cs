using System;
using dddnetcore.Domain.Staffs;
using DDDSample1.Domain.OperationTypes;

namespace DDDSample1.Domain.OperationRequests
{
    public class OperationRequestWithAllDataDto
    {
        public Guid Id  {get;set;}
        
        public string DoctorId {get; set;}

        public OperationTypeDto OperationType {get;set;}

        public string MedicalRecordNumber{get;set;}

        public string Deadline {get;set;}

        public string Priority {get;set;}

        public string Status{get;set;}
    }
}
