using System;
using DDDSample1.Domain.OperationTypeSpecializations;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Specializations;
using Xunit;
using DDDSample1.Domain.OperationTypesSpecializations;
using dddnetcore.Domain.Specializations;

namespace DDDSample1.Tests.Domain.OperationTypesSpecializations
{
    public class OperationTypeSpecializationTests
    {
        [Fact]
        public void ConstructorValidParametersShouldCreateOperationTypeSpecialization()
        {
            var operationType = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var specialization = new Specialization(new SpecializationName("Anaesthetist"), new SpecializationCode("789012"), new SpecializationDescription("asdf"));
            var numberOfStaff = new NumberOfStaff(5);

            var operationTypeSpecialization = new OperationTypeSpecialization(operationType, specialization, numberOfStaff);

            Assert.NotNull(operationTypeSpecialization);
            Assert.Equal(numberOfStaff, operationTypeSpecialization.NumberOfStaff);
            Assert.Equal(operationType.Id, operationTypeSpecialization.Id.OperationTypeId);
            Assert.Equal(specialization.Id, operationTypeSpecialization.Id.SpecializationId);
        }

        [Fact]
        public void ConstructorInvalidNumberOfStaffShouldThrowBusinessRuleValidationException()
        {
            var operationType = new OperationType(new OperationTypeName("Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var specialization = new Specialization(new SpecializationName("Anaesthetist"), new SpecializationCode("123456"), new SpecializationDescription("qwerty"));

            var exception = Assert.Throws<BusinessRuleValidationException>(() => {new OperationTypeSpecialization(operationType, specialization, new NumberOfStaff(-1));});

            Assert.Equal("The number of staff must be positive", exception.Message);
        }

        [Fact]
        public void EqualsSameIdShouldReturnTrue()
        {
            var operationType = new OperationType(new OperationTypeName("Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var specialization = new Specialization(new SpecializationName("Anaesthetist"), new SpecializationCode("123456"), new SpecializationDescription("qwerty"));
            var numberOfStaff = new NumberOfStaff(5);

            var operationTypeSpecialization1 = new OperationTypeSpecialization(operationType, specialization, numberOfStaff);
            var operationTypeSpecialization2 = new OperationTypeSpecialization(operationType, specialization, numberOfStaff);

            bool result = operationTypeSpecialization1.Equals(operationTypeSpecialization2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsWithDifferentIdsShouldReturnFalse()
        {
            var operationType1 = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var operationType2 = new OperationType(new OperationTypeName("Knee Reconstruction Surgery"), new EstimatedDuration(120), new AnesthesiaTime(30), new CleaningTime(20), new SurgeryTime(50));
            var specialization = new Specialization(new SpecializationName("Anaesthetist"), new SpecializationCode("123456"), new SpecializationDescription("qwerty"));
            var numberOfStaff = new NumberOfStaff(5);

            var operationTypeSpecialization1 = new OperationTypeSpecialization(operationType1, specialization, numberOfStaff);
            var operationTypeSpecialization2 = new OperationTypeSpecialization(operationType2, specialization, numberOfStaff);

            bool result = operationTypeSpecialization1.Equals(operationTypeSpecialization2);

            Assert.False(result);
        }

        [Fact]
        public void ChangeNumberOfStaff_ShouldUpdateStaff_WhenValidNumberProvided()
        {
            // Arrange
            var operationType = new OperationType(new OperationTypeName("Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var specialization = new Specialization(new SpecializationName("Anaesthetist"), new SpecializationCode("123456"), new SpecializationDescription("qwerty"));
            var numberOfStaff = new NumberOfStaff(5);
            var operationTypeSpecialization = new OperationTypeSpecialization(operationType, specialization, numberOfStaff);

            var newNumberOfStaff = new NumberOfStaff(10);

            // Act
            operationTypeSpecialization.ChangeNumberOfStaff(newNumberOfStaff);

            // Assert
            Assert.Equal(newNumberOfStaff, operationTypeSpecialization.NumberOfStaff);
        }

        [Fact]
        public void ChangeNumberOfStaff_ShouldThrowArgumentNullException_WhenNullProvided()
        {
            // Arrange
            var operationType = new OperationType(new OperationTypeName("Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var specialization = new Specialization(new SpecializationName("Anaesthetist"), new SpecializationCode("123456"), new SpecializationDescription("qwerty"));
            var numberOfStaff = new NumberOfStaff(5);
            var operationTypeSpecialization = new OperationTypeSpecialization(operationType, specialization, numberOfStaff);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => operationTypeSpecialization.ChangeNumberOfStaff(null));
        }
    }
    
}
