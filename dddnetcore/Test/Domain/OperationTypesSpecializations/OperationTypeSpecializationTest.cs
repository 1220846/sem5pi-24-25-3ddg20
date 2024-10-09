using System;
using DDDSample1.Domain.OperationTypeSpecializations;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Specializations;
using Xunit;
using DDDSample1.Domain.OperationTypesSpecializations;

namespace DDDSample1.Tests.Domain.OperationTypesSpecializations
{
    public class OperationTypeSpecializationTests
    {
        [Fact]
        public void ConstructorValidParametersShouldCreateOperationTypeSpecialization()
        {
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var specializationId = new SpecializationId(Guid.NewGuid());
            var numberOfStaff = new NumberOfStaff(5);

            var operationTypeSpecialization = new OperationTypeSpecialization(operationTypeId, specializationId, numberOfStaff);

            Assert.NotNull(operationTypeSpecialization);
            Assert.Equal(numberOfStaff, operationTypeSpecialization.NumberOfStaff);
            Assert.Equal(operationTypeId, operationTypeSpecialization.Id.OperationTypeId);
            Assert.Equal(specializationId, operationTypeSpecialization.Id.SpecializationId);
        }

        [Fact]
        public void ConstructorInvalidNumberOfStaffShouldThrowBusinessRuleValidationException()
        {
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var specializationId = new SpecializationId(Guid.NewGuid());
            var invalidNumberOfStaff = -1;

            var exception = Assert.Throws<BusinessRuleValidationException>(() =>
            {
                new NumberOfStaff(invalidNumberOfStaff);
            });

            Assert.Equal("The number of staff must be positive", exception.Message);
        }

        [Fact]
        public void EqualsSameIdShouldReturnTrue()
        {
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var specializationId = new SpecializationId(Guid.NewGuid());
            var numberOfStaff = new NumberOfStaff(5);

            var operationTypeSpecialization1 = new OperationTypeSpecialization(operationTypeId, specializationId, numberOfStaff);
            var operationTypeSpecialization2 = new OperationTypeSpecialization(operationTypeId, specializationId, numberOfStaff);

            bool result = operationTypeSpecialization1.Equals(operationTypeSpecialization2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsWithDifferentIdsShouldReturnTrue()
        {
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var specializationId = new SpecializationId(Guid.NewGuid());
            var numberOfStaff = new NumberOfStaff(5);

            var newOperationTypeId = new OperationTypeId(Guid.NewGuid());

            var operationTypeSpecialization1 = new OperationTypeSpecialization(operationTypeId, specializationId, numberOfStaff);
            var operationTypeSpecialization2 = new OperationTypeSpecialization(newOperationTypeId, specializationId, numberOfStaff);

            bool result = operationTypeSpecialization1.Equals(operationTypeSpecialization2);

            Assert.False(result);
        }
    }
}