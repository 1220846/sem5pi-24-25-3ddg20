using System;
using DDDSample1.Domain.RoomTypes;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.RoomTypes
{
    public class RoomTypeCodeTests
    {
        [Fact]
        public void ConstructorShouldCreateRoomTypeCodeWithValidString()
        {
            string validCode = "ABC12345";

            var roomTypeCode = new RoomTypeCode(validCode);

            Assert.Equal(validCode, roomTypeCode.Code);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionForNullCode()
        {
            string invalidCode = null;

            var exception = Assert.Throws<NullReferenceException>(() => new RoomTypeCode(invalidCode));
        }

        [Fact]
        public void ConstructorShouldThrowExceptionForEmptyCode()
        {
            string invalidCode = "";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new RoomTypeCode(invalidCode));
            Assert.Contains("Code cannot be null or empty.", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionForCodeWithSpaces()
        {
            string invalidCode = "   ";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new RoomTypeCode(invalidCode));
            Assert.Contains("Code cannot be null or empty.", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionForCodeWithInvalidCharacters()
        {
            string invalidCode = "ABCD!@#$";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new RoomTypeCode(invalidCode));
            Assert.Contains("Code must be exactly 8 characters long", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionForCodeWithLessThan8Characters()
        {
            string invalidCode = "1234567";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new RoomTypeCode(invalidCode));
            Assert.Contains("Code must be exactly 8 characters long", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionForCodeWithMoreThan8Characters()
        {
            string invalidCode = "123456789";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new RoomTypeCode(invalidCode));
            Assert.Contains("Code must be exactly 8 characters long", exception.Message);
        }

        [Fact]
        public void AsStringShouldReturnCorrectCode()
        {
            string validCode = "ABC12345";

            var roomTypeCode = new RoomTypeCode(validCode);

            Assert.Equal(validCode, roomTypeCode.Code);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenCodesAreEqual()
        {
            string code = "ABC12345";
            var code1 = new RoomTypeCode(code);
            var code2 = new RoomTypeCode(code);

            bool areEqual = code1.Equals(code2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenCodesAreDifferent()
        {
            var code1 = new RoomTypeCode("ABC12345");
            var code2 = new RoomTypeCode("DEF67890");

            bool areEqual = code1.Equals(code2);

            Assert.False(areEqual);
        }
    }
}
