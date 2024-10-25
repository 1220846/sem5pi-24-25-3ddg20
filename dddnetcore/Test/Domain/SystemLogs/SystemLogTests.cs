using System;
using Xunit;
using DDDSample1.Domain.SystemLogs;

namespace DDDSample1.Tests.Domain.SystemLogs
{
    public class SystemLogTests
    {
        [Fact]
        public void ShouldCreateSystemLogWithValidParameters()
        {
            var operation = Operation.INSERT;
            var entity = Entity.OPERATION_TYPE;
            var content = "Inserting a new operation type";
            var entityId = "b8a1f7b0-4c1d-48be-9f16-2e3b072d5a39";

            var systemLog = new SystemLog(operation, entity, content, entityId);

            Assert.NotNull(systemLog);
            Assert.Equal(operation, systemLog.Operation);
            Assert.Equal(entity, systemLog.Entity);
            Assert.Equal(content, systemLog.Content);
            Assert.Equal(entityId, systemLog.EntityId);
            Assert.True(systemLog.Created_At <= DateTime.Now);
            Assert.IsType<SystemLogId>(systemLog.Id);
        }

        [Fact]
        public void ShouldReturnTrueWhenComparingSameSystemLogById()
        {
            var operation = Operation.DELETE;
            var entity = Entity.PATIENT;
            var content = "Deleting patient record";
            var entityId = "b8a1f7b0-4c1d-48be-9f16-2e3b072d5a38";
            var systemLog1 = new SystemLog(operation, entity, content, entityId);

            Assert.True(systemLog1.Equals(systemLog1));
            Assert.Equal(systemLog1.GetHashCode(), systemLog1.GetHashCode());
        }

        [Fact]
        public void ShouldReturnFalseWhenComparingDifferentSystemLogById()
        {
            var systemLog1 = new SystemLog(Operation.UPDATE, Entity.USER, "Updating user", "b8a1f7b0-4c1d-48be-9f16-2e3b072d5a39");
            var systemLog2 = new SystemLog(Operation.UPDATE, Entity.USER, "Updating user", "b8a1f7b0-4c1d-48be-9f16-2e3b072d5a38");

            Assert.False(systemLog1.Equals(systemLog2));
            Assert.NotEqual(systemLog1.GetHashCode(), systemLog2.GetHashCode());
        }
    }
}
