using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
using Xunit;

namespace DDDSample1.Tests.Domain.Staffs
{
    public class LicenseNumberTests
    {
        [Fact]
        public void EnsureValidLicenseNumberIsAllowed() {
            string validLicenseNumber = "12345";

            var licenseNumber = new LicenseNumber(validLicenseNumber);

            Assert.Equal(validLicenseNumber, licenseNumber.Number);
        }

        [Fact]
        public void EnsureNullLicenseNumberThrowsException() {
            string invalidLicenseNumber = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new LicenseNumber(invalidLicenseNumber));
            Assert.Equal("License number of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEmptyLicenseNumberThrowsException() {
            string invalidLicenseNumber = "";

            
            var exception = Assert.Throws<BusinessRuleValidationException>(() => new LicenseNumber(invalidLicenseNumber));
            Assert.Equal("License number of staff cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EnsureEqualsReturnsTrueWhenLicenseNumbersAreEqual() {
            string licenseNumber1 = "12345";
            string licenseNumber2 = "12345";

            var _licenseNumber1 = new LicenseNumber(licenseNumber1);
            var _licenseNumber2 = new LicenseNumber(licenseNumber2);

            Assert.True(_licenseNumber1.Equals(_licenseNumber2));
        }

        [Fact]
        public void EnsureEqualsReturnsFalseWhenLicenseNumbersAreDifferent() {
            string licenseNumber1 = "12345";
            string licenseNumber2 = "23456";

            var _licenseNumber1 = new LicenseNumber(licenseNumber1);
            var _licenseNumber2 = new LicenseNumber(licenseNumber2);

            Assert.False(_licenseNumber1.Equals(_licenseNumber2));
        }
    }
}