using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationRequests;

namespace DDDSample1.Domain.OperationRequests{

    public class OperationRequest:Entity<OperationRequestId>, IAggregateRoot{

        public DeadlineDate deadlineDate {get;private set;}

        public Priority priority{get;private set;}
        public OperationRequest(OperationRequestId id, DeadlineDate deadlineDate, Priority priority)
        {
            this.Id=id;
            this.deadlineDate=deadlineDate;
            this.priority=priority;
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