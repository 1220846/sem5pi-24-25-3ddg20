using System.ComponentModel;

namespace DDDSample1.Domain.Appointments{
    public enum AppointmentStatus
    {
        [Description ("Scheduled")] SCHEDULED,
        [Description ("Canceled")] CANCELED,
        [Description ("Completed")] COMPLETED
    }
}