using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.RoomTypes
{
    public class RoomTypeIsSurgical : IValueObject
    {
        public bool IsSurgical { get; private set; }

        public RoomTypeIsSurgical(bool isSurgical)
        {
            this.IsSurgical = isSurgical;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return IsSurgical == ((RoomTypeIsSurgical)obj).IsSurgical;
        }
        public override int GetHashCode()
        {
            return IsSurgical.GetHashCode();
        }
        public override string ToString()
        {
            return IsSurgical ? "Surgical" : "Non-Surgical";
        }
    }
}
