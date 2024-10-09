using System;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationTypes
{
    public class AnesthesiaTimeTests
    {
        [Fact]
        public void ConstructorShouldCreateInstanceWithValidMinutes()
        {
            int validMinutes = 30;

            var anesthesiaTime = new AnesthesiaTime(validMinutes);

            Assert.Equal(validMinutes, anesthesiaTime.Minutes);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenMinutesAreNegative()
        {
            int invalidMinutes = -5;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new AnesthesiaTime(invalidMinutes));
            Assert.Equal("The anesthesia time must be positive", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenMinutesAreZero()
        {
            int invalidMinutes = 0;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new AnesthesiaTime(invalidMinutes));
            Assert.Equal("The anesthesia time must be positive", exception.Message);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenMinutesAreEqual()
        {
            var anesthesiaTime1 = new AnesthesiaTime(45);
            var anesthesiaTime2 = new AnesthesiaTime(45);

            bool result = anesthesiaTime1.Equals(anesthesiaTime2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenMinutesAreDifferent()
        {
            var anesthesiaTime1 = new AnesthesiaTime(30);
            var anesthesiaTime2 = new AnesthesiaTime(60);

            bool result = anesthesiaTime1.Equals(anesthesiaTime2);

            Assert.False(result);
        }
    }
}