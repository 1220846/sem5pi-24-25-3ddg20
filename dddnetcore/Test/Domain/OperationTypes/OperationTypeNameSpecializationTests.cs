using System;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationTypes
{
    public class OperationTypeNameTests
    {
        [Fact]
        public void ConstructorShouldCreateOperationTypeNameWithValidName(){
            string validName = "ACL Reconstruction Surgery";

            var operationTypeName = new OperationTypeName(validName);

            Assert.Equal(validName, operationTypeName.Name);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenNameIsNull(){
            string invalidName = null;

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new OperationTypeName(invalidName));
            Assert.Equal("The name of operation type cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void ConstructorShouldThrowBusinessRuleValidationExceptionWhenNameIsEmpty(){
            string invalidName = "";

            var exception = Assert.Throws<BusinessRuleValidationException>(() => new OperationTypeName(invalidName));
            Assert.Equal("The name of operation type cannot be null or empty!", exception.Message);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenNamesAreEqual(){
            var operationTypeName1 = new OperationTypeName("ACL Reconstruction Surgery");
            var operationTypeName2 = new OperationTypeName("ACL Reconstruction Surgery");

            bool areEqual = operationTypeName1.Equals(operationTypeName2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenNamesAreDifferent(){
            var operationTypeName1 = new OperationTypeName("ACL Reconstruction Surgery");
            var operationTypeName2 = new OperationTypeName("Knee Replacement Surgery");

            bool areEqual = operationTypeName1.Equals(operationTypeName2);

            Assert.False(areEqual);
        }
    }
}