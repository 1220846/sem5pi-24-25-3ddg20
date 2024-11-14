using System;
using Xunit;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Tests.Domain.SurgeryRooms
{
    public class RoomNumberTests
    {
        [Fact]
        public void CreateRoomNumber_WithValidId_ShouldSetCorrectValue()
        {
            // Arrange
            string validId = "A123";

            // Act
            var roomNumber = new RoomNumber(validId);

            // Assert
            Assert.Equal(validId, roomNumber.Id);
        }

        [Fact]
        public void CreateRoomNumber_WithEmptyString_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            string invalidId = "";

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new RoomNumber(invalidId));
        }

        [Fact]
        public void RoomNumber_Equality_ShouldBeTrueForSameValue()
        {
            // Arrange
            var roomNumber1 = new RoomNumber("B456");
            var roomNumber2 = new RoomNumber("B456");

            // Act & Assert
            Assert.True(roomNumber1.Equals(roomNumber2));
        }

        [Fact]
        public void RoomNumber_Equality_ShouldBeFalseForDifferentValues()
        {
            // Arrange
            var roomNumber1 = new RoomNumber("C789");
            var roomNumber2 = new RoomNumber("D012");

            // Act & Assert
            Assert.False(roomNumber1.Equals(roomNumber2));
        }
    }
}
