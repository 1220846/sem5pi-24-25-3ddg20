using System;
using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;
using Newtonsoft.Json;

namespace DDDSample1.Domain.Staffs {

    public class StaffId : EntityId {

        public string Id {get; private set;}

        [JsonConstructor]
        public StaffId(Guid value):base(value)
        {
        }

        [JsonConstructor]
        public StaffId(String value):base(value)
        {
            if (!Regex.IsMatch(value, @"^[ODN]\d{9}$")) {
                throw new BusinessRuleValidationException("Staff ID is badly formatted!");
            }
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