using DDDSample1.Domain.RoomTypes;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.RoomTypes
{
    public class RoomTypeTests
    {
        [Fact]
        public void CreateValidNameShouldCreateRoomType() {
            var specializationName = new RoomTypeName("ICU");

            var specialization = new RoomType(specializationName);

            Assert.NotNull(specialization);
            Assert.Equal(specializationName, specialization.Name);
        }

        [Fact]
        public void CreateNullOrEmptyNameShouldThrowBusinessRuleValidationException(){
            string invalidName = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => {new RoomTypeName(invalidName);});

            Assert.Equal("The name of room type cannot be null or empty!", exception.Message);

            invalidName = "";

            exception = Assert.Throws<BusinessRuleValidationException>(() =>{new RoomTypeName(invalidName);});

            Assert.Equal("The name of room type cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EqualsSameIdShouldReturnTrue(){
            var specializationName = new RoomTypeName("ICU");
            var specialization1 = new RoomType(specializationName);

            bool result = specialization1.Equals(specialization1);

            Assert.True(result);
        }

        [Fact]
        public void EqualsDifferentIdShouldReturnTrue(){
            var specializationName = new RoomTypeName("Operation Room");
            var specialization1 = new RoomType(specializationName);
            var specialization2 = new RoomType(specializationName);

            bool result = specialization1.Equals(specialization2);

            Assert.False(result);
        }
    }
}