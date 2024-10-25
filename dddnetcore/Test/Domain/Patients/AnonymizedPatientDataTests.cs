using System;
using Xunit;
using DDDSample1.Domain.Patients;

namespace DDDSample1.Tests.Domain.Patients
{
    public class AnonymizedPatientDataTests
    {
        [Fact]
        public void ShouldCreateAnonymizedPatientDataWithValidParameters(){

            var ageRange = "36-50";
            var gender = "Female";
            var medicalConditions = "Asthma";
            var appointmentHistory = "2 appointments";

            var anonymizedData = new AnonymizedPatientData(ageRange, gender, medicalConditions, appointmentHistory);

            Assert.NotNull(anonymizedData);
            Assert.Equal(ageRange, anonymizedData.AgeRange);
            Assert.Equal(gender, anonymizedData.Gender);
            Assert.Equal(medicalConditions, anonymizedData.MedicalConditions);
            Assert.Equal(appointmentHistory, anonymizedData.AppointmentHistory);
            Assert.True(anonymizedData.AnonymizedDate <= DateTime.Now);
            Assert.IsType<AnonymizedPatientDataId>(anonymizedData.Id);
        }

        [Fact]
        public void ShouldReturnTrueWhenComparingSameAnonymizedPatientDataById()
        {
            var ageRange = "18-35";
            var gender = "Male";
            var medicalConditions = "Diabetes";
            var appointmentHistory = "3 appointments";

            var anonymizedData1 = new AnonymizedPatientData(ageRange, gender, medicalConditions, appointmentHistory);

            var anonymizedData2 = new AnonymizedPatientData(ageRange, gender, medicalConditions, appointmentHistory);
            typeof(AnonymizedPatientData).GetProperty("Id").SetValue(anonymizedData2, anonymizedData1.Id);

            Assert.True(anonymizedData1.Equals(anonymizedData2));
            Assert.Equal(anonymizedData1.GetHashCode(), anonymizedData2.GetHashCode());
        }

        [Fact]
        public void ShouldReturnFalseWhenComparingDifferentAnonymizedPatientDataById()
        {
            var anonymizedData1 = new AnonymizedPatientData("36-50", "Female", "Hypertension", "2 appointments");
            var anonymizedData2 = new AnonymizedPatientData("51-65", "Male", "Diabetes", "4 appointments");

            Assert.False(anonymizedData1.Equals(anonymizedData2));
            Assert.NotEqual(anonymizedData1.GetHashCode(), anonymizedData2.GetHashCode());
        }
    }
}
