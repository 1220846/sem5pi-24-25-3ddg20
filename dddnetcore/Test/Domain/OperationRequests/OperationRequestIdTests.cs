using System;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationRequests
{
    public class OperationRequestIdTests
    {
        [Fact]
        public void ConstructorShouldCreateOperationRequestIdWhenValidGuidProvided()
        {
            var guid = Guid.NewGuid();
            var operationRequestId = new OperationRequestId(guid.ToString());

            Assert.NotNull(operationRequestId);
            Assert.Equal(guid.ToString(), operationRequestId.AsString());
        }

        [Fact]
        public void AsGuidShouldReturnCorrectGuidWhenCalled()
        {
            var guid = Guid.NewGuid();
            var operationRequestId = new OperationRequestId(guid.ToString());

            Assert.Equal(guid, operationRequestId.AsGuid());
        }

        [Fact]
        public void AsStringShouldReturnCorrectStringWhenCalled()
        {
            var guid = Guid.NewGuid();
            var operationRequestId = new OperationRequestId(guid.ToString());

            Assert.Equal(guid.ToString(), operationRequestId.AsString());
        }
    }
}
