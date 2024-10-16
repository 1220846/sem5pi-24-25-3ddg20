using System;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.AvailabilitySlots
{
    public class AvailabilitySlot : Entity<AvailabilitySlotId>, IAggregateRoot
    {
        public StartTime StartTime {get; private set;}
        public EndTime EndTime {get; private set;}
    
        private AvailabilitySlot() {}

        public AvailabilitySlot(StartTime startTime, EndTime endTime) {
            this.Id = new AvailabilitySlotId(Guid.NewGuid());
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (AvailabilitySlot)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}