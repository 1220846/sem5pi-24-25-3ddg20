using System;
using System.Collections.Generic;
using DDDSample1.Domain.OperationTypesSpecializations;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationTypes{

    public class OperationType : Entity<OperationTypeId>, IAggregateRoot
    {
        public OperationTypeName Name { get;  private set; }
        public EstimatedDuration EstimatedDuration { get;  private set; }

        public AnesthesiaTime AnesthesiaTime {get; private set;}

        public CleaningTime CleaningTime {get; private set;}

        public SurgeryTime SurgeryTime {get; private set;}

        public OperationTypeStatus OperationTypeStatus {get;private set;}
        public ICollection<OperationTypeSpecialization> OperationTypeSpecializations {get; private set;} = new List<OperationTypeSpecialization>();

        private OperationType() { }

        public OperationType(OperationTypeName name, EstimatedDuration estimatedDuration,AnesthesiaTime anesthesiaTime,
                                CleaningTime cleaningTime, SurgeryTime surgeryTime){
            this.Id = new OperationTypeId(Guid.NewGuid());
            this.Name = name;
            this.EstimatedDuration = estimatedDuration;
            this.AnesthesiaTime = anesthesiaTime;
            this.CleaningTime = cleaningTime;
            this.SurgeryTime = surgeryTime;
            this.OperationTypeStatus = OperationTypeStatus.ACTIVE;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (OperationType)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}