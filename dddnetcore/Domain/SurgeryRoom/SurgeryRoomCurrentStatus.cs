using System.ComponentModel;

namespace dddnetcore.Domain.SurgeryRoom
{
    public enum SurgeryRoomCurrentStatus
    {
        [Description("Available")] AVAILABLE,
        [Description("Occupied")] OCCUPIED,
        [Description("Under Maintenance")] UNDER_MAINTENANCE
    }
}