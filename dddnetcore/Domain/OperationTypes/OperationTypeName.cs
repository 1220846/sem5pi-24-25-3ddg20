using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationTypes{
    
    public class OperationTypeName : IValueObject{
        public String Name { get; private set; }

        public OperationTypeName(String name){

            if(string.IsNullOrEmpty(name))
                throw new BusinessRuleValidationException("The name of operation type cannot be null or empty!");
            this.Name = name;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Name.Equals(((OperationTypeName)obj).Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
