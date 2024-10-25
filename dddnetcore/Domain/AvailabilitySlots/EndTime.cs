using System;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.AvailabilitySlots
{
    public class EndTime : IValueObject
    {
        public DateTime Time {get; private set;}

        public EndTime(DateTime time) {
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