using System.ComponentModel;

namespace dddnetcore.Domain.Staffs
{
    public enum StaffStatus
    {
        [Description("Active")]
        ACTIVE,

        [Description("Deactivated")]
        DEACTIVATED
    }
}