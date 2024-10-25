using System;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.AvailabilitySlots
{
    public class StartTime : IValueObject
    {
        public DateTime Time {get; private set;}

        public StartTime(DateTime time) {
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