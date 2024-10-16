using System;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.AvailabilitySlots
{
    public class StartTime
    {
        public DateTime Time {get; private set;}

        public StartTime(DateTime time) {
            if (time <= DateTime.Now.AddMinutes(-5)) //para deixar criar ligeiramente no passado
                throw new BusinessRuleValidationException("Availability cannot start in the past.");
            this.Time = time;
        } 

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Time.Equals(((StartTime)obj).Time);
        }

        public override int GetHashCode()
        {
            return Time.GetHashCode();
        }
    }
}