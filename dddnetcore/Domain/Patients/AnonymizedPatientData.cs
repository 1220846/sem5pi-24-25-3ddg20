using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    public class AnonymizedPatientData : Entity<AnonymizedPatientDataId>{
        public string AgeRange { get; set; }
        public string Gender { get; set; }
        public string MedicalConditions { get; set; }
        public string AppointmentHistory {get; set; }
        public DateTime AnonymizedDate { get; set; }

        protected AnonymizedPatientData()
        {
        }

        public AnonymizedPatientData(string ageRange,string gender,string medicalConditions, string appointmentHistory){
            
            this.Id = new AnonymizedPatientDataId(Guid.NewGuid());
            this.AgeRange = ageRange;
            this.Gender = gender;
            this.MedicalConditions = medicalConditions;
            this.AppointmentHistory = appointmentHistory;
            this.AnonymizedDate = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (AnonymizedPatientData)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

}