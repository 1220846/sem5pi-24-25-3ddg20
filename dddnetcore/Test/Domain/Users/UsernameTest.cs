using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using Xunit;

/*namespace DDDSample1.Tests.Domain.Users
{
    public class UsernameTests
    {
        /*[Fact]
        public void CreateUsername_ValidEmail_ShouldCreateSuccessfully()
        {
            // Arrange
            var validEmail = "user@example.com";

            // Act
            var username = new Username(validEmail);

            // Assert
            Assert.NotNull(username);
            Assert.Equal(validEmail, username.AsString());
        }

        [Fact]
        public void CreateUsername_ValidMechanographicNumber_ShouldCreateSuccessfully()
        {
            // Arrange
            var role = Role.NURSE;
            var mechanographicNumber = "20240003";

            // Act
            var username = new Username(role, mechanographicNumber);

            // Assert
            Assert.NotNull(username);
            Assert.Equal($"N{mechanographicNumber}@sarm.com", username.AsString());
        }

        [Fact]
        public void CreateUsername_InvalidEmail_ShouldThrowException()
        {
            // Arrange
            var invalidEmail = "invalid-email";

            // Act & Assert
            Assert.Throws<BusinessRuleValidationException>(() => new Username(invalidEmail));
        }

        [Fact]
        public void Equals_WithSameName_ShouldReturnTrue()
        {
            // Arrange
            var username1 = new Username("user@example.com");
            var username2 = new Username("user@example.com");

            // Act & Assert
            Assert.True(username1.Equals(username2));
        }

        [Fact]
        public void Equals_WithDifferentName_ShouldReturnFalse()
        {
            // Arrange
            var username1 = new Username("user1@example.com");
            var username2 = new Username("user2@example.com");

            // Act & Assert
            Assert.False(username1.Equals(username2));
        }

        [Fact]
        public void GetHashCode_ShouldReturnCorrectHash()
        {
            // Arrange
            var username = new Username("user@example.com");

            // Act
            var expectedHashCode = username.AsString().GetHashCode();
            var actualHashCode = username.GetHashCode();

            // Assert
            Assert.Equal(expectedHashCode, actualHashCode);
        }
    }
}
*/