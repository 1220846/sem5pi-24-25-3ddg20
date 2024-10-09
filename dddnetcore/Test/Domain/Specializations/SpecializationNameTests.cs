using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using Xunit;

namespace DDDSample1.Tests.Domain.Specializations
{
    public class SpecializationNameTests{
        [Fact]
        public void ConstructorShouldCreateSpecializationNameWithValidName(){
            var validName = "Anaesthetist";

            var specializationName = new SpecializationName(validName);

            Assert.Equal(validName, specializationName.Name);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionWhenNameIsNull(){
            string invalidName = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationName(invalidName));
            Assert.Equal("The name of specialization cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowExceptionWhenNameIsEmpty(){
            string invalidName = "";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationName(invalidName));
            Assert.Equal("The name of specialization cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenNamesAreEqual(){
            var specializationName1 = new SpecializationName("Anaesthetist");
            var specializationName2 = new SpecializationName("Anaesthetist");

            bool areEqual = specializationName1.Equals(specializationName2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenNamesAreDifferent(){
            var specializationName1 = new SpecializationName("Anaesthetist");
            var specializationName2 = new SpecializationName("Instrumenting Nurse");

            bool areEqual = specializationName1.Equals(specializationName2);

            Assert.False(areEqual);
        }
    }
}