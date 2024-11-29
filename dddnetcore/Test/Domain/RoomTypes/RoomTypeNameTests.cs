using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.RoomTypes;
using Xunit;

namespace DDDSample1.Tests.Domain.RoomTypes
{
    public class RoomTypeNameTests{
        [Fact]
        public void ConstructorShouldCreateRoomTypeNameWithValidName(){
            var validName = "ICU";

            var roomTypeName = new RoomTypeName(validName);

            Assert.Equal(validName, roomTypeName.Name);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionWhenNameIsNull(){
            string invalidName = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new RoomTypeName(invalidName));
            Assert.Equal("The name of room type cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionWhenNameIsEmpty(){
            string invalidName = "";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new RoomTypeName(invalidName));
            Assert.Equal("The name of room type cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenNamesAreEqual(){
            var roomTypeName1 = new RoomTypeName("ICU");
            var roomTypeName2 = new RoomTypeName("ICU");

            bool areEqual = roomTypeName1.Equals(roomTypeName2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenNamesAreDifferent(){
            var roomTypeName1 = new RoomTypeName("ICU");
            var roomTypeName2 = new RoomTypeName("Operation Room");

            bool areEqual = roomTypeName1.Equals(roomTypeName2);

            Assert.False(areEqual);
        }
    }
}