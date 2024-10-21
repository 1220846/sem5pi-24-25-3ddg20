using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.OperationTypes;

namespace DDDSample1.Domain.OperationRequests{

    public class OperationRequest:Entity<OperationRequestId>, IAggregateRoot{

        public DeadlineDate DeadlineDate {get;private set;}

        public OperationTypeId OperationTypeId{get;private set;}

        public Priority Priority{get;private set;}

        public MedicalRecordNumber MedicalRecordNumber{get;private set;}

        public OperationRequestStatus Status{get;private set;}
        public StaffId StaffId{get;private set;}
        public OperationRequest(MedicalRecordNumber patientId, StaffId staffId, OperationTypeId operationTypeId, DeadlineDate DeadlineDate, Priority priority)
        {
            this.Id=new OperationRequestId(new Guid());
            this.DeadlineDate=DeadlineDate;
            this.Priority=priority;
            this.OperationTypeId=operationTypeId;
            this.MedicalRecordNumber=patientId;
            this.StaffId=staffId;
            this.Status=OperationRequestStatus.WAITING;
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