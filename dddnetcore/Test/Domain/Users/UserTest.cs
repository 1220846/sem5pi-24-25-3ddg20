using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using Xunit;
using System;

namespace DDDSample1.Tests.Domain.Users
{
    public class UserTests
    {
        [Fact]
        public void CreateUserValidParametersShouldCreateUserSuccessfully()
        {
            var username = new Username("D240003");
            var email = new Email("user@example.com");
            var role = Role.NURSE;

            var user = new User(username, email, role);

            Assert.NotNull(user);
            Assert.Equal(username, user.Id);
            Assert.Equal(email, user.Email);
            Assert.Equal(role, user.Role);
        }

        [Fact]
        public void EqualsWithSameUsernameShouldReturnTrue()
        {
            var username1 = new Username("D240003");
            var email1 = new Email("user1@example.com");
            var role1 = Role.NURSE;

            var user1 = new User(username1, email1, role1);

            var username2 = new Username("D240003");
            var email2 = new Email("user2@example.com");
            var role2 = Role.NURSE;

            var user2 = new User(username2, email2, role2);

            Assert.True(user1.Equals(user2));
        }

        [Fact]
        public void EqualsWithDifferentUsernameShouldReturnFalse()
        {
            var username1 = new Username("D240003");
            var email1 = new Email("user1@example.com");
            var role1 = Role.ADMIN;

            var user1 = new User(username1, email1, role1);

            var username2 = new Username("D240004");
            var email2 = new Email("user2@example.com");
            var role2 = Role.ADMIN;

            var user2 = new User(username2, email2, role2);

            Assert.False(user1.Equals(user2));
        }

        [Fact]
        public void GetHashCodeShouldReturnCorrectHash()
        {
            var username = new Username("D240003");
            var email = new Email("user@example.com");
            var role =Role.DOCTOR;

            var user = new User(username, email, role);

            var expectedHashCode = HashCode.Combine(username, email, role);
            var actualHashCode = user.GetHashCode();

            Assert.Equal(expectedHashCode, actualHashCode);
        }
    }
}
