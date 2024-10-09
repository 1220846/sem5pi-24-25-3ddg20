using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationTypes{
    
    public class EstimatedDuration : IValueObject {
        public int Minutes {get;private set;}
        public EstimatedDuration(int minutes){

            if(minutes <= 0)
                throw new BusinessRuleValidationException("The estimated duration must be positive");

            this.Minutes =minutes;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (EstimatedDuration)obj;
            return Minutes == other.Minutes;
        }

        public override int GetHashCode(){
            return Minutes.GetHashCode();
        }
    }
}
