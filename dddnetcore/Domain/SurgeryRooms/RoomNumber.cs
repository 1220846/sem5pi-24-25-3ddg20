using System;
using System.Text.Json.Serialization;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRooms
{
    public class RoomNumber : EntityId
    {
        public string Id {get; private set;}

        [JsonConstructor]
        public RoomNumber(String value):base(value) {
            if (String.IsNullOrEmpty(value))
                throw new BusinessRuleValidationException("Room number cannot be null or empty!");
            this.Id = value;
        }

        
        override
        protected Object createFromString(String text){
            return text;
        }

        override
        public String AsString() {
            Guid obj = (Guid) base.ObjValue;
            return obj.ToString();
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Id.Equals(((RoomNumber)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}