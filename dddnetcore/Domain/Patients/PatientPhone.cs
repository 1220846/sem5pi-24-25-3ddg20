using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    
    public class PatientPhone : IValueObject {
        public string PhoneNumber {get;private set;}
        public PatientPhone(string phoneNumber){
            if (!ValidatePhoneNumber(phoneNumber))
                throw new BusinessRuleValidationException("Phone number is not valid!");
            this.PhoneNumber = phoneNumber;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (PatientPhone)obj;
            return PhoneNumber == other.PhoneNumber;
        }

        public override int GetHashCode(){
            return PhoneNumber.GetHashCode();
        }
        public void UpdatePatientPhone(string phoneNumber){
            if (!ValidatePhoneNumber(phoneNumber))
                throw new BusinessRuleValidationException("Phone number is not valid!");
            this.PhoneNumber = phoneNumber;
        }
        public bool ValidatePhoneNumber(string phoneNumber){
            if (string.IsNullOrEmpty(phoneNumber))
                return false;
            if (!Regex.IsMatch(phoneNumber,@"^9[1236]\d{7}$"))
                return false;
            return true;
        }
    }
}