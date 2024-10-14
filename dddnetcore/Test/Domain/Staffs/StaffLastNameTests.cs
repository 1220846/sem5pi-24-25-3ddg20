using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
using Xunit;

namespace Domain.Staffs
{
    public class StaffLastNameTests
    {
        [Fact]
        public void EnsureValidNameIsAllowed() {
            string validName = "Abcdef";

            var name = new StaffLastName(validName);

            Assert.Equal(validName, name.Name);
        }

        [Fact]
        public void EnsureNullNameThrowsException() {
            string invalidName = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffLastName(invalidName));
            Assert.Equal("Last name of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEmptyNameThrowsException() {
            string invalidName = "";

            
            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffLastName(invalidName));
            Assert.Equal("Last name of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEqualsReturnsTrueWhenNamesAreEqual() {
            string name1 = "Abcdef";
            string name2 = "Abcdef";

            var _name1 = new StaffLastName(name1);
            var _name2 = new StaffLastName(name2);

            Assert.True(_name1.Equals(_name2));
        }

        [Fact]
        public void EnsureEqualsReturnsFalseWhenNamesAreDifferent() {
            string name1 = "Abcdef";
            string name2 = "Ghijkl";

            var _name1 = new StaffLastName(name1);
            var _name2 = new StaffLastName(name2);

            Assert.False(_name1.Equals(_name2));
        }
    }
}