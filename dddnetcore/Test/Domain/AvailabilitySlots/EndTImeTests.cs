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
        public void CreateEndTimeShouldSucceedForAValidInput()
        {
            
            var futureTime = DateTime.Now.AddHours(1);

            
            var endTime = new EndTime(futureTime);

            
            Assert.NotNull(endTime);
            Assert.Equal(futureTime, endTime.Time);
        }

        [Fact]
        public void EndTimeEqualsShouldReturnTrueForSameTime()
        {
            
            var time = DateTime.Now.AddHours(1);
            var endTime1 = new EndTime(time);
            var endTime2 = new EndTime(time);

            
            var result = endTime1.Equals(endTime2);

            
            Assert.True(result);
        }

        [Fact]
        public void EndTimeEqualsShouldReturnFalseForDifferentTimes()
        {
            
            var endTime1 = new EndTime(DateTime.Now.AddHours(1));
            var endTime2 = new EndTime(DateTime.Now.AddHours(2));

            
            var result = endTime1.Equals(endTime2);

            
            Assert.False(result);
        }

        [Fact]
        public void EndTimeGetHashCodeShouldReturnSameHashCodeForSameTime()
        {
            
            var time = DateTime.Now.AddHours(1);
            var endTime1 = new EndTime(time);
            var endTime2 = new EndTime(time);

            
            var hashCode1 = endTime1.GetHashCode();
            var hashCode2 = endTime2.GetHashCode();

            
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void EndTimeGetHashCodeShouldReturnDifferentHashCodesForDifferentTimes()
        {
            
            var endTime1 = new EndTime(DateTime.Now.AddHours(1));
            var endTime2 = new EndTime(DateTime.Now.AddHours(2));

            
            var hashCode1 = endTime1.GetHashCode();
            var hashCode2 = endTime2.GetHashCode();

            
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}