using System.ComponentModel;

namespace dddnetcore.Domain.SurgeryRoom
{
    public enum RoomType
    {
        [Description("Operating Room")] OPERATING_ROOM,
        [Description("Consultation Room")] CONSULTATION_ROOM,
        [Description("ICU")] ICU
    }
}