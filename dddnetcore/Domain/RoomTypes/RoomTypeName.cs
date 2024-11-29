using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.RoomTypes{
    
    public class RoomTypeName{
        public String Name { get; private set; }

        public RoomTypeName(String name){

            if(string.IsNullOrEmpty(name))
                throw new BusinessRuleValidationException("The name of room type cannot be null or empty!");
            this.Name = name;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Name.Equals(((RoomTypeName)obj).Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
