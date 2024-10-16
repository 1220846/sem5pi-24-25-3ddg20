using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    
    public class EmergencyContact : IValueObject {
        public string PhoneNumber {get;private set;}
        public EmergencyContact(string phoneNumber){
            if (string.IsNullOrEmpty(phoneNumber))
                throw new BusinessRuleValidationException("Phone number cannot be null or empty!");
            if (!(Regex.IsMatch(phoneNumber,@"^9[1236]\d{7}$")))
                throw new BusinessRuleValidationException("Phone number format is not valid!");
            this.PhoneNumber = phoneNumber;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (EmergencyContact)obj;
            return PhoneNumber == other.PhoneNumber;
        }

        public override int GetHashCode(){
            return PhoneNumber.GetHashCode();
        }
    }
}