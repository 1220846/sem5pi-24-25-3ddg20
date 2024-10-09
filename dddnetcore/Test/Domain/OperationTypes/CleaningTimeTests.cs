using System;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationTypes
{
    public class CleaningTimeTests{
        [Fact]
        public void ConstructorShouldCreateInstanceWithValidMinutes(){
            int validMinutes = 30;

            var cleaningTime = new CleaningTime(validMinutes);

            Assert.Equal(validMinutes, cleaningTime.Minutes);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenMinutesAreNegative(){
            int invalidMinutes = -10;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new CleaningTime(invalidMinutes));
            Assert.Equal("The cleaning time must be positive", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionnWhenMinutesAreZero(){
            int invalidMinutes = 0;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new CleaningTime(invalidMinutes));
            Assert.Equal("The cleaning time must be positive", exception.Message);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenMinutesAreEqual(){
            var cleaningTime1 = new CleaningTime(45);
            var cleaningTime2 = new CleaningTime(45);

            bool result = cleaningTime1.Equals(cleaningTime2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenMinutesAreDifferent(){
            var cleaningTime1 = new CleaningTime(30);
            var cleaningTime2 = new CleaningTime(60);

            bool result = cleaningTime1.Equals(cleaningTime2);

            Assert.False(result);
        }
    }
}
