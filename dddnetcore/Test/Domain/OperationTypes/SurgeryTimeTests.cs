using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationTypes
{
    public class SurgeryTimeTests{
        [Fact]
        public void ConstructorShouldCreateSurgeryTimeWithValidMinutes(){
            int validMinutes = 60;

            var surgeryTime = new SurgeryTime(validMinutes);

            Assert.Equal(validMinutes, surgeryTime.Minutes);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenMinutesAreNegative(){
            int invalidMinutes = -10;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new SurgeryTime(invalidMinutes));
            Assert.Equal("The surgery time must be positive", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenMinutesAreZero(){
            int invalidMinutes = 0;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new SurgeryTime(invalidMinutes));
            Assert.Equal("The surgery time must be positive", exception.Message);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenMinutesAreEqual(){
            var surgeryTime1 = new SurgeryTime(30);
            var surgeryTime2 = new SurgeryTime(30);

            bool result = surgeryTime1.Equals(surgeryTime2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenMinutesAreDifferent(){
            var surgeryTime1 = new SurgeryTime(40);
            var surgeryTime2 = new SurgeryTime(60);

            bool result = surgeryTime1.Equals(surgeryTime2);

            Assert.False(result);
        }
    }
}