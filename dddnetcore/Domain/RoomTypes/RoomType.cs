using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.RoomTypes{

    public class RoomType : Entity<RoomTypeId>, IAggregateRoot
    {
        public RoomTypeName Name { get;  private set; }

        public RoomType(RoomTypeName name){
            this.Id = new RoomTypeId(Guid.NewGuid());
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (RoomType)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}