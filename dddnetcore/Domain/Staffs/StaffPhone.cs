using System;
using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Staffs {

    public class StaffPhone : IValueObject {
    
        public String PhoneNumber {get; private set;}

        public StaffPhone(String phone) {
            if (string.IsNullOrEmpty(phone))
                throw new BusinessRuleValidationException("Phone number of staff cannot be null or empty!");
            if (!(Regex.IsMatch(phone,@"^9[1236]\d{7}$")))
                throw new BusinessRuleValidationException("Phone number format is not valid!");
            this.PhoneNumber = phone;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return PhoneNumber.Equals(((StaffFirstName)obj).Name);
        }

        
        public override int GetHashCode()
        {
            return PhoneNumber.GetHashCode();
        }
    }   
}