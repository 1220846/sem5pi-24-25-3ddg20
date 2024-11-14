using System;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRooms
{
    public class SurgeryRoomAssignedEquipment : IValueObject {
    
        public String AssignedEquipment {get; private set;}

        public SurgeryRoomAssignedEquipment(String AssignedEquipment) {
            if (String.IsNullOrEmpty(AssignedEquipment))
                throw new BusinessRuleValidationException("Assigned Equipment cannot be null or empty");
            this.AssignedEquipment = AssignedEquipment;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return AssignedEquipment.Equals(((SurgeryRoomAssignedEquipment)obj).AssignedEquipment);
        }

        
        public override int GetHashCode()
        {
            return AssignedEquipment.GetHashCode();
        }
    }
}