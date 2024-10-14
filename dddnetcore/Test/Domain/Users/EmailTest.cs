using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.Users
{
    public class EmailTests
    {
        [Fact]
        public void CreateEmailValidAddressShouldCreateSuccessfully()
        {
            var validEmail = "user@example.com";

            var email = new Email(validEmail);

            Assert.NotNull(email);
            Assert.Equal(validEmail, email.Address);
        }

        [Fact]
        public void CreateEmailInvalidAddressShouldThrowException()
        {
            var invalidEmail = "invalid-email";

            Assert.Throws<BusinessRuleValidationException>(() => new Email(invalidEmail));
        }

        [Fact]
        public void EqualsWithSameAddressShouldReturnTrue()
        {
            var email1 = new Email("user@example.com");
            var email2 = new Email("user@example.com");

            Assert.True(email1.Equals(email2));
        }

        [Fact]
        public void EqualsWithDifferentAddressShouldReturnFalse()
        {
            var email1 = new Email("user1@example.com");
            var email2 = new Email("user2@example.com");

            Assert.False(email1.Equals(email2));
        }

        [Fact]
        public void GetHashCodeShouldReturnCorrectHash()
        {
            var email = new Email("user@example.com");

            var expectedHashCode = email.Address.GetHashCode();
            var actualHashCode = email.GetHashCode();

            Assert.Equal(expectedHashCode, actualHashCode);
        }
    }
}
