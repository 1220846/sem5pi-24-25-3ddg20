using System;
using System.Collections.Generic;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;

namespace DDDSample1.Domain.Staffs {

    public class Staff : Entity<StaffId>, IAggregateRoot {

        public StaffFirstName FirstName {get; private set;}
        public StaffLastName LastName {get; private set;}
        public StaffFullName FullName {get; private set;}
        public StaffContactInformation ContactInformation {get; private set;}
        public LicenseNumber LicenseNumber {get; private set;}
        public ICollection<AvailabilitySlot> AvailabilitySlots {get; private set;}

        public Specialization Specialization {get; private set;}

        private Staff() {}

        public Staff(StaffFirstName firstName,
            StaffLastName lastName,
            StaffFullName fullName,
            StaffContactInformation contactInformation,
            LicenseNumber licenseNumber,
            List<AvailabilitySlot> availabilitySlots,
            Specialization specialization) 
        {
            this.Id = new StaffId(Guid.NewGuid());
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = fullName;
            this.ContactInformation = contactInformation;
            this.AvailabilitySlots = availabilitySlots;
            this.Specialization = specialization;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (Staff)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}