using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Staffs {

    public class Staff : Entity<StaffId>, IAggregateRoot {
        public StaffFirstName FirstName {get; private set;}
        public StaffLastName LastName {get; private set;}
        public StaffFullName Fullname {get; private set;}
        public StaffContactInformation ContactInformation {get; private set;}
        //TODO: regex license number, availability slots
    }
}