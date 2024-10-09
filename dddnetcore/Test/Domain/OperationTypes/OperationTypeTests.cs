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
    }
}