using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Specializations{
    
    public class SpecializationName{
        public String Name { get; private set; }

        public SpecializationName(String name){

            if(string.IsNullOrEmpty(name))
                throw new BusinessRuleValidationException("The name of specialization cannot be null or empty!");
            this.Name = name;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Name.Equals(((SpecializationName)obj).Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
