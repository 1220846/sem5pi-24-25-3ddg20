using System;

namespace DDDSample1.Domain.Specializations{

    public class SpecializationDto{
        public Guid Id  {get;set;}
        public string Name {get;set;}

        public SpecializationDto() {}

        public SpecializationDto(Specialization specialization) {
            this.Id = specialization.Id.AsGuid();
            this.Name = specialization.Name.Name;
        }
    }
}