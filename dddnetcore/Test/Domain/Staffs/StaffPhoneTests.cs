using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Staffs
{
    public class StaffPhoneTests
    {
        [Fact]
        public void EnsureValidPhoneIsAllowed() {
            string validPhone = "912345678";

            var phone = new StaffPhone(validPhone);

            Assert.Equal(validPhone, phone);
        }

        [Fact]
        public void EnsureNullPhoneThrowsException() {
            string invalidPhone = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffPhone(invalidPhone));
            Assert.Equal("Phone number of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEmptyPhoneThrowsException() {
            string invalidPhone = "";

            
            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffPhone(invalidPhone));
            Assert.Equal("Phone number of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureBadlyFormattedPhoneThrowsException() {
            string invalidPhone = "thisisaninvalidnumber";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new StaffPhone(invalidPhone));
            Assert.Equal("Phone number format is not valid!", exception.Message);
        }

        [Fact]
        public void EnsureEqualsReturnsTrueWhenPhonesAreEqual() {
            string phone1 = "912345678";
            string phone2 = "912345678";

            var _phone1 = new StaffPhone(phone1);
            var _phone2 = new StaffPhone(phone2);

            Assert.True(_phone1.Equals(_phone2));
        }

        [Fact]
        public void EnsureEqualsReturnsFalseWhenPhonesAreDifferent() {
            string phone1 = "912345678";
            string phone2 = "923456781";

            var _phone1 = new StaffPhone(phone1);
            var _phone2 = new StaffPhone(phone2);

            Assert.False(_phone1.Equals(_phone2));
        }
    }
}