namespace DDDSample1.Domain.SurgeryRooms{

    public class SurgeryRoomDto{
        public string Number  {get;set;}
        public string Type {get;set;}
        public int Capacity {get;set;}
        public string CurrentStatus {get;set;}
        public string MaintenanceSlots {get;set;}
        public string AssignedEquipment {get;set;}
    }
}