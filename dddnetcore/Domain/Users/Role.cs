using System.ComponentModel;

namespace DDDSample1.Domain.Users{
    public enum Role
    {
        [Description ("Nurse")]
        NURSE,
        [Description ("Doctor")]
        DOCTOR,
        [Description ("Technician")]
        TECHNICIAN,
        [Description ("Patient")]
        PATIENT,
        [Description ("Admin")]
        ADMIN

    }
}