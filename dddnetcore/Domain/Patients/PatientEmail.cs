using System;
using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    public class PatientEmail : IValueObject{
        public String Email{get; private set;}
        public PatientEmail(String email){
            if (!ValidateEmail(email))
                throw new BusinessRuleValidationException("Email is not valid!");
            this.Email = email;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (PatientEmail)obj;
            return Email == other.Email;
        }

        public override int GetHashCode(){
            return Email.GetHashCode();
        }
        public void UpdateEmail(string email){
            if (!ValidateEmail(email))
                throw new BusinessRuleValidationException("Email is not valid!");
            this.Email = email;
        }
        public bool ValidateEmail(string email){
            if (string.IsNullOrEmpty(email))
                return false;
            if (!Regex.IsMatch(email,@"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return false;
            return true;
        }
    }
}