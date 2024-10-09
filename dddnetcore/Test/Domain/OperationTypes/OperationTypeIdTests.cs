using System;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationTypes
{
    public class OperationTypeIdTests
    {
        [Fact]
        public void ConstructorShouldCreateOperationTypeIdWithValidGuid()
        {
            Guid validId = Guid.NewGuid();

            var operationTypeId = new OperationTypeId(validId);

            Assert.Equal(validId, operationTypeId.AsGuid());
            Assert.Equal(validId.ToString(), operationTypeId.AsString());
        }

        [Fact]
        public void ConstructorShouldCreateOperationTypeIdWithValidString()
        {
            string validId = Guid.NewGuid().ToString();

            var operationTypeId = new OperationTypeId(validId);

            Guid expectedGuid = Guid.Parse(validId);
            Assert.Equal(expectedGuid, operationTypeId.AsGuid());
            Assert.Equal(validId, operationTypeId.AsString());
        }

        [Fact]
        public void ConstructorShouldThrowFormatExceptionWhenStringIsInvalid(){

            string invalidIdString = "invalidIdString";

            Assert.Throws<FormatException>(() => new OperationTypeId(invalidIdString));
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenIdsAreEqual()
        {
            var operationTypeId = Guid.NewGuid();
            var operationTypeId1 = new OperationTypeId(operationTypeId);
            var operationTypeId2 = new OperationTypeId(operationTypeId);

            bool result = operationTypeId1.Equals(operationTypeId2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenIdsAreDifferent()
        {
            var operationTypeId1 = new OperationTypeId(Guid.NewGuid());
            var operationTypeId2 = new OperationTypeId(Guid.NewGuid());

            bool result = operationTypeId1.Equals(operationTypeId2);

            Assert.False(result);
        }
    }
}