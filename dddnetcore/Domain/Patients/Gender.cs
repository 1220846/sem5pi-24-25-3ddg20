using System.ComponentModel;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    public enum Gender {
        [Description ("Male")]
        MALE,
        [Description ("Female")]
        FEMALE,
        [Description ("Undefined")]
        UNDEFINED,
        [Description ("Other")]
        OTHER
    }
}