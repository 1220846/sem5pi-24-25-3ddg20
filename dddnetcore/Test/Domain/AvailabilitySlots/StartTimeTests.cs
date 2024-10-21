using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.AvailabilitySlots
{
    public class StartTimeTests
    {
        [Fact]
        public void CreateStartTimeShouldSucceedForAValidInput()
        {
            
            var futureTime = DateTime.Now;

            
            var endTime = new StartTime(futureTime);

            
            Assert.NotNull(endTime);
            Assert.Equal(futureTime, endTime.Time);
        }

        [Fact]
        public void StartTimeEqualsShouldReturnTrueForSameTime()
        {
            
            var time = DateTime.Now.AddHours(1);
            var endTime1 = new StartTime(time);
            var endTime2 = new StartTime(time);

            
            var result = endTime1.Equals(endTime2);

            
            Assert.True(result);
        }

        [Fact]
        public void StartTimeEqualsShouldReturnFalseForDifferentTimes()
        {
            
            var endTime1 = new StartTime(DateTime.Now.AddHours(1));
            var endTime2 = new StartTime(DateTime.Now.AddHours(2));

            
            var result = endTime1.Equals(endTime2);

            
            Assert.False(result);
        }

        [Fact]
        public void StartTimeGetHashCodeShouldReturnSameHashCodeForSameTime()
        {
            
            var time = DateTime.Now.AddHours(1);
            var endTime1 = new StartTime(time);
            var endTime2 = new StartTime(time);

            
            var hashCode1 = endTime1.GetHashCode();
            var hashCode2 = endTime2.GetHashCode();

            
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void StartTimeGetHashCodeShouldReturnDifferentHashCodesForDifferentTimes()
        {
            
            var endTime1 = new StartTime(DateTime.Now.AddHours(1));
            var endTime2 = new StartTime(DateTime.Now.AddHours(2));

            
            var hashCode1 = endTime1.GetHashCode();
            var hashCode2 = endTime2.GetHashCode();

            
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}