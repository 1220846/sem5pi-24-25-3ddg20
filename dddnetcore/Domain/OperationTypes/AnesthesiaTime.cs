using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationTypes{

    public class AnesthesiaTime : IValueObject {

        public int Minutes {get; private set;}

        public AnesthesiaTime(int minutes){

            if(minutes <= 0)
                throw new BusinessRuleValidationException("The anesthesia time must be positive");

            this.Minutes =minutes;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (AnesthesiaTime)obj;
            return Minutes == other.Minutes;
        }

        public override int GetHashCode(){
            return Minutes.GetHashCode();
        }
    }
}