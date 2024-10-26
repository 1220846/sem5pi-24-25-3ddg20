using System;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Tests.Domain.Patients
{
    public class PatientLastNameTests
    {
        [Fact]
        public void Constructor_ShouldInitializeName()
        {
            // Arrange
            string validName = "Doe";

            // Act
            var lastName = new PatientLastName(validName);

            // Assert
            Assert.Equal(validName, lastName.Name);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameIsNull()
        {
            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new PatientLastName(null));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameIsEmpty()
        {
            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new PatientLastName(string.Empty));
        }

        [Fact]
        public void UpdateLastName_ShouldUpdateName()
        {
            // Arrange
            var lastName = new PatientLastName("Doe");
            string newName = "Smith";

            // Act
            lastName.UpdateLastName(newName);

            // Assert
            Assert.Equal(newName, lastName.Name);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameName()
        {
            // Arrange
            var lastName1 = new PatientLastName("Doe");
            var lastName2 = new PatientLastName("Doe");

            // Act
            bool areEqual = lastName1.Equals(lastName2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentNames()
        {
            // Arrange
            var lastName1 = new PatientLastName("Doe");
            var lastName2 = new PatientLastName("Smith");

            // Act
            bool areEqual = lastName1.Equals(lastName2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSameName()
        {
            // Arrange
            var lastName1 = new PatientLastName("Doe");
            var lastName2 = new PatientLastName("Doe");

            // Act
            int hash1 = lastName1.GetHashCode();
            int hash2 = lastName2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentNames()
        {
            // Arrange
            var lastName1 = new PatientLastName("Doe");
            var lastName2 = new PatientLastName("Smith");

            // Act
            int hash1 = lastName1.GetHashCode();
            int hash2 = lastName2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
