using System;
using DDDSample1.Domain.OperationTypes;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationTypes
{
    public class OperationTypeTests
    {
        [Fact]
        public void ConstructorValidParametersShouldCreateOperationType(){

            var name = new OperationTypeName("ACL Reconstruction Surgery");
            var estimatedDuration = new EstimatedDuration(120);
            var anesthesiaTime = new AnesthesiaTime(30);
            var cleaningTime = new CleaningTime(20);
            var surgeryTime = new SurgeryTime(60);

            var operationType = new OperationType(name, estimatedDuration, anesthesiaTime, cleaningTime, surgeryTime);

            Assert.NotNull(operationType);
            Assert.Equal(name, operationType.Name);
            Assert.Equal(estimatedDuration, operationType.EstimatedDuration);
            Assert.Equal(anesthesiaTime, operationType.AnesthesiaTime);
            Assert.Equal(cleaningTime, operationType.CleaningTime);
            Assert.Equal(surgeryTime, operationType.SurgeryTime);
            Assert.Equal(OperationTypeStatus.ACTIVE, operationType.OperationTypeStatus);
        }

        [Fact]
        public void EqualsSameIdShouldReturnTrue(){
            var name = new OperationTypeName("ACL Reconstruction Surgery");
            var estimatedDuration = new EstimatedDuration(120);
            var anesthesiaTime = new AnesthesiaTime(30);
            var cleaningTime = new CleaningTime(20);
            var surgeryTime = new SurgeryTime(60);

            var operationType1 = new OperationType(name, estimatedDuration, anesthesiaTime, cleaningTime, surgeryTime);

            bool result = operationType1.Equals(operationType1);

            Assert.True(result);
        }

        [Fact]
        public void EqualsWithDifferentIdsShouldReturnFalse(){
            var name = new OperationTypeName("ACL Reconstruction Surgery");
            var estimatedDuration = new EstimatedDuration(120);
            var anesthesiaTime = new AnesthesiaTime(30);
            var cleaningTime = new CleaningTime(20);
            var surgeryTime = new SurgeryTime(60);

            var operationType1 = new OperationType(name, estimatedDuration, anesthesiaTime, cleaningTime, surgeryTime);

            var operationType2 = new OperationType(name, estimatedDuration, anesthesiaTime, cleaningTime, surgeryTime);

            bool result = operationType1.Equals(operationType2);
            Assert.False(result);
        }
        [Fact]
        public void ChangeName_ShouldUpdateName_WhenValidNameProvided()
        {
            // Arrange
            var name = new OperationTypeName("ACL Reconstruction Surgery");
            var estimatedDuration = new EstimatedDuration(120);
            var anesthesiaTime = new AnesthesiaTime(30);
            var cleaningTime = new CleaningTime(20);
            var surgeryTime = new SurgeryTime(60);

            var operationType = new OperationType(name, estimatedDuration, anesthesiaTime, cleaningTime, surgeryTime);

            var newName = new OperationTypeName("New Operation Name"); 

            // Act
            operationType.ChangeName(newName);

            // Assert
            Assert.Equal(newName, operationType.Name);
        }

        [Fact]
        public void ChangeName_ShouldThrowArgumentNullException_WhenNullProvided()
        {
            // Arrange
            var name = new OperationTypeName("ACL Reconstruction Surgery");
            var estimatedDuration = new EstimatedDuration(120);
            var anesthesiaTime = new AnesthesiaTime(30);
            var cleaningTime = new CleaningTime(20);
            var surgeryTime = new SurgeryTime(60);

            var operationType = new OperationType(name, estimatedDuration, anesthesiaTime, cleaningTime, surgeryTime);


            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => operationType.ChangeName(null));
        }

        [Fact]
        public void ChangeEstimatedDuration_ShouldUpdateDuration_WhenValidDurationProvided()
        {
            // Arrange
            var name = new OperationTypeName("ACL Reconstruction Surgery");
            var estimatedDuration = new EstimatedDuration(120);
            var anesthesiaTime = new AnesthesiaTime(30);
            var cleaningTime = new CleaningTime(20);
            var surgeryTime = new SurgeryTime(60);

            var operationType = new OperationType(name, estimatedDuration, anesthesiaTime, cleaningTime, surgeryTime);

            var newDuration = new EstimatedDuration(30);

            // Act
            operationType.ChangeEstimatedDuration(newDuration);

            // Assert
            Assert.Equal(newDuration, operationType.EstimatedDuration);
        }

        [Fact]
        public void ChangeEstimatedDuration_ShouldThrowArgumentNullException_WhenNullProvided()
        {
            // Arrange
            var name = new OperationTypeName("ACL Reconstruction Surgery");
            var estimatedDuration = new EstimatedDuration(120);
            var anesthesiaTime = new AnesthesiaTime(30);
            var cleaningTime = new CleaningTime(20);
            var surgeryTime = new SurgeryTime(60);

            var operationType = new OperationType(name, estimatedDuration, anesthesiaTime, cleaningTime, surgeryTime);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => operationType.ChangeEstimatedDuration(null));
        }
    }
}