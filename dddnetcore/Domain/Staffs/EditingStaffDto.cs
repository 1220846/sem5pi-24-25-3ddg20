using System;

namespace dddnetcore.Domain.Staffs
{
    public class EditingStaffDto
    {
        public string Email {get;set;}
        public string PhoneNumber {get;set;}
        public Guid? SpecializationId {get;set;}
        public long? NewAvailabilitySlotStartTime {get;set;}
        public long? NewAvailabilitySlotEndTime {get;set;}
        public Guid? ToRemoveAvailabilitySlotId {get;set;}
    }
}