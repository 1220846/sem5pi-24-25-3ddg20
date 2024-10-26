using System;
using Xunit;
using DDDSample1.Domain.Patients;

namespace DDDSample1.Tests.Domain.Patients
{
    public class MedicalConditionsTests
    {
        [Fact]
        public void Constructor_ShouldInitializeConditions()
        {
            // Arrange
            string initialConditions = "Diabetes, Hypertension";

            // Act
            var medicalConditions = new MedicalConditions(initialConditions);

            // Assert
            Assert.Equal(initialConditions, medicalConditions.Conditions);
        }

        [Fact]
        public void UpdateMedicalConditions_ShouldUpdateConditions()
        {
            // Arrange
            var medicalConditions = new MedicalConditions("Diabetes");
            string updatedConditions = "Diabetes, Hypertension";

            // Act
            medicalConditions.UpdateMedicalConditions(updatedConditions);

            // Assert
            Assert.Equal(updatedConditions, medicalConditions.Conditions);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameConditions()
        {
            // Arrange
            string conditions = "Diabetes";
            var medicalConditions1 = new MedicalConditions(conditions);
            var medicalConditions2 = new MedicalConditions(conditions);

            // Act
            bool areEqual = medicalConditions1.Equals(medicalConditions2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentConditions()
        {
            // Arrange
            var medicalConditions1 = new MedicalConditions("Diabetes");
            var medicalConditions2 = new MedicalConditions("Hypertension");

            // Act
            bool areEqual = medicalConditions1.Equals(medicalConditions2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSameConditions()
        {
            // Arrange
            string conditions = "Diabetes";
            var medicalConditions1 = new MedicalConditions(conditions);
            var medicalConditions2 = new MedicalConditions(conditions);

            // Act
            int hash1 = medicalConditions1.GetHashCode();
            int hash2 = medicalConditions2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentConditions()
        {
            // Arrange
            var medicalConditions1 = new MedicalConditions("Diabetes");
            var medicalConditions2 = new MedicalConditions("Hypertension");

            // Act
            int hash1 = medicalConditions1.GetHashCode();
            int hash2 = medicalConditions2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
