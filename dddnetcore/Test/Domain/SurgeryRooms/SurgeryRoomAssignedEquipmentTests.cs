using System;
using Xunit;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Tests.Domain.SurgeryRooms
{
    public class SurgeryRoomAssignedEquipmentTests
    {
        [Fact]
        public void CreateSurgeryRoomAssignedEquipmentShouldSetCorrectValue()
        {
            
            string equipment = "Heart Monitor";

            
            var assignedEquipment = new SurgeryRoomAssignedEquipment(equipment);

            
            Assert.Equal(equipment, assignedEquipment.AssignedEquipment);
        }

        [Fact]
        public void CreateSurgeryRoomAssignedEquipmentWithEmptyStringShouldThrowBusinessRuleValidationException()
        {
            
            string equipment = "";

            
            Assert.Throws<BusinessRuleValidationException>(() => new SurgeryRoomAssignedEquipment(equipment));
        }

        [Fact]
        public void CreateSurgeryRoomAssignedEquipmentWithNullStringShouldThrowBusinessRuleValidationException()
        {
            
            Assert.Throws<BusinessRuleValidationException>(() => new SurgeryRoomAssignedEquipment(null));
        }

        [Fact]
        public void AssignedEquipmentEqualityShouldBeTrueForSameValue()
        {
            
            var equipment1 = new SurgeryRoomAssignedEquipment("Surgical Light");
            var equipment2 = new SurgeryRoomAssignedEquipment("Surgical Light");

            
            Assert.True(equipment1.Equals(equipment2));
            Assert.Equal(equipment1.GetHashCode(), equipment2.GetHashCode());
        }

        [Fact]
        public void AssignedEquipmentEqualityShouldBeFalseForDifferentValue()
        {
            
            var equipment1 = new SurgeryRoomAssignedEquipment("Ventilator");
            var equipment2 = new SurgeryRoomAssignedEquipment("Anesthesia Machine");

            
            Assert.False(equipment1.Equals(equipment2));
            Assert.NotEqual(equipment1.GetHashCode(), equipment2.GetHashCode());
        }
    }
}