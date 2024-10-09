using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using Xunit;

namespace DDDSample1.Tests.Domain.Specializations
{
    public class SpecializationIdTests
    {
        [Fact]
        public void ConstructorShouldCreateSpecializationIdWithValidGuid(){
            var validId = Guid.NewGuid();

            var specializationId = new SpecializationId(validId);

            Assert.Equal(validId, specializationId.AsGuid());
        }

        [Fact]
        public void ConstructorShouldCreateSpecializationIdWithValidString(){

            var validId = Guid.NewGuid().ToString();

            var specializationId = new SpecializationId(validId);

            Assert.Equal(new Guid(validId), specializationId.AsGuid());
        }

        [Fact]
        public void AsStringShouldReturnCorrectStringRepresentation(){
            var specializationId = new SpecializationId(Guid.NewGuid());
            
            var stringRepresentation = specializationId.AsString();

            Assert.Equal(specializationId.AsGuid().ToString(), stringRepresentation);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenIdsAreEqual(){
            var id = Guid.NewGuid();
            var id1 = new SpecializationId(id);
            var id2 = new SpecializationId(id);

            bool areEqual = id1.Equals(id2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenIdsAreDifferent(){
            var id1 = new SpecializationId(Guid.NewGuid());
            var id2 = new SpecializationId(Guid.NewGuid());

            bool areEqual = id1.Equals(id2);

            Assert.False(areEqual);
        }
    }
}