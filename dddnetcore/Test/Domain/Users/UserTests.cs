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
            var email = new Email("user@example.com");
            var role = Role.NURSE;
            var username = Username.Create(role,"D202400003");

            var user = new User(username, email, role);

            Assert.NotNull(user);
            Assert.Equal(username, user.Id);
            Assert.Equal(email, user.Email);
            Assert.Equal(role, user.Role);
        }

        [Fact]
        public void EqualsWithSameUsernameShouldReturnTrue()
        {
            var email1 = new Email("user1@example.com");
            var role1 = Role.NURSE;
            var username1 = Username.Create(role1,"D202400003");

            var user1 = new User(username1, email1, role1);

            var email2 = new Email("user2@example.com");
            var role2 = Role.NURSE;
            var username2 = Username.Create(role2,"D202400003");

            var user2 = new User(username2, email2, role2);

            Assert.True(user1.Equals(user2));
        }

        [Fact]
        public void EqualsWithDifferentUsernameShouldReturnFalse()
        {
            var email1 = new Email("user1@example.com");
            var role1 = Role.ADMIN;
            var username1 = Username.Create(role1,"D202400003");

            var user1 = new User(username1, email1, role1);

            var email2 = new Email("user2@example.com");
            var role2 = Role.ADMIN;
            var username2 = Username.Create(role2,"D202400004");

            var user2 = new User(username2, email2, role2);

            Assert.False(user1.Equals(user2));
        }

        [Fact]
        public void GetHashCodeShouldReturnCorrectHash()
        {
            var email = new Email("user@example.com");
            var role =Role.DOCTOR;
            var username = Username.Create(role,"D202400003");

            var user = new User(username, email, role);

            var expectedHashCode = HashCode.Combine(username, email, role);
            var actualHashCode = user.GetHashCode();

            Assert.Equal(expectedHashCode, actualHashCode);
        }

        [Fact]
        public void ChangeUsernameShouldUpdateUsernameWithValidUsername(){

            var initialUsername = new Username("user@example.com");
            var newUsername = new Username("other@example.com");
            var user = new User(initialUsername, new Email("user@example.com"), Role.PATIENT);
            
            user.ChangeUsername(newUsername);
            
            Assert.Equal(newUsername, user.Id);
        }

        [Fact]
        public void ChangeUsernameShouldThrowArgumentNullExceptionWhenNewUsernameIsNull(){

            var username = new Username("user@example.com");
            var user = new User(username, new Email("user@example.com"), Role.PATIENT);

            var exception = Assert.Throws<ArgumentNullException>(() => user.ChangeUsername(null));
            Assert.Equal("newUsername", exception.ParamName);
        }

        [Fact]
        public void ChangeEmailShouldUpdateEmailWhenValidEmailIsProvided(){

            var username = new Username("user@example.com");
            var initialEmail = new Email("user@example.com");
            var newEmail = new Email("other@example.com");
            var user = new User(username, initialEmail, Role.PATIENT);
            
            user.ChangeEmail(newEmail);
            
            Assert.Equal(newEmail, user.Email);
        }

        [Fact]
        public void ChangeEmail_ShouldThrowArgumentNullException_WhenNewEmailIsNull(){
            
            var username = new Username("user@example.com");
            var initialEmail = new Email("user@example.com");
            var user = new User(username, initialEmail, Role.DOCTOR);

            var exception = Assert.Throws<ArgumentNullException>(() => user.ChangeEmail(null));
            Assert.Equal("newEmail", exception.ParamName);
        }
    }
}
