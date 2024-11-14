using System;
using Xunit;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Tests.Domain.SurgeryRooms
{
    public class SurgeryRoomMaintenanceSlotsTests
    {
        [Fact]
        public void CreateSurgeryRoomMaintenanceSlotsShouldSetCorrectValue()
        {
            
            string slots = "Monday 10:00-12:00";

            
            var maintenanceSlots = new SurgeryRoomMaintenanceSlots(slots);

            
            Assert.Equal(slots, maintenanceSlots.MaintenanceSlots);
        }

        [Fact]
        public void CreateSurgeryRoomMaintenanceSlotsWithEmptyStringShouldThrowBusinessRuleValidationException()
        {
            
            string slots = "";

            
            Assert.Throws<BusinessRuleValidationException>(() => new SurgeryRoomMaintenanceSlots(slots));
        }

        [Fact]
        public void CreateSurgeryRoomMaintenanceSlotsWithNullStringShouldThrowBusinessRuleValidationException()
        {
            
            Assert.Throws<BusinessRuleValidationException>(() => new SurgeryRoomMaintenanceSlots(null));
        }

        [Fact]
        public void MaintenanceSlotsEqualityShouldBeTrueForSameValue()
        {
            
            var slots1 = new SurgeryRoomMaintenanceSlots("Friday 14:00-16:00");
            var slots2 = new SurgeryRoomMaintenanceSlots("Friday 14:00-16:00");

            
            Assert.True(slots1.Equals(slots2));
            Assert.Equal(slots1.GetHashCode(), slots2.GetHashCode());
        }

        [Fact]
        public void MaintenanceSlotsEqualityShouldBeFalseForDifferentValue()
        {
            
            var slots1 = new SurgeryRoomMaintenanceSlots("Tuesday 08:00-10:00");
            var slots2 = new SurgeryRoomMaintenanceSlots("Wednesday 10:00-12:00");

            
            Assert.False(slots1.Equals(slots2));
            Assert.NotEqual(slots1.GetHashCode(), slots2.GetHashCode());
        }
    }
}