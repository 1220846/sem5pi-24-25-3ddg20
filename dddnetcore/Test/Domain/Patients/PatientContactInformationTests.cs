using System;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Tests.Domain.Patients
{
    public class PatientContactInformationTests
    {
        [Fact]
        public void Constructor_ShouldInitializeEmailAndPhoneNumber()
        {
            // Arrange
            var email = new PatientEmail("test@example.com");
            var phoneNumber = new PatientPhone("912345678");

            // Act
            var contactInfo = new PatientContactInformation(email, phoneNumber);

            // Assert
            Assert.Equal(email, contactInfo.Email);
            Assert.Equal(phoneNumber, contactInfo.PhoneNumber);
        }

        [Fact]
        public void ChangeEmail_ShouldUpdateEmail()
        {
            // Arrange
            var contactInfo = new PatientContactInformation(
                new PatientEmail("old@example.com"), 
                new PatientPhone("912345678")
            );
            var newEmail = new PatientEmail("new@example.com");

            // Act
            contactInfo.ChangeEmail(newEmail);

            // Assert
            Assert.Equal(newEmail, contactInfo.Email);
        }

        [Fact]
        public void ChangeEmail_ShouldThrowException_WhenEmailIsNull()
        {
            // Arrange
            var contactInfo = new PatientContactInformation(
                new PatientEmail("test@example.com"), 
                new PatientPhone("912345678")
            );

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => contactInfo.ChangeEmail(null));
        }

        [Fact]
        public void ChangePhoneNumber_ShouldUpdatePhoneNumber()
        {
            // Arrange
            var contactInfo = new PatientContactInformation(
                new PatientEmail("test@example.com"), 
                new PatientPhone("912345678")
            );
            var newPhoneNumber = new PatientPhone("923456789");

            // Act
            contactInfo.ChangePhoneNumber(newPhoneNumber);

            // Assert
            Assert.Equal(newPhoneNumber, contactInfo.PhoneNumber);
        }

        [Fact]
        public void ChangePhoneNumber_ShouldThrowException_WhenPhoneNumberIsNull()
        {
            // Arrange
            var contactInfo = new PatientContactInformation(
                new PatientEmail("test@example.com"), 
                new PatientPhone("912345678")
            );

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => contactInfo.ChangePhoneNumber(null));
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameEmailAndPhoneNumber()
        {
            // Arrange
            var email = new PatientEmail("test@example.com");
            var phoneNumber = new PatientPhone("912345678");
            var contactInfo1 = new PatientContactInformation(email, phoneNumber);
            var contactInfo2 = new PatientContactInformation(email, phoneNumber);

            // Act
            bool areEqual = contactInfo1.Equals(contactInfo2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentEmailOrPhoneNumber()
        {
            // Arrange
            var contactInfo1 = new PatientContactInformation(
                new PatientEmail("test@example.com"), 
                new PatientPhone("912345678")
            );
            var contactInfo2 = new PatientContactInformation(
                new PatientEmail("other@example.com"), 
                new PatientPhone("912345678")
            );
            var contactInfo3 = new PatientContactInformation(
                new PatientEmail("test@example.com"), 
                new PatientPhone("923456789")
            );

            // Act & Assert
            Assert.False(contactInfo1.Equals(contactInfo2));
            Assert.False(contactInfo1.Equals(contactInfo3));
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSameEmailAndPhoneNumber()
        {
            // Arrange
            var email = new PatientEmail("test@example.com");
            var phoneNumber = new PatientPhone("912345678");
            var contactInfo1 = new PatientContactInformation(email, phoneNumber);
            var contactInfo2 = new PatientContactInformation(email, phoneNumber);

            // Act
            int hash1 = contactInfo1.GetHashCode();
            int hash2 = contactInfo2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentEmailOrPhoneNumber()
        {
            // Arrange
            var contactInfo1 = new PatientContactInformation(
                new PatientEmail("test@example.com"), 
                new PatientPhone("912345678")
            );
            var contactInfo2 = new PatientContactInformation(
                new PatientEmail("other@example.com"), 
                new PatientPhone("912345678")
            );
            var contactInfo3 = new PatientContactInformation(
                new PatientEmail("test@example.com"), 
                new PatientPhone("923456789")
            );

            // Act
            int hash1 = contactInfo1.GetHashCode();
            int hash2 = contactInfo2.GetHashCode();
            int hash3 = contactInfo3.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
            Assert.NotEqual(hash1, hash3);
        }
    }
}
