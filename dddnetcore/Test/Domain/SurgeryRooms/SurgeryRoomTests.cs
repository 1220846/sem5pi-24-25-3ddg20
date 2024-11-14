using System;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.Shared;
using Xunit;

namespace dddnetcore.Tests.Domain.SurgeryRooms
{
    public class SurgeryRoomTests
    {
        [Fact]
        public void CreateSurgeryRoom_WithValidData_ShouldCreateSuccessfully()
        {
            // Arrange
            var roomNumber = new RoomNumber("A123");
            var roomType = RoomType.OPERATING_ROOM;
            var roomCapacity = new SurgeryRoomCapacity(10);
            var maintenanceSlots = new SurgeryRoomMaintenanceSlots("Mon-Fri: 9am-5pm");
            var assignedEquipment = new SurgeryRoomAssignedEquipment("Scalpel, Monitor");
            var currentStatus = SurgeryRoomCurrentStatus.AVAILABLE;

            // Act
            var surgeryRoom = new SurgeryRoom(
                roomNumber, roomType, roomCapacity, maintenanceSlots, assignedEquipment, currentStatus
            );

            // Assert
            Assert.NotNull(surgeryRoom);
            Assert.Equal(roomNumber, surgeryRoom.Id);
            Assert.Equal(roomType, surgeryRoom.RoomType);
            Assert.Equal(roomCapacity, surgeryRoom.RoomCapacity);
            Assert.Equal(maintenanceSlots, surgeryRoom.MaintenanceSlots);
            Assert.Equal(assignedEquipment, surgeryRoom.AssignedEquipment);
            Assert.Equal(currentStatus, surgeryRoom.CurrentStatus);
        }

        [Fact]
        public void CreateSurgeryRoom_WithInvalidCapacity_ShouldThrowException()
        {
            // Arrange
            var roomNumber = new RoomNumber("A123");
            var roomType = RoomType.OPERATING_ROOM;
            var maintenanceSlots = new SurgeryRoomMaintenanceSlots("Mon-Fri: 9am-5pm");
            var assignedEquipment = new SurgeryRoomAssignedEquipment("Scalpel, Monitor");
            var currentStatus = SurgeryRoomCurrentStatus.AVAILABLE;

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => 
                new SurgeryRoom(roomNumber, roomType, new SurgeryRoomCapacity(0), maintenanceSlots, assignedEquipment, currentStatus)
            );
        }

        [Fact]
        public void CreateSurgeryRoom_WithEmptyAssignedEquipment_ShouldThrowException()
        {
            // Arrange
            var roomNumber = new RoomNumber("A123");
            var roomType = RoomType.CONSULTATION_ROOM;
            var roomCapacity = new SurgeryRoomCapacity(5);
            var maintenanceSlots = new SurgeryRoomMaintenanceSlots("Mon-Fri: 9am-5pm");
            var currentStatus = SurgeryRoomCurrentStatus.UNDER_MAINTENANCE;

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => 
                new SurgeryRoom(roomNumber, roomType, roomCapacity, maintenanceSlots, new SurgeryRoomAssignedEquipment(""), currentStatus)
            );
        }
    }
}
