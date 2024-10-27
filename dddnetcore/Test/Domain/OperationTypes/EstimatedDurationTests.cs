using System;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationTypes
{
    public class EstimatedDurationTests
    {
        [Fact]
        public void ConstructorShouldCreateInstanceWithValidMinutes()
        {
            int validMinutes = 45;

            var estimatedDuration = new EstimatedDuration(validMinutes);

            Assert.Equal(validMinutes, estimatedDuration.Minutes);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenMinutesAreNegative()
        {
            int invalidMinutes = -15;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new EstimatedDuration(invalidMinutes));
            Assert.Equal("The estimated duration must be positive", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenMinutesAreZero()
        {
            int invalidMinutes = 0;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new EstimatedDuration(invalidMinutes));
            Assert.Equal("The estimated duration must be positive", exception.Message);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenMinutesAreEqual()
        {
            var estimatedDuration1 = new EstimatedDuration(30);
            var estimatedDuration2 = new EstimatedDuration(30);

            bool result = estimatedDuration1.Equals(estimatedDuration2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenMinutesAreDifferent()
        {
            var estimatedDuration1 = new EstimatedDuration(20);
            var estimatedDuration2 = new EstimatedDuration(40);

            bool result = estimatedDuration1.Equals(estimatedDuration2);

            Assert.False(result);
        }
    }
}