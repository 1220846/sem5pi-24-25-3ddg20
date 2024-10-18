using System;
using DDDSample1.Domain.Shared;
using Newtonsoft.Json;

namespace DDDSample1.Domain.Patients{
    public class MedicalRecordNumber : EntityId{
        public string Id{get; private set;}

        [JsonConstructor]
        
        public MedicalRecordNumber(string id):base(id){
            this.Id = id;
        }
        
        protected override object createFromString(string text)
        {
            return text; 
        }

        public override string AsString()
        {
            return Id;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Id.Equals(((MedicalRecordNumber)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    
    }
}