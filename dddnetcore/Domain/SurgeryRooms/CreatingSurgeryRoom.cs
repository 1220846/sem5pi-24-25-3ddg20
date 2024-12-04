namespace DDDSample1.Domain.SurgeryRooms{

    public class CreatingSurgeryRoomDto{
        public string Number  {get;set;}
        public string RoomTypeCode {get;set;}
        public short Capacity {get;set;}
        public string CurrentStatus {get;set;}
        public string MaintenanceSlots {get;set;}
        public string AssignedEquipment {get;set;}
    }
}