using System;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.AvailabilitySlots
{
    public class StartTime
    {
        public long Time {get; private set;}

        public StartTime(long time) {
            if (time < 0)
                throw new BusinessRuleValidationException("Timestamp cannot be negative!");
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