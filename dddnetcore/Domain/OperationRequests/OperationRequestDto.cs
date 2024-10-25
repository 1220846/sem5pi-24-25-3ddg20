using System;
using DDDSample1.Domain.Staffs;
using Microsoft.Net.Http.Headers;

namespace DDDSample1.Domain.OperationRequests
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

        public OperationRequestDto() {}

        public OperationRequestDto(OperationRequest operationRequest) {
            this.Id = operationRequest.Id.AsGuid();
            this.OperationTypeId = operationRequest.OperationTypeId.AsGuid().ToString();
            this.MedicalRecordNumber = operationRequest.MedicalRecordNumber.Id;
            this.Deadline = operationRequest.DeadlineDate.Date.ToString();
            this.Priority = operationRequest.Priority.ToString();
            this.Status = operationRequest.Status.ToString();
        }
    }
}
