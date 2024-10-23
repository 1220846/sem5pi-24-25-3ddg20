using System.ComponentModel;

namespace DDDSample1.Domain.SystemLogs{

    public enum Operation
    {
        [Description("Insert")]
        INSERT,

        [Description("Delete")]
        DELETE,

        [Description("Update")]
        UPDATE
    }
}