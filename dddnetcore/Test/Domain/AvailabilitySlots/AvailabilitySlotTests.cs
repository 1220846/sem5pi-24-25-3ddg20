using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Domain.Shared;
using Xunit;

namespace Domain.AvailabilitySlots
{
    public class AvailabilitySlotTests
    {
        [Fact]
        public void CreateAvailabilitySlotShouldSucceed()
        {
            
            var startTime = new StartTime(DateTime.Now);
            var endTime = new EndTime(DateTime.Now.AddHours(1));

            
            var availabilitySlot = new AvailabilitySlot(startTime, endTime);

            
            Assert.NotNull(availabilitySlot);
            Assert.Equal(startTime, availabilitySlot.StartTime);
            Assert.Equal(endTime, availabilitySlot.EndTime);
            Assert.NotNull(availabilitySlot.Id);
        }

        [Fact]
        public void AvailabilitySlotEqualsShouldReturnTrueForSameId()
        {
            
            var startTime = new StartTime(DateTime.Now);
            var endTime = new EndTime(DateTime.Now.AddHours(1));

            var availabilitySlot = new AvailabilitySlot(startTime, endTime);

            var result = availabilitySlot.Equals(availabilitySlot);

            Assert.True(result); 
        }

        [Fact]
        public void AvailabilitySlotEqualsShouldReturnFalseForDifferentObject()
        {
            
            var startTime = new StartTime(DateTime.Now);
            var endTime = new EndTime(DateTime.Now.AddHours(1));

            var availabilitySlot = new AvailabilitySlot(startTime, endTime);

            
            var result = availabilitySlot.Equals(new object());

            
            Assert.False(result); 
        }

        [Fact]
        public void AvailabilitySlotShouldNotAllowToEndBeforeItStarts() {
            var startTime = new StartTime(DateTime.Now);
            var endTime = new EndTime(DateTime.Now.AddHours(-1));

            var ex = Assert.Throws<BusinessRuleValidationException>(() => new AvailabilitySlot (startTime, endTime));

            Assert.Equal("Start time of availability slot must be before the end time!", ex.Message);
        }
    }
}