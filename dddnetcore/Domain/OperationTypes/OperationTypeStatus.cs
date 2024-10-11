using System.ComponentModel;

namespace DDDSample1.Domain.OperationTypes{

    public enum OperationTypeStatus
    {
        [Description("Active")]
        ACTIVE,

        [Description("Inactive")]
        INACTIVE
    }
}