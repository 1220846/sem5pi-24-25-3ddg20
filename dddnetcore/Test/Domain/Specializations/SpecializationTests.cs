using System;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.OperationTypeSpecializations;
using DDDSample1.Domain.Shared;
using Xunit;
using dddnetcore.Domain.Specializations;

namespace DDDSample1.Tests.Domain.Specializations
{
    public class SpecializationTests
    {
        [Fact]
        public void CreateValidNameShouldCreateSpecialization() {
            var specializationName = new SpecializationName("Anaesthetist");

            var specialization = new Specialization(specializationName, new SpecializationCode("789012"), new SpecializationDescription("asdf"));

            Assert.NotNull(specialization);
            Assert.Equal(specializationName, specialization.Name);
        }

        [Fact]
        public void CreateNullOrEmptyNameShouldThrowBusinessRuleValidationException(){
            string invalidName = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => {new SpecializationName(invalidName);});

            Assert.Equal("The name of specialization cannot be null or empty!", exception.Message);

            invalidName = "";

            exception = Assert.Throws<BusinessRuleValidationException>(() =>{new SpecializationName(invalidName);});

            Assert.Equal("The name of specialization cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EqualsSameIdShouldReturnTrue(){
            var specializationName = new SpecializationName("Anaesthetist");
            var specialization1 = new Specialization(specializationName, new SpecializationCode("789013"), new SpecializationDescription("asdf"));

            bool result = specialization1.Equals(specialization1);

            Assert.True(result);
        }

        [Fact]
        public void EqualsDifferentIdShouldReturnTrue(){
            var specializationName = new SpecializationName("Cardiology");
            var specialization1 = new Specialization(specializationName, new SpecializationCode("789012"), new SpecializationDescription("asdf"));
            var specialization2 = new Specialization(specializationName, new SpecializationCode("789012"), new SpecializationDescription("asdf"));

            bool result = specialization1.Equals(specialization2);

            Assert.False(result);
        }
    }
}