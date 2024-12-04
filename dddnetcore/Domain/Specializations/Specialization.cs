using System;
using System.Collections.Generic;
using dddnetcore.Domain.Specializations;
using DDDSample1.Domain.OperationTypesSpecializations;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Specializations{

    public class Specialization : Entity<SpecializationId>, IAggregateRoot
    {
        public SpecializationName Name { get; private set; }
        public SpecializationCode Code { get; private set; }
        public SpecializationDescription Description { get; private set; }

        public ICollection<OperationTypeSpecialization> OperationTypeSpecializations {get; private set;} = new List<OperationTypeSpecialization>();

        public Specialization(SpecializationName name, SpecializationCode code, SpecializationDescription description){
            this.Id = new SpecializationId(Guid.NewGuid());
            this.Name = name;
            this.Code = code;
            this.Description = description;
        }

        public void ChangeName(SpecializationName newName) {
            ArgumentNullException.ThrowIfNull(newName);
            this.Name = newName;
        }

        public void ChangeDescription(SpecializationDescription newDescription) {
            this.Description = newDescription; // null is allowed
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