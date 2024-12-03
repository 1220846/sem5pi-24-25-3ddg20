using System;

namespace DDDSample1.Domain.Specializations{

    public class SpecializationDto{
        public Guid Id  {get;set;}
        public string Name {get;set;}
        public string Code {get;set;}
        public string Description {get;set;}

        public SpecializationDto() {}

        public SpecializationDto(Specialization specialization) {
            this.Id = specialization.Id.AsGuid();
            this.Name = specialization.Name.Name;
            this.Code = specialization.Code.Code;
            this.Description = specialization.Description!.Description!;
        }
    }
}