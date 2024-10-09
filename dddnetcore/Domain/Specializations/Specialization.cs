using System;
using System.Collections.Generic;
using DDDSample1.Domain.OperationTypesSpecializations;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Specializations{

    public class Specialization : Entity<SpecializationId>, IAggregateRoot
    {
        public SpecializationName Name { get;  private set; }

        public ICollection<OperationTypeSpecialization> OperationTypeSpecializations {get; private set;} = new List<OperationTypeSpecialization>();

        public Specialization(SpecializationName name){
            this.Id = new SpecializationId(Guid.NewGuid());
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (Specialization)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}