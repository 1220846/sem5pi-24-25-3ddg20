using System;
using DDDSample1.Domain.OperationTypeSpecializations;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationTypeSpecializations
{
    public class NumberOfStaffTests
    {
        [Fact]
        public void ConstructorShouldCreateNumberOfStaffWithValidNumber(){
            int validNumber = 5;

            var numberOfStaff = new NumberOfStaff(validNumber);

            Assert.Equal(validNumber, numberOfStaff.Number);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenNumberIsNegative(){
            int invalidNumber = -3;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new NumberOfStaff(invalidNumber));
            Assert.Equal("The number of staff must be positive", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenNumberIsZero(){
            int invalidNumber = 0;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new NumberOfStaff(invalidNumber));
            Assert.Equal("The number of staff must be positive", exception.Message);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenNumbersAreEqual(){
            var numberOfStaff1 = new NumberOfStaff(10);
            var numberOfStaff2 = new NumberOfStaff(10);

            bool result = numberOfStaff1.Equals(numberOfStaff2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenNumbersAreDifferent(){
            var numberOfStaff1 = new NumberOfStaff(7);
            var numberOfStaff2 = new NumberOfStaff(3);

            bool result = numberOfStaff1.Equals(numberOfStaff2);

            Assert.False(result);
        }
    }
}