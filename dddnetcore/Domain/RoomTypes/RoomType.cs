using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.RoomTypes{

    public class RoomType : Entity<RoomTypeCode>, IAggregateRoot
    {
        public RoomTypeDesignation Designation { get;  private set; }
        public RoomTypeDescription Description { get; private set; }
        public RoomTypeIsSurgical IsSurgical {get; private set;}

        public RoomType(){
        }

        public RoomType(RoomTypeCode code,RoomTypeDesignation designation, RoomTypeDescription description, RoomTypeIsSurgical isSurgical){
            this.Id = code;
            this.Designation = designation;
            this.Description = description;
            this.IsSurgical = isSurgical;
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