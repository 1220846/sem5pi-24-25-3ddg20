using System;
using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Staffs {

    public class StaffEmail : IValueObject {
    
        public String Email {get; private set;}

        public StaffEmail(String email) {
            if (string.IsNullOrEmpty(email))
                throw new BusinessRuleValidationException("Email of staff cannot be null or empty!");
            if (!(Regex.IsMatch(email,@"^[^@\s]+@[^@\s]+\.[^@\s]+$")))
                throw new BusinessRuleValidationException("Email format is not valid!");
            this.Email = email;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Email.Equals(((StaffFirstName)obj).Name);
        }

        
        public override int GetHashCode()
        {
            return Email.GetHashCode();
        }
    }   
}