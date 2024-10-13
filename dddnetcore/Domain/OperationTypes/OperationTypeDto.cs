using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.OperationTypes{

    public class OperationTypeDto{
        public Guid Id  {get;set;}
        public string Name {get;set;}
        public int EstimatedDuration {get;set;}
        public int AnesthesiaTime {get;set;}
        public int CleaningTime {get;set;}
        public int SurgeryTime {get;set;}
        public string OperationTypeStatus {get;set;}
        public List<StaffSpecializationDto> StaffSpecializationDtos {get;set;}
    }
}