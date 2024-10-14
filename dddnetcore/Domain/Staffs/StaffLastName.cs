using System;
using System.ComponentModel.DataAnnotations;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Staffs {

    public class StaffLastName : IValueObject {
    
        public String Name {get; private set;}

        public StaffLastName(String name) {
            if (string.IsNullOrEmpty(name))
                throw new BusinessRuleValidationException("Last name of staff cannot be null or empty!");
            this.Name = name;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Name.Equals(((StaffLastName)obj).Name);
        }

        
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }   
}