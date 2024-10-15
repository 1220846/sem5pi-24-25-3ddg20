using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
using Xunit;

namespace DDDSample1.Tests.Domain.Staffs
{
    public class StaffIdTests
    {
        [Fact]
        public void EnsureValidIdIsAllowed() {
            string validId = "D123456789";

            var id = new StaffId(validId);

            Assert.Equal(validId, id.Id);
        }

        [Fact]
        public void EnsureNullIdThrowsException() {
            string invalidId = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffId(invalidId));
            Assert.Equal("Staff id cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEmptyIdThrowsException() {
            string invalidId = "";

            
            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffId(invalidId));
            Assert.Equal("Staff id cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureBadlyFormattedIdThrowsException() {
            string invalidId = "thisisaninvalidid";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffId(invalidId));
            Assert.Equal("Staff id is badly formatted!", exception.Message);
        }
    }
}