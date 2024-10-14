using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
using Xunit;

namespace Domain.Staffs
{
    public class StaffFullNameTests
    {
        [Fact]
        public void EnsureValidNameIsAllowed() {
            string validName = "Abcdef Mnopqr";

            var name = new StaffFullName(validName);

            Assert.Equal(validName, name.Name);
        }

        [Fact]
        public void EnsureNullNameThrowsException() {
            string invalidName = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffFullName(invalidName));
            Assert.Equal("Full name of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEmptyNameThrowsException() {
            string invalidName = "";

            
            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffFullName(invalidName));
            Assert.Equal("Full name of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEqualsReturnsTrueWhenNamesAreEqual() {
            string name1 = "Abcdef Mnopqr";
            string name2 = "Abcdef Mnopqr";

            var _name1 = new StaffFullName(name1);
            var _name2 = new StaffFullName(name2);

            Assert.True(_name1.Equals(_name2));
        }

        [Fact]
        public void EnsureEqualsReturnsFalseWhenNamesAreDifferent() {
            string name1 = "Abcdef Mnopqr";
            string name2 = "Ghijkl Stuvwx";

            var _name1 = new StaffFullName(name1);
            var _name2 = new StaffFullName(name2);

            Assert.False(_name1.Equals(_name2));
        }
    }
}