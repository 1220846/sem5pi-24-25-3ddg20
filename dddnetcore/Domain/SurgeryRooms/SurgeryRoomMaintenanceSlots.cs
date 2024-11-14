using System;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRooms
{
    public class SurgeryRoomMaintenanceSlots : IValueObject {
    
        public String MaintenanceSlots {get; private set;}

        public SurgeryRoomMaintenanceSlots(String MaintenanceSlots) {
            if (String.IsNullOrEmpty(MaintenanceSlots))
                throw new BusinessRuleValidationException("Maintenance Slots cannot be null or empty");
            this.MaintenanceSlots = MaintenanceSlots;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return MaintenanceSlots.Equals(((SurgeryRoomMaintenanceSlots)obj).MaintenanceSlots);
        }

        
        public override int GetHashCode()
        {
            return MaintenanceSlots.GetHashCode();
        }
    }
}