using System;
using DDDSample1.Domain.RoomTypes;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.RoomTypes
{
    public class RoomTypeTests
    {
        [Fact]
        public void ConstructorShouldSetPropertiesCorrectly()
        {
            var roomTypeCode = new RoomTypeCode("ABC12345");
            var roomTypeDesignation = new RoomTypeDesignation("ICU");
            var roomTypeDescription = new RoomTypeDescription("Intensive Care Unit");
            var roomTypeIsSurgical = new RoomTypeIsSurgical(true);

            var roomType = new RoomType(roomTypeCode, roomTypeDesignation, roomTypeDescription,roomTypeIsSurgical);

            Assert.Equal(roomTypeCode, roomType.Id);
            Assert.Equal(roomTypeDesignation, roomType.Designation);
            Assert.Equal(roomTypeDescription, roomType.Description);
            Assert.Equal(roomTypeIsSurgical,roomType.IsSurgical);
        }

        [Fact]
        public void EqualsShouldReturnTrueForEqualObjects()
        {
            var roomTypeCode = new RoomTypeCode("ABC12345");
            var roomTypeDesignation = new RoomTypeDesignation("ICU");
            var roomTypeDescription = new RoomTypeDescription("Intensive Care Unit");
            var roomTypeIsSurgical = new RoomTypeIsSurgical(true);

            var roomType1 = new RoomType(roomTypeCode, roomTypeDesignation, roomTypeDescription,roomTypeIsSurgical);
            var roomType2 = new RoomType(roomTypeCode, roomTypeDesignation, roomTypeDescription,roomTypeIsSurgical);

            var result = roomType1.Equals(roomType2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseForDifferentObjects()
        {
            var roomTypeCode1 = new RoomTypeCode("ABC12345");
            var roomTypeCode2 = new RoomTypeCode("DEF67890");
            var roomTypeDesignation = new RoomTypeDesignation("ICU");
            var roomTypeDescription = new RoomTypeDescription("Intensive Care Unit");
            var roomTypeIsSurgical = new RoomTypeIsSurgical(true);

            var roomType1 = new RoomType(roomTypeCode1, roomTypeDesignation, roomTypeDescription,roomTypeIsSurgical);
            var roomType2 = new RoomType(roomTypeCode2, roomTypeDesignation, roomTypeDescription,roomTypeIsSurgical);

            var result = roomType1.Equals(roomType2);

            Assert.False(result);
        }

        [Fact]
        public void GetHashCodeShouldReturnSameValueForEqualObjects()
        {
            var roomTypeCode = new RoomTypeCode("ABC12345");
            var roomTypeDesignation = new RoomTypeDesignation("ICU");
            var roomTypeDescription = new RoomTypeDescription("Intensive Care Unit");
            var roomTypeIsSurgical = new RoomTypeIsSurgical(true);

            var roomType1 = new RoomType(roomTypeCode, roomTypeDesignation, roomTypeDescription,roomTypeIsSurgical);
            var roomType2 = new RoomType(roomTypeCode, roomTypeDesignation, roomTypeDescription,roomTypeIsSurgical);

            var hashCode1 = roomType1.GetHashCode();
            var hashCode2 = roomType2.GetHashCode();

            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void GetHashCodeShouldReturnDifferentValuesForDifferentObjects()
        {
            var roomTypeCode1 = new RoomTypeCode("ABC12345");
            var roomTypeCode2 = new RoomTypeCode("DEF67890");
            var roomTypeDesignation = new RoomTypeDesignation("ICU");
            var roomTypeDescription = new RoomTypeDescription("Intensive Care Unit");
            var roomTypeIsSurgical = new RoomTypeIsSurgical(true);

            var roomType1 = new RoomType(roomTypeCode1, roomTypeDesignation, roomTypeDescription,roomTypeIsSurgical);
            var roomType2 = new RoomType(roomTypeCode2, roomTypeDesignation, roomTypeDescription,roomTypeIsSurgical);

            var hashCode1 = roomType1.GetHashCode();
            var hashCode2 = roomType2.GetHashCode();

            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}
