using System.Collections.Generic;

namespace DDDSample1.Domain.OperationTypes
{
    public class EditingOperationTypeDto{
        public string Name{get; set;}
        public int? EstimatedDuration{get; set;}
        public Dictionary<string,int> StaffBySpecializations{get; set;}
    }
}