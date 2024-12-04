using Xunit;
using DDDSample1.Domain.RoomTypes;

namespace DDDSample1.Tests.Domain.RoomTypes
{
    public class RoomTypeIsSurgicalTests
    {
        [Fact]
        public void ConstructorShouldCreateSurgicalRoomType()
        {
            // Arrange
            bool isSurgical = true;

            // Act
            var roomTypeIsSurgical = new RoomTypeIsSurgical(isSurgical);

            // Assert
            Assert.NotNull(roomTypeIsSurgical);
            Assert.True(roomTypeIsSurgical.IsSurgical);
        }

        [Fact]
        public void ConstructorShouldCreateNonSurgicalRoomType()
        {
            // Arrange
            bool isSurgical = false;

            // Act
            var roomTypeIsSurgical = new RoomTypeIsSurgical(isSurgical);

            // Assert
            Assert.NotNull(roomTypeIsSurgical);
            Assert.False(roomTypeIsSurgical.IsSurgical);
        }

        [Fact]
        public void EqualsShouldReturnTrueForSameValues()
        {
            // Arrange
            var surgical1 = new RoomTypeIsSurgical(true);
            var surgical2 = new RoomTypeIsSurgical(true);

            // Act & Assert
            Assert.True(surgical1.Equals(surgical2));
        }

        [Fact]
        public void EqualsShouldReturnFalseForDifferentValues()
        {
            // Arrange
            var surgical = new RoomTypeIsSurgical(true);
            var nonSurgical = new RoomTypeIsSurgical(false);

            // Act & Assert
            Assert.False(surgical.Equals(nonSurgical));
        }

        [Fact]
        public void GetHashCodeShouldReturnSameHashCodeForSameValues()
        {
            // Arrange
            var surgical1 = new RoomTypeIsSurgical(true);
            var surgical2 = new RoomTypeIsSurgical(true);

            // Act & Assert
            Assert.Equal(surgical1.GetHashCode(), surgical2.GetHashCode());
        }

        [Fact]
        public void GetHashCodeShouldReturnDifferentHashCodeForDifferentValues()
        {
            // Arrange
            var surgical = new RoomTypeIsSurgical(true);
            var nonSurgical = new RoomTypeIsSurgical(false);

            // Act & Assert
            Assert.NotEqual(surgical.GetHashCode(), nonSurgical.GetHashCode());
        }

        [Fact]
        public void ToStringShouldReturnSurgicalForTrue()
        {
            // Arrange
            var surgical = new RoomTypeIsSurgical(true);

            // Act
            var result = surgical.ToString();

            // Assert
            Assert.Equal("Surgical", result);
        }

        [Fact]
        public void ToStringShouldReturnNonSurgicalForFalse()
        {
            // Arrange
            var nonSurgical = new RoomTypeIsSurgical(false);

            // Act
            var result = nonSurgical.ToString();

            // Assert
            Assert.Equal("Non-Surgical", result);
        }
    }
}
