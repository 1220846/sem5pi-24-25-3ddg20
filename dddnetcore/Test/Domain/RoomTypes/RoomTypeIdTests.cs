using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.RoomTypes;
using Xunit;

namespace DDDSample1.Tests.Domain.RoomTypes
{
    public class RoomTypeIdTests
    {
        [Fact]
        public void ConstructorShouldCreateRoomTypeIdWithValidGuid(){
            var validId = Guid.NewGuid();

            var roomTypeId = new RoomTypeId(validId);

            Assert.Equal(validId, roomTypeId.AsGuid());
        }

        [Fact]
        public void ConstructorShouldCreateRoomTypeIdWithValidString(){

            var validId = Guid.NewGuid().ToString();

            var roomTypeId = new RoomTypeId(validId);

            Assert.Equal(new Guid(validId), roomTypeId.AsGuid());
        }

        [Fact]
        public void AsStringShouldReturnCorrectStringRepresentation(){
            var roomTypeId = new RoomTypeId(Guid.NewGuid());
            
            var stringRepresentation = roomTypeId.AsString();

            Assert.Equal(roomTypeId.AsGuid().ToString(), stringRepresentation);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenIdsAreEqual(){
            var id = Guid.NewGuid();
            var id1 = new RoomTypeId(id);
            var id2 = new RoomTypeId(id);

            bool areEqual = id1.Equals(id2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenIdsAreDifferent(){
            var id1 = new RoomTypeId(Guid.NewGuid());
            var id2 = new RoomTypeId(Guid.NewGuid());

            bool areEqual = id1.Equals(id2);

            Assert.False(areEqual);
        }
    }
}