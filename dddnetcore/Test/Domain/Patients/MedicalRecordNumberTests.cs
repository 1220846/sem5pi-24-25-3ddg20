using System;
using Xunit;
using DDDSample1.Domain.Patients;

namespace DDDSample1.Tests.Domain.Patients
{
    public class MedicalRecordNumberTests
    {
        [Fact]
        public void Constructor_ShouldInitializeId()
        {
            // Arrange
            string validId = "202410000001";

            // Act
            var medicalRecordNumber = new MedicalRecordNumber(validId);

            // Assert
            Assert.Equal(validId, medicalRecordNumber.Id);
        }

        [Fact]
        public void AsString_ShouldReturnIdAsString()
        {
            // Arrange
            string validId = "202410000001";
            var medicalRecordNumber = new MedicalRecordNumber(validId);

            // Act
            string idAsString = medicalRecordNumber.AsString();

            // Assert
            Assert.Equal(validId, idAsString);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameId()
        {
            // Arrange
            string id = "202410000001";
            var medicalRecordNumber1 = new MedicalRecordNumber(id);
            var medicalRecordNumber2 = new MedicalRecordNumber(id);

            // Act
            bool areEqual = medicalRecordNumber1.Equals(medicalRecordNumber2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentIds()
        {
            // Arrange
            var medicalRecordNumber1 = new MedicalRecordNumber("202410000001");
            var medicalRecordNumber2 = new MedicalRecordNumber("202410000002");

            // Act
            bool areEqual = medicalRecordNumber1.Equals(medicalRecordNumber2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSameId()
        {
            // Arrange
            string id = "202410000001";
            var medicalRecordNumber1 = new MedicalRecordNumber(id);
            var medicalRecordNumber2 = new MedicalRecordNumber(id);

            // Act
            int hash1 = medicalRecordNumber1.GetHashCode();
            int hash2 = medicalRecordNumber2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentIds()
        {
            // Arrange
            var medicalRecordNumber1 = new MedicalRecordNumber("202410000001");
            var medicalRecordNumber2 = new MedicalRecordNumber("202410000002");

            // Act
            int hash1 = medicalRecordNumber1.GetHashCode();
            int hash2 = medicalRecordNumber2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
