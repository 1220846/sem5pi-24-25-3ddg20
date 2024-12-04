using System;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.RoomTypes;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.SurgeryRooms{

    public class SurgeryRoomDto{
        public string Number  {get;set;}
        public RoomTypeDto RoomType {get;set;}
        public short Capacity {get;set;}
        public string CurrentStatus {get;set;}
        public string MaintenanceSlots {get;set;}
        public string AssignedEquipment {get;set;}

        public SurgeryRoomDto() {}

        public SurgeryRoomDto(SurgeryRoom surgeryRoom) {
            this.Number = surgeryRoom.Id.Id;
            this.RoomType = new RoomTypeDto{Code = surgeryRoom.RoomType.Id.Code, Designation = surgeryRoom.RoomType.Designation.Designation, Description = surgeryRoom.RoomType.Description?.Description, IsSurgical = surgeryRoom.RoomType.IsSurgical.IsSurgical };
            this.Capacity = surgeryRoom.RoomCapacity.Capacity;
            this.CurrentStatus = EnumDescription.GetEnumDescription(surgeryRoom.CurrentStatus);
            this.MaintenanceSlots = surgeryRoom.MaintenanceSlots.MaintenanceSlots;
            this.AssignedEquipment = surgeryRoom.AssignedEquipment.AssignedEquipment;
        } 
    }
}