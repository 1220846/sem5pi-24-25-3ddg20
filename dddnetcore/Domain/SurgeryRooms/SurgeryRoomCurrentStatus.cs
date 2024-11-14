using System.ComponentModel;

namespace dddnetcore.Domain.SurgeryRooms
{
    public enum SurgeryRoomCurrentStatus
    {
        [Description("Available")] AVAILABLE,
        [Description("Occupied")] OCCUPIED,
        [Description("Under Maintenance")] UNDER_MAINTENANCE
    }
}