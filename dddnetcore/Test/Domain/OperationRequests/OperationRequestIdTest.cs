using System;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationRequests
{
    public class OperationRequestIdTests
    {
        [Fact]
        public void Constructor_ShouldCreateOperationRequestId_WhenValidGuidProvided()
        {
            var guid = Guid.NewGuid();
            var operationRequestId = new OperationRequestId(guid.ToString());

            Assert.NotNull(operationRequestId);
            Assert.Equal(guid.ToString(), operationRequestId.AsString());
        }

        [Fact]
        public void AsGuid_ShouldReturnCorrectGuid_WhenCalled()
        {
            var guid = Guid.NewGuid();
            var operationRequestId = new OperationRequestId(guid.ToString());

            Assert.Equal(guid, operationRequestId.AsGuid());
        }

        [Fact]
        public void AsString_ShouldReturnCorrectString_WhenCalled()
        {
            var guid = Guid.NewGuid();
            var operationRequestId = new OperationRequestId(guid.ToString());

            Assert.Equal(guid.ToString(), operationRequestId.AsString());
        }
    }
}
