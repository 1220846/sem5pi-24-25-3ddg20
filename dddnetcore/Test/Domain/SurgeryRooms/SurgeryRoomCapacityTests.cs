using System;
using Xunit;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Tests.Domain.SurgeryRooms
{
    public class SurgeryRoomCapacityTests
    {
        [Fact]
        public void CreateSurgeryRoomCapacityShouldSetCorrectValue()
        {
            
            short capacity = 10;

            
            var roomCapacity = new SurgeryRoomCapacity(capacity);

            
            Assert.Equal(capacity, roomCapacity.Capacity);
        }

        [Fact]
        public void CreateSurgeryRoomCapacityWithZeroShouldThrowBusinessRuleValidationException()
        {
            
            short capacity = 0;

            
            Assert.Throws<BusinessRuleValidationException>(() => new SurgeryRoomCapacity(capacity));
        }

        [Fact]
        public void CreateSurgeryRoomCapacityWithNegativeValueShouldThrowBusinessRuleValidationException()
        {
            
            short capacity = -5;

            
            Assert.Throws<BusinessRuleValidationException>(() => new SurgeryRoomCapacity(capacity));
        }

        [Fact]
        public void SurgeryRoomCapacityEqualityShouldBeTrueForSameValue()
        {
            
            var capacity1 = new SurgeryRoomCapacity(15);
            var capacity2 = new SurgeryRoomCapacity(15);

            
            Assert.True(capacity1.Equals(capacity2));
            Assert.Equal(capacity1.GetHashCode(), capacity2.GetHashCode());
        }

        [Fact]
        public void SurgeryRoomCapacityEqualityShouldBeFalseForDifferentValue()
        {
            
            var capacity1 = new SurgeryRoomCapacity(10);
            var capacity2 = new SurgeryRoomCapacity(20);

            
            Assert.False(capacity1.Equals(capacity2));
            Assert.NotEqual(capacity1.GetHashCode(), capacity2.GetHashCode());
        }
    }
}