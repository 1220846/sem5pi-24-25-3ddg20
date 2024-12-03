using System;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.Specializations
{
    public class SpecializationDescription : IValueObject
    {
        public String Description {get; private set;}

        public SpecializationDescription(String description) {
            this.Description = description;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Description.Equals(((SpecializationDescription)obj).Description);
        }

        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }
    }
}