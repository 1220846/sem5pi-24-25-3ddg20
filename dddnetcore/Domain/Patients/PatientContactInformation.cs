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

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;
            
            return Email.Equals(((PatientContactInformation)obj).Email) && PhoneNumber.Equals(((PatientContactInformation)obj).PhoneNumber);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Email, PhoneNumber);
        }

        public void ChangeEmail(PatientEmail newPatientEmail){
            ArgumentNullException.ThrowIfNull(newPatientEmail);
            this.Email = newPatientEmail;
        }

        public void ChangePhoneNumber(PatientPhone newPatientPhone){
            ArgumentNullException.ThrowIfNull(newPatientPhone);
            this.PhoneNumber = newPatientPhone;
        }
    }
}