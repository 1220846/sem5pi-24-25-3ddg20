using System.ComponentModel;

namespace DDDSample1.Domain.OperationRequests{
    public enum OperationRequestStatus
    {
        [Description ("Scheduled")] SCHEDULED,
        [Description ("OnWaitingList")] WAITING 

    }
}