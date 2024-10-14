using System;
using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Users
{
    public class Email : IValueObject
    {
        public string Address { get; private set; }

        public Email(string address)
        {
            if(!Email.isValidEmail(address)){
                throw new BusinessRuleValidationException("Invalid email format!");
            }
            this.Address=address;
        }

        private static bool isValidEmail(string email)
        {
            var regex= new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(email))
            {
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Address.Equals(((Email)obj).Address);
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }
    }
}
