using System;
using System.Text.Json.Serialization;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRoom
{
    public class RoomNumber : EntityId
    {
        public string Id {get; private set;}

        [JsonConstructor]
        public RoomNumber(Guid value):base(value) {}

        [JsonConstructor]
        public RoomNumber(String value):base(value) {
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

        public Guid AsGuid() {
            return (Guid) base.ObjValue;
        }
    }
}