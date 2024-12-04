using DDDSample1.Domain.RoomTypes;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRooms
{
    public class SurgeryRoom : Entity<RoomNumber>, IAggregateRoot {
    
        public RoomType RoomType {get; private set;}
        public RoomTypeCode RoomTypeCode {get; private set;}
        public SurgeryRoomCapacity RoomCapacity {get; private set;}
        public SurgeryRoomMaintenanceSlots MaintenanceSlots {get; private set;}
        public SurgeryRoomAssignedEquipment AssignedEquipment {get; private set;}
        public SurgeryRoomCurrentStatus CurrentStatus {get; private set;}

        private SurgeryRoom() {}

        public SurgeryRoom(
            RoomNumber roomNumber,
            RoomType roomType,
            SurgeryRoomCapacity roomCapacity,
            SurgeryRoomMaintenanceSlots maintenanceSlots,
            SurgeryRoomAssignedEquipment assignedEquipment,
            SurgeryRoomCurrentStatus currentStatus
        ) {
            this.Id = roomNumber;
            this.RoomType = roomType;
            this.RoomCapacity = roomCapacity;
            this.MaintenanceSlots = maintenanceSlots;
            this.AssignedEquipment = assignedEquipment;
            this.CurrentStatus = currentStatus;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (SurgeryRoom)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}