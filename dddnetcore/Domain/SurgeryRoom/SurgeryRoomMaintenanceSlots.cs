using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRoom
{
    public class SurgeryRoomMaintenanceSlots : IValueObject {
    
        public String MaintenanceSlots {get; private set;}

        public SurgeryRoomMaintenanceSlots(String MaintenanceSlots) {
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