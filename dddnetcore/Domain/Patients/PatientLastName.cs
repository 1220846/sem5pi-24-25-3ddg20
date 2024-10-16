using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    public class PatientLastName : IValueObject{
        public String Name{get; private set;}
        public PatientLastName(String name){
            if(string.IsNullOrEmpty(name))
                throw new BusinessRuleValidationException("The name cannot be null or empty!");
            this.Name = name;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (PatientLastName)obj;
            return Name == other.Name;
        }

        public override int GetHashCode(){
            return Name.GetHashCode();
        }
        public void UpdateLastName(String name){
            this.Name = name;
        }
    }
}