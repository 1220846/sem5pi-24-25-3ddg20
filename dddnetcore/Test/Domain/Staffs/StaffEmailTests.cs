using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
using Xunit;

namespace DDDSample1.Tests.Domain.Staffs
{
    public class StaffEmailTests
    {
        [Fact]
        public void EnsureValidEmailIsAllowed() {
            string validEmail = "one@valid.email";

            var email = new StaffEmail(validEmail);

            Assert.Equal(validEmail, email.Email);
        }

        [Fact]
        public void EnsureNullEmailThrowsException() {
            string invalidEmail = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffEmail(invalidEmail));
            Assert.Equal("Email of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEmptyEmailThrowsException() {
            string invalidEmail = "";

            
            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffEmail(invalidEmail));
            Assert.Equal("Email of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureBadlyFormattedEmailThrowsException() {
            string invalidEmail = "thisisaninvalidemail";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffEmail(invalidEmail));
            Assert.Equal("Email format is not valid!", exception.Message);
        }

        [Fact]
        public void EnsureEqualsReturnsTrueWhenEmailsAreEqual() {
            string email1 = "one@valid.email";
            string email2 = "one@valid.email";

            var _email1 = new StaffEmail(email1);
            var _email2 = new StaffEmail(email2);

            Assert.True(_email1.Equals(_email2));
        }

        [Fact]
        public void EnsureEqualsReturnsFalseWhenEmailsAreDifferent() {
            string email1 = "one@valid.email";
            string email2 = "another@valid.email";

            var _email1 = new StaffEmail(email1);
            var _email2 = new StaffEmail(email2);

            Assert.False(_email1.Equals(_email2));
        }
    }

    internal class FactAttribute : Attribute
    {
    }
}