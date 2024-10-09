namespace DDDSample1.Domain.Specializations{
    
    public class CreatingSpecializationDto{
        public string Name { get; set; }        
        public CreatingSpecializationDto(string name){
            this.Name = name;
        }
    }
}