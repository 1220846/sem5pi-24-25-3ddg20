using System;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Tests.Domain.Patients
{
    public class PatientEmailTests
    {
        [Fact]
        public void Constructor_ShouldInitializeEmail()
        {
            // Arrange
            string validEmail = "test@example.com";

            // Act
            var patientEmail = new PatientEmail(validEmail);

            // Assert
            Assert.Equal(validEmail, patientEmail.Email);
        }

        [Fact]
        public void Constructor_ShouldThrowException_ForInvalidEmail()
        {
            // Arrange
            string invalidEmail = "invalid-email";

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new PatientEmail(invalidEmail));
        }

        [Fact]
        public void Constructor_ShouldThrowException_ForEmptyEmail()
        {
            // Arrange
            string emptyEmail = "";

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new PatientEmail(emptyEmail));
        }

        [Fact]
        public void UpdateEmail_ShouldUpdateValidEmail()
        {
            // Arrange
            var patientEmail = new PatientEmail("test@example.com");
            string newEmail = "new@example.com";

            // Act
            patientEmail.UpdateEmail(newEmail);

            // Assert
            Assert.Equal(newEmail, patientEmail.Email);
        }

        [Fact]
        public void UpdateEmail_ShouldThrowException_ForInvalidEmail()
        {
            // Arrange
            var patientEmail = new PatientEmail("test@example.com");
            string invalidEmail = "invalid-email";

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => patientEmail.UpdateEmail(invalidEmail));
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameEmail()
        {
            // Arrange
            string email = "test@example.com";
            var email1 = new PatientEmail(email);
            var email2 = new PatientEmail(email);

            // Act
            bool areEqual = email1.Equals(email2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentEmails()
        {
            // Arrange
            var email1 = new PatientEmail("test@example.com");
            var email2 = new PatientEmail("other@example.com");

            // Act
            bool areEqual = email1.Equals(email2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSameEmail()
        {
            // Arrange
            string email = "test@example.com";
            var email1 = new PatientEmail(email);
            var email2 = new PatientEmail(email);

            // Act
            int hash1 = email1.GetHashCode();
            int hash2 = email2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentEmails()
        {
            // Arrange
            var email1 = new PatientEmail("test@example.com");
            var email2 = new PatientEmail("other@example.com");

            // Act
            int hash1 = email1.GetHashCode();
            int hash2 = email2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
