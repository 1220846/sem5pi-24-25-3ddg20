using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationTypeSpecializations{
    
    public class NumberOfStaff : IValueObject {
        public int Number {get;private set;}
        public NumberOfStaff(int number){

            if(number  <= 0)
                throw new BusinessRuleValidationException("The number of staff must be positive");

            this.Number =number;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (NumberOfStaff)obj;
            return Number == other.Number;
        }

        public override int GetHashCode(){
            return Number.GetHashCode();
        }
    }
}
