using System;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Tests.Domain.Patients
{
    public class PatientFirstNameTests
    {
        [Fact]
        public void Constructor_ShouldInitializeName()
        {
            // Arrange
            string validName = "John";

            // Act
            var firstName = new PatientFirstName(validName);

            // Assert
            Assert.Equal(validName, firstName.Name);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameIsNull()
        {
            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new PatientFirstName(null));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameIsEmpty()
        {
            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new PatientFirstName(string.Empty));
        }

        [Fact]
        public void UpdateFirstName_ShouldUpdateName()
        {
            // Arrange
            var firstName = new PatientFirstName("John");
            string newName = "James";

            // Act
            firstName.UpdateFirstName(newName);

            // Assert
            Assert.Equal(newName, firstName.Name);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameName()
        {
            // Arrange
            var name1 = new PatientFirstName("John");
            var name2 = new PatientFirstName("John");

            // Act
            bool areEqual = name1.Equals(name2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentNames()
        {
            // Arrange
            var name1 = new PatientFirstName("John");
            var name2 = new PatientFirstName("James");

            // Act
            bool areEqual = name1.Equals(name2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSameName()
        {
            // Arrange
            var name1 = new PatientFirstName("John");
            var name2 = new PatientFirstName("John");

            // Act
            int hash1 = name1.GetHashCode();
            int hash2 = name2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentNames()
        {
            // Arrange
            var name1 = new PatientFirstName("John");
            var name2 = new PatientFirstName("James");

            // Act
            int hash1 = name1.GetHashCode();
            int hash2 = name2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
