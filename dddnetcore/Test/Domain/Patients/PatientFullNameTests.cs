using System;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Tests.Domain.Patients
{
    public class PatientFullNameTests
    {
        [Fact]
        public void Constructor_ShouldInitializeName()
        {
            // Arrange
            string validName = "John Doe";

            // Act
            var fullName = new PatientFullName(validName);

            // Assert
            Assert.Equal(validName, fullName.Name);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameIsNull()
        {
            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new PatientFullName(null));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameIsEmpty()
        {
            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new PatientFullName(string.Empty));
        }

        [Fact]
        public void UpdateFullName_ShouldUpdateName()
        {
            // Arrange
            var fullName = new PatientFullName("John Doe");
            string newName = "Jane Doe";

            // Act
            fullName.UpdateFullName(newName);

            // Assert
            Assert.Equal(newName, fullName.Name);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameName()
        {
            // Arrange
            var fullName1 = new PatientFullName("John Doe");
            var fullName2 = new PatientFullName("John Doe");

            // Act
            bool areEqual = fullName1.Equals(fullName2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentNames()
        {
            // Arrange
            var fullName1 = new PatientFullName("John Doe");
            var fullName2 = new PatientFullName("Jane Doe");

            // Act
            bool areEqual = fullName1.Equals(fullName2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSameName()
        {
            // Arrange
            var fullName1 = new PatientFullName("John Doe");
            var fullName2 = new PatientFullName("John Doe");

            // Act
            int hash1 = fullName1.GetHashCode();
            int hash2 = fullName2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentNames()
        {
            // Arrange
            var fullName1 = new PatientFullName("John Doe");
            var fullName2 = new PatientFullName("Jane Doe");

            // Act
            int hash1 = fullName1.GetHashCode();
            int hash2 = fullName2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
