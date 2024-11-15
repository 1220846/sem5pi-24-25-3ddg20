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

            var anonymizedData = new AnonymizedPatientData(ageRange, gender, medicalConditions);

            Assert.NotNull(anonymizedData);
            Assert.Equal(ageRange, anonymizedData.AgeRange);
            Assert.Equal(gender, anonymizedData.Gender);
            Assert.Equal(medicalConditions, anonymizedData.MedicalConditions);
            Assert.True(anonymizedData.AnonymizedDate <= DateTime.Now);
            Assert.IsType<AnonymizedPatientDataId>(anonymizedData.Id);
        }

        [Fact]
        public void ShouldReturnTrueWhenComparingSameAnonymizedPatientDataById()
        {
            var ageRange = "18-35";
            var gender = "Male";
            var medicalConditions = "Diabetes";

            var anonymizedData1 = new AnonymizedPatientData(ageRange, gender, medicalConditions);

            var anonymizedData2 = new AnonymizedPatientData(ageRange, gender, medicalConditions);
            typeof(AnonymizedPatientData).GetProperty("Id").SetValue(anonymizedData2, anonymizedData1.Id);

            Assert.True(anonymizedData1.Equals(anonymizedData2));
            Assert.Equal(anonymizedData1.GetHashCode(), anonymizedData2.GetHashCode());
        }

        [Fact]
        public void ShouldReturnFalseWhenComparingDifferentAnonymizedPatientDataById()
        {
            var anonymizedData1 = new AnonymizedPatientData("36-50", "Female", "Hypertension");
            var anonymizedData2 = new AnonymizedPatientData("51-65", "Male", "Diabetes");

            Assert.False(anonymizedData1.Equals(anonymizedData2));
            Assert.NotEqual(anonymizedData1.GetHashCode(), anonymizedData2.GetHashCode());
        }
    }
}
