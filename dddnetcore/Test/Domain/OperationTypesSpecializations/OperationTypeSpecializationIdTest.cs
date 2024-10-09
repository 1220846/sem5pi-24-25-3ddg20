using System;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.OperationTypesSpecializations;
using DDDSample1.Domain.Specializations;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationTypesSpecializations
{
    public class OperationTypeSpecializationIdTests
    {
        [Fact]
        public void ConstructorShouldCreateOperationTypeSpecializationIdWithValidIds(){
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var specializationId = new SpecializationId(Guid.NewGuid());

            var operationTypeSpecializationId = new OperationTypeSpecializationId(operationTypeId, specializationId);

            Assert.Equal(operationTypeId, operationTypeSpecializationId.OperationTypeId);
            Assert.Equal(specializationId, operationTypeSpecializationId.SpecializationId);
        }

        [Fact]
        public void AsStringShouldReturnCorrectStringRepresentation(){
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var specializationId = new SpecializationId(Guid.NewGuid());
            var operationTypeSpecializationId = new OperationTypeSpecializationId(operationTypeId, specializationId);

            var stringRepresentation = operationTypeSpecializationId.AsString();

            Assert.Equal($"{operationTypeId.AsGuid()}-{specializationId.AsGuid()}", stringRepresentation);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenIdsAreEqual(){
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var specializationId = new SpecializationId(Guid.NewGuid());
            var id1 = new OperationTypeSpecializationId(operationTypeId, specializationId);
            var id2 = new OperationTypeSpecializationId(operationTypeId, specializationId);

            bool result = id1.Equals(id2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenIdsAreDifferent(){
            var operationTypeId1 = new OperationTypeId(Guid.NewGuid());
            var operationTypeId2 = new OperationTypeId(Guid.NewGuid());
            var specializationId = new SpecializationId(Guid.NewGuid());
            var id1 = new OperationTypeSpecializationId(operationTypeId1, specializationId);
            var id2 = new OperationTypeSpecializationId(operationTypeId2, specializationId);

            bool result = id1.Equals(id2);

            Assert.False(result);
        }
    }
}