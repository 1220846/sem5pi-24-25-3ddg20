using System;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Tests.Domain.Patients
{
    public class PatientPhoneTests
    {
        [Fact]
        public void Constructor_ShouldInitializePhoneNumber()
        {
            // Arrange
            string validPhoneNumber = "912345678";

            // Act
            var patientPhone = new PatientPhone(validPhoneNumber);

            // Assert
            Assert.Equal(validPhoneNumber, patientPhone.PhoneNumber);
        }

        [Fact]
        public void Constructor_ShouldThrowException_ForInvalidPhoneNumber()
        {
            // Arrange
            string invalidPhoneNumber = "12345678";

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new PatientPhone(invalidPhoneNumber));
        }

        [Fact]
        public void Constructor_ShouldThrowException_ForEmptyPhoneNumber()
        {
            // Arrange
            string emptyPhoneNumber = "";

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new PatientPhone(emptyPhoneNumber));
        }

        [Fact]
        public void UpdatePatientPhone_ShouldUpdateValidPhoneNumber()
        {
            // Arrange
            var patientPhone = new PatientPhone("912345678");
            string newPhoneNumber = "923456789";

            // Act
            patientPhone.UpdatePatientPhone(newPhoneNumber);

            // Assert
            Assert.Equal(newPhoneNumber, patientPhone.PhoneNumber);
        }

        [Fact]
        public void UpdatePatientPhone_ShouldThrowException_ForInvalidPhoneNumber()
        {
            // Arrange
            var patientPhone = new PatientPhone("912345678");
            string invalidPhoneNumber = "12345678";

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => patientPhone.UpdatePatientPhone(invalidPhoneNumber));
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSamePhoneNumber()
        {
            // Arrange
            string phoneNumber = "912345678";
            var phone1 = new PatientPhone(phoneNumber);
            var phone2 = new PatientPhone(phoneNumber);

            // Act
            bool areEqual = phone1.Equals(phone2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentPhoneNumbers()
        {
            // Arrange
            var phone1 = new PatientPhone("912345678");
            var phone2 = new PatientPhone("923456789");

            // Act
            bool areEqual = phone1.Equals(phone2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSamePhoneNumber()
        {
            // Arrange
            string phoneNumber = "912345678";
            var phone1 = new PatientPhone(phoneNumber);
            var phone2 = new PatientPhone(phoneNumber);

            // Act
            int hash1 = phone1.GetHashCode();
            int hash2 = phone2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentPhoneNumbers()
        {
            // Arrange
            var phone1 = new PatientPhone("912345678");
            var phone2 = new PatientPhone("923456789");

            // Act
            int hash1 = phone1.GetHashCode();
            int hash2 = phone2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
