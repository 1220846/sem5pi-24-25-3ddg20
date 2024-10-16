using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    public class Patient : Entity<MedicalRecordNumber>, IAggregateRoot{
        public AppointmentHistory AppointmentHistory{get; private set;}
        public DateOfBirth DateOfBirth{get; private set;}
        public EmergencyContact EmergencyContact{get; private set;}
        public Gender gender{get; private set;}
        public MedicalConditions MedicalConditions{get; private set;}
        public PatientContactInformation ContactInformation{get; private set;}
        public PatientFirstName FirstName{get; private set;}
        public PatientLastName LastName{get; private set;}
        public PatientFullName FullName{get; private set;}
        public Patient(AppointmentHistory appointmentHistory, DateOfBirth dateOfBirth, EmergencyContact emergencyContact,
            Gender gender, MedicalConditions medicalConditions, PatientContactInformation contactInformation, 
            PatientFirstName firstName, PatientLastName lastName, PatientFullName fullName){
            this.AppointmentHistory = appointmentHistory;
            this.DateOfBirth = dateOfBirth;
            this.EmergencyContact = emergencyContact;
            this.gender = gender;
            this.MedicalConditions = medicalConditions;
            this.ContactInformation = contactInformation;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = fullName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (Patient)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode(){
            return Id.GetHashCode();
        }

    }
}