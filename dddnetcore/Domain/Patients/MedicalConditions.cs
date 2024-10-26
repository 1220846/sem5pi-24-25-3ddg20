using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    public class MedicalConditions : IValueObject{
        public String Conditions{get; private set;}
        public MedicalConditions(String medicalConditions){
            this.Conditions = medicalConditions;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (MedicalConditions)obj;
            return Conditions == other.Conditions;
        }

        public override int GetHashCode(){
            return Conditions.GetHashCode();
        }
        public void UpdateMedicalConditions(string conditions){
            this.Conditions = conditions;
        }
    }
}