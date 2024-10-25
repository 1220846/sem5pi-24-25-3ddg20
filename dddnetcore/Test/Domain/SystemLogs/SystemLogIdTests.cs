using System;
using DDDSample1.Domain.SystemLogs;
using Xunit;

namespace DDDSample1.Tests.Domain.SystemLogs
{
    public class SystemLogIdTests
    {
        [Fact]
        public void ConstructorShouldCreateSystemLogIdWithValidGuid()
        {
            Guid validId = Guid.NewGuid();

            var systemLogId = new SystemLogId(validId);

            Assert.Equal(validId, systemLogId.AsGuid());
            Assert.Equal(validId.ToString(), systemLogId.AsString());
        }

        [Fact]
        public void ConstructorShouldCreateSystemLogIdWithValidString()
        {
            string validId = Guid.NewGuid().ToString();

            var systemLogId = new SystemLogId(validId);

            Guid expectedGuid = Guid.Parse(validId);
            Assert.Equal(expectedGuid, systemLogId.AsGuid());
            Assert.Equal(validId, systemLogId.AsString());
        }

        [Fact]
        public void ConstructorShouldThrowFormatExceptionWhenStringIsInvalid(){

            string invalidIdString = "invalidIdString";

            Assert.Throws<FormatException>(() => new SystemLogId(invalidIdString));
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenIdsAreEqual()
        {
            var systemLogId = Guid.NewGuid();
            var systemLogId1 = new SystemLogId(systemLogId);
            var systemLogId2 = new SystemLogId(systemLogId);

            bool result = systemLogId1.Equals(systemLogId2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenIdsAreDifferent()
        {
            var systemLogId1 = new SystemLogId(Guid.NewGuid());
            var systemLogId2 = new SystemLogId(Guid.NewGuid());

            bool result = systemLogId1.Equals(systemLogId2);

            Assert.False(result);
        }
    }
}