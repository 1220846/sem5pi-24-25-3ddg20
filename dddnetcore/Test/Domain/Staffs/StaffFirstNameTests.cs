using System;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using Xunit;

namespace Domain.Staffs
{
    public class StaffFirstNameTests
    {
        [Fact]
        public void EnsureValidNameIsAllowed() {
            string validName = "Abcdef";

            var name = new StaffFirstName(validName);

            Assert.Equal(validName, name);
        }

        [Fact]
        public void EnsureNullNameThrowsException() {
            string invalidName = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffFirstName(invalidName));
            Assert.Equal("Fist name of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEmptyNameThrowsException() {
            string invalidName = "";

            
            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffFirstName(invalidName));
            Assert.Equal("Fist name of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEqualsReturnsTrueWhenNamesAreEqual() {
            string name1 = "Abcdef";
            string name2 = "Abcdef";

            var _name1 = new StaffFirstName(name1);
            var _name2 = new StaffFirstName(name2);

            Assert.True(_name1.Equals(_name2));
        }

        [Fact]
        public void EnsureEqualsReturnsFalseWhenNamesAreDifferent() {
            string name1 = "Abcdef";
            string name2 = "Ghijkl";

            var _name1 = new StaffFirstName(name1);
            var _name2 = new StaffFirstName(name2);

            Assert.False(_name1.Equals(_name2));
        }
    }
}