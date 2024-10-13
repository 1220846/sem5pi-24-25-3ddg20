using System;
using System.Text.Json.Serialization;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationRequests{
    public class OperationRequestId : EntityId {

        public String Id {get; private set;}

        [JsonConstructor]
        public OperationRequestId(String value):base(value)
        {
            this.Id=value;
        }

        override
        protected  Object createFromString(String text){
            return new Guid(text);
        }

        override
        public String AsString(){
            Guid obj = (Guid) base.ObjValue;
            return obj.ToString();
        }
        
       
        public Guid AsGuid(){
            return (Guid) base.ObjValue;
        }
    }
}