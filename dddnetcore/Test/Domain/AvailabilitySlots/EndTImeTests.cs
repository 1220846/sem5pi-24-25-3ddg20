using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.AvailabilitySlots
{
    public class EndTimeTests
    {
        [Fact]
        public void EnsureValidTimestampIsAllowed() {
            long validTime = 1727272800;

            var time = new EndTime(validTime);

            Assert.Equal(validTime, time.Time);
        }

        [Fact]
        public void EnsureNegativeTimestampThrowsException() {
            long invalidTime = -1;
            
            var exception = Assert.Throws<BusinessRuleValidationException>(() => new EndTime(invalidTime));
            Assert.Equal("Timestamp cannot be negative!", exception.Message);
        }

        [Fact]
        public void EnsureEqualsReturnsTrueWhenEndTimesAreEqual() {
            long validTime1 = 1727272800;
            long validTime2 = 1727272800;

            var _validTime1 = new EndTime(validTime1);
            var _validTime2 = new EndTime(validTime2);

            Assert.True(_validTime1.Equals(_validTime2));
        }

        [Fact]
        public void EnsureEqualsReturnsFalseWhenEndTimesAreDifferent() {
            long validTime1 = 1727272800;
            long validTime2 = 1727272801;

            var _validTime1 = new EndTime(validTime1);
            var _validTime2 = new EndTime(validTime2);

            Assert.False(_validTime1.Equals(_validTime2));
        }
    }
}