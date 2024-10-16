using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    public class PatientContactInformation : IValueObject{
        public PatientEmail Email{get; private set;}
        public PatientPhone PhoneNumber{get; private set;}
        public PatientContactInformation(PatientEmail email, PatientPhone phoneNumber){
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }
    }
}