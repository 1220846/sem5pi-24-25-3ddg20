using System.ComponentModel;

namespace dddnetcore.Domain.SurgeryRooms
{
    public enum RoomType
    {
        [Description("Operating Room")] OPERATING_ROOM,
        [Description("Consultation Room")] CONSULTATION_ROOM,
        [Description("ICU")] ICU
    }
}