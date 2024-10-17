using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dddnetcore.Domain.AvailabilitySlots
{
    public class AvailabilitySlotDto
    {
        public Guid Id {get;set;}
        public DateTime StartTime {get;set;}
        public DateTime EndTime {get;set;}

        
        public AvailabilitySlotDto(AvailabilitySlot availabilitySlot) {
            this.Id = availabilitySlot.Id.AsGuid();
            this.StartTime = availabilitySlot.StartTime.Time;
            this.EndTime = availabilitySlot.EndTime.Time;
        }
    }
}