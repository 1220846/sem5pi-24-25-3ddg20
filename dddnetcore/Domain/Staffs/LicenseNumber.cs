using System;
using System.ComponentModel.DataAnnotations;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Staffs {

    public class LicenseNumber : IValueObject {
    
        public String Number {get; private set;}

        public LicenseNumber(String number) {
            if (string.IsNullOrEmpty(number))
                throw new BusinessRuleValidationException("License number of staff cannot be null or empty!");
            this.Number = number;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Number.Equals(((LicenseNumber)obj).Number);
        }
        
        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }
    }   
}