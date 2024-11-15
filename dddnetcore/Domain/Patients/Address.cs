using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    public class Address : IValueObject{
        public String Location{get; private set;}
        public Address(String location){
            if(string.IsNullOrEmpty(location))
                throw new BusinessRuleValidationException("The address cannot be null or empty!");
            this.Location = location;
        }
    }
}