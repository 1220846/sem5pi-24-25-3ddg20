using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.RoomTypes
{
    public class RoomTypeDescription : IValueObject
    {
        public String Description {get; private set;}

        public RoomTypeDescription(String description) {
            this.Description = description;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Description.Equals(((RoomTypeDescription)obj).Description);
        }

        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }
    }
}