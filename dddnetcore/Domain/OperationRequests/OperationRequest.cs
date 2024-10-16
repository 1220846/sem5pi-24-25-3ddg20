using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.OperationTypes;

namespace DDDSample1.Domain.OperationRequests{

    public class OperationRequest:Entity<OperationRequestId>, IAggregateRoot{

        public DeadlineDate deadlineDate {get;private set;}

        public OperationType operationType{get;private set;}

        public Priority priority{get;private set;}

        public MedicalRecordNumber medicalRecordNumber{get;private set;}

        public Status status{get;private set;}
        public StaffId staffId{get;private set;}
        public OperationRequest(MedicalRecordNumber patientId, StaffId staffId, OperationTypeId operationTypeId, DeadlineDate deadlineDate, Priority priority)
        {
            this.Id=new OperationRequestId(new Guid());
            this.deadlineDate=deadlineDate;
            this.priority=priority;
            this.medicalRecordNumber=patientId;
            this.staffId=staffId;
            this.status=Status.ONWAITINGLIST;
        }

        public OperationRequest()
        {
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (OperationRequest)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}