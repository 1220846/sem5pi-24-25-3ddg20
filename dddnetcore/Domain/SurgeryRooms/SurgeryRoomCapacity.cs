using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRooms
{
    public class SurgeryRoomCapacity : IValueObject
    {
        public short Capacity {get; private set;}

        public SurgeryRoomCapacity(short capacity) {
            if (capacity <= 0)
                throw new BusinessRuleValidationException("Surgery room capacity must be positive!");
            this.Capacity = capacity;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;
            return ((SurgeryRoomCapacity) obj).Capacity == Capacity;
        }

        public override int GetHashCode()
        {
            return Capacity.GetHashCode();
        }

    }
}