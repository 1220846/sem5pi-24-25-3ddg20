using System;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Tests.Domain.Patients
{
    public class DateOfBirthTests
    {
        [Fact]
        public void Constructor_ShouldInitializeDate()
        {
            // Arrange
            DateTime birthDate = new DateTime(2000, 1, 1);

            // Act
            var dateOfBirth = new DateOfBirth(birthDate);

            // Assert
            Assert.Equal(birthDate, dateOfBirth.Date);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenDateInFuture()
        {
            // Arrange
            DateTime futureDate = DateTime.Now.AddDays(1);

            // Act & Assert
            var exception = Assert.Throws<BusinessRuleValidationException>(() => new DateOfBirth(futureDate));
            Assert.Equal("The date of birth can't be in the future!", exception.Message);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameDate()
        {
            // Arrange
            DateTime birthDate = new DateTime(2000, 1, 1);
            var dateOfBirth1 = new DateOfBirth(birthDate);
            var dateOfBirth2 = new DateOfBirth(birthDate);

            // Act
            bool areEqual = dateOfBirth1.Equals(dateOfBirth2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentDates()
        {
            // Arrange
            var dateOfBirth1 = new DateOfBirth(new DateTime(2000, 1, 1));
            var dateOfBirth2 = new DateOfBirth(new DateTime(1990, 1, 1));

            // Act
            bool areEqual = dateOfBirth1.Equals(dateOfBirth2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSameDate()
        {
            // Arrange
            DateTime birthDate = new DateTime(2000, 1, 1);
            var dateOfBirth1 = new DateOfBirth(birthDate);
            var dateOfBirth2 = new DateOfBirth(birthDate);

            // Act
            int hash1 = dateOfBirth1.GetHashCode();
            int hash2 = dateOfBirth2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentDates()
        {
            // Arrange
            var dateOfBirth1 = new DateOfBirth(new DateTime(2000, 1, 1));
            var dateOfBirth2 = new DateOfBirth(new DateTime(1990, 1, 1));

            // Act
            int hash1 = dateOfBirth1.GetHashCode();
            int hash2 = dateOfBirth2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
