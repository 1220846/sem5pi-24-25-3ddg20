using System.Collections.Generic;

namespace DDDSample1.Domain.OperationTypes{
    
    public class CreatingOperationTypeDto{
        public string Name { get; set; }
        public int EstimatedDuration  { get; set; }
        
        public int AnesthesiaTime { get; set; }

        public int CleaningTime { get; set; }
    
        public int SurgeryTime { get; set; }
        public List<CreatingStaffSpecializationDto> StaffSpecializations { get; set; }
    }
}