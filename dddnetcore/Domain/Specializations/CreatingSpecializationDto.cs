namespace DDDSample1.Domain.Specializations{
    
    public class CreatingSpecializationDto{
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public CreatingSpecializationDto() {}

        public CreatingSpecializationDto(string name, string code, string Description) {
            this.Name = name;
            this.Code = code;
            this.Description = Description;
        }
    }
}