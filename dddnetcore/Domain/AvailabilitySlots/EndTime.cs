using System;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.AvailabilitySlots
{
    public class EndTime
    {
        public DateTime Time {get; private set;}

        public EndTime(DateTime time) {
            if (time <= DateTime.Now.AddMinutes(-5)) //para deixar criar ligeiramente no passado
                throw new BusinessRuleValidationException("Availability cannot end in the past.");
            this.Time = time;
        } 

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Time.Equals(((EndTime)obj).Time);
        }

        
        public override int GetHashCode()
        {
            return Time.GetHashCode();
        }
    }
}