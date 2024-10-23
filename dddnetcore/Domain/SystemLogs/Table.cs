using System.ComponentModel;

namespace DDDSample1.Domain.SystemLogs{

    public enum Entity
    {
        [Description("Staff")]
        STAFF,

        [Description("Patient")]
        PATIENT,

        [Description("OperationType")]
        OPERATION_TYPE,

        [Description("User")]
        USER,

        [Description("OperationRequest")]
        OPERATION_REQUEST,

        [Description("AvailabilitySlot")]
        AVAILABILITY_SLOT
    }
}