using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.OperationTypeSpecializations;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;

namespace DDDSample1.Domain.OperationTypesSpecializations{

    public class OperationTypeSpecialization : Entity<OperationTypeSpecializationId>, IAggregateRoot
    {
        public NumberOfStaff NumberOfStaff { get;  private set; }
        private OperationTypeSpecialization() { }

        public OperationTypeSpecialization(OperationTypeId operationTypeId, SpecializationId specializationId,NumberOfStaff numberOfStaff){
            this.Id = new OperationTypeSpecializationId(operationTypeId,specializationId);
            this.NumberOfStaff = numberOfStaff;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (OperationTypeSpecialization)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}