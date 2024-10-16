using System.ComponentModel;

namespace DDDSample1.Domain.OperationRequests{
    public enum Status
    {
        [Description ("Scheduled")] SCHEDULED,
        [Description ("OnWaitingList")] ONWAITINGLIST 

    }
}