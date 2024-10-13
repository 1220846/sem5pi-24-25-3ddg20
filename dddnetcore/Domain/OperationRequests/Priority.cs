using System.ComponentModel;

namespace DDDSample1.Domain.OperationRequests{
    public enum Priority
    {
        [Description ("Elective")] ELECTIVE,
        [Description ("Urgent")] URGENT,
        [Description ("Emergency")] EMERGENCY

    }
}