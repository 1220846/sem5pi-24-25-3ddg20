using System;
using DDDSample1.Domain.Patients;
using Xunit;

namespace DDDSample1.Tests.Domain.Patients
{
    public class AnonymizedPatientDataIdTests
    {
        [Fact]
        public void ConstructorShouldCreateAnonymizedPatientDataIdWithValidGuid()
        {
            Guid validId = Guid.NewGuid();

            var anonymizedPatientDataId = new AnonymizedPatientDataId(validId);

            Assert.Equal(validId, anonymizedPatientDataId.AsGuid());
            Assert.Equal(validId.ToString(), anonymizedPatientDataId.AsString());
        }

        [Fact]
        public void ConstructorShouldCreateAnonymizedPatientDataIdWithValidString()
        {
            string validId = Guid.NewGuid().ToString();

            var anonymizedPatientDataId = new AnonymizedPatientDataId(validId);

            Guid expectedGuid = Guid.Parse(validId);
            Assert.Equal(expectedGuid, anonymizedPatientDataId.AsGuid());
            Assert.Equal(validId, anonymizedPatientDataId.AsString());
        }

        [Fact]
        public void ConstructorShouldThrowFormatExceptionWhenStringIsInvalid(){

            string invalidIdString = "invalidIdString";

            Assert.Throws<FormatException>(() => new AnonymizedPatientDataId(invalidIdString));
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenIdsAreEqual()
        {
            var anonymizedPatientDataId = Guid.NewGuid();
            var anonymizedPatientDataId1 = new AnonymizedPatientDataId(anonymizedPatientDataId);
            var anonymizedPatientDataId2 = new AnonymizedPatientDataId(anonymizedPatientDataId);

            bool result = anonymizedPatientDataId1.Equals(anonymizedPatientDataId2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenIdsAreDifferent()
        {
            var anonymizedPatientDataId1 = new AnonymizedPatientDataId(Guid.NewGuid());
            var anonymizedPatientDataId2 = new AnonymizedPatientDataId(Guid.NewGuid());

            bool result = anonymizedPatientDataId2.Equals(anonymizedPatientDataId1);

            Assert.False(result);
        }
    }
}