using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRoom
{
    public class SurgeryRoom : Entity<RoomNumber>, IAggregateRoot {
        
        public RoomNumber RoomNumber {get; private set;}
        public RoomType RoomType {get; private set;}
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
            this.RoomNumber = roomNumber;
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
            return Id.Equals(other.RoomNumber);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}