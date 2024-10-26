using System;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Tests.Domain.Patients
{
    public class EmergencyContactTests
    {
        [Fact]
        public void Constructor_ShouldInitializePhoneNumber()
        {
            // Arrange
            string validPhoneNumber = "912345678";

            // Act
            var emergencyContact = new EmergencyContact(validPhoneNumber);

            // Assert
            Assert.Equal(validPhoneNumber, emergencyContact.PhoneNumber);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenPhoneNumberIsNullOrEmpty()
        {
            // Arrange
            string emptyPhoneNumber = "";

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new EmergencyContact(null));
            Assert.Throws<BusinessRuleValidationException>(() => new EmergencyContact(emptyPhoneNumber));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenPhoneNumberIsInvalid()
        {
            // Arrange
            string invalidPhoneNumber = "812345678"; // Does not start with a valid prefix (9[1236])

            // Act & Assert
            var exception = Assert.Throws<BusinessRuleValidationException>(() => new EmergencyContact(invalidPhoneNumber));
            Assert.Equal("Phone number format is not valid!", exception.Message);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSamePhoneNumber()
        {
            // Arrange
            string phoneNumber = "912345678";
            var contact1 = new EmergencyContact(phoneNumber);
            var contact2 = new EmergencyContact(phoneNumber);

            // Act
            bool areEqual = contact1.Equals(contact2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentPhoneNumbers()
        {
            // Arrange
            var contact1 = new EmergencyContact("912345678");
            var contact2 = new EmergencyContact("923456789");

            // Act
            bool areEqual = contact1.Equals(contact2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSamePhoneNumber()
        {
            // Arrange
            string phoneNumber = "912345678";
            var contact1 = new EmergencyContact(phoneNumber);
            var contact2 = new EmergencyContact(phoneNumber);

            // Act
            int hash1 = contact1.GetHashCode();
            int hash2 = contact2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentPhoneNumbers()
        {
            // Arrange
            var contact1 = new EmergencyContact("912345678");
            var contact2 = new EmergencyContact("923456789");

            // Act
            int hash1 = contact1.GetHashCode();
            int hash2 = contact2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
