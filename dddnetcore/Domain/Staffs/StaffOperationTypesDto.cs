using System.Collections.Generic;

namespace dddnetcore.Domain.Staffs
{
    public class StaffOperationTypesDto
    {
        public string StaffID {get;set;}
        public string Role {get;set;}
        public string Specialization {get;set;}
        public List<string> OperationTypesName {get;set;}
    }
}