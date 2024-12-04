using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.RoomTypes;
using Xunit;

namespace DDDSample1.Tests.Domain.RoomTypes
{
    public class RoomTypeDesignationTests{
        [Fact]
        public void ConstructorShouldCreateRoomTypeDesignationWithValidDesignation(){
            var validDesignation = "ICU";

            var roomTypeDesignation = new RoomTypeDesignation(validDesignation);

            Assert.Equal(validDesignation, roomTypeDesignation.Designation);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionWhenDesignationIsNull(){
            string invalidDesignation = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new RoomTypeDesignation(invalidDesignation));
            Assert.Equal("The designation of room type cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionWhenDesignationIsEmpty(){
            string invalidDesignation = "";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new RoomTypeDesignation(invalidDesignation));
            Assert.Equal("The designation of room type cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionWhenDesignationIsInvalid(){
            string invalidDesignation = "!@#$";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new RoomTypeDesignation(invalidDesignation));
            Assert.Equal("The designation of room type must be alphanumeric and may include spaces!", exception.Message);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenDesignationsAreEqual(){
            var roomTypeDesignation1 = new RoomTypeDesignation("ICU");
            var roomTypeDesignation2 = new RoomTypeDesignation("ICU");

            bool areEqual = roomTypeDesignation1.Equals(roomTypeDesignation2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenDesignationsAreDifferent(){
            var roomTypeDesignation1 = new RoomTypeDesignation("ICU");
            var roomTypeDesignation2 = new RoomTypeDesignation("Operation Room");

            bool areEqual = roomTypeDesignation1.Equals(roomTypeDesignation2);

            Assert.False(areEqual);
        }
    }
}