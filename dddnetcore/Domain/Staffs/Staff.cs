using System;
using System.Collections.Generic;
using dddnetcore.Domain.AvailabilitySlots;
using dddnetcore.Domain.Staffs;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Staffs {

    public class Staff : Entity<StaffId>, IAggregateRoot {

        public StaffFirstName FirstName {get; private set;}
        public StaffLastName LastName {get; private set;}
        public StaffFullName FullName {get; private set;}
        public StaffContactInformation ContactInformation {get; private set;}
        public LicenseNumber LicenseNumber {get; private set;}
        public ICollection<AvailabilitySlot> AvailabilitySlots {get; private set;}
        public StaffStatus Status {get; private set;}
        public Specialization Specialization {get; private set;}
        public User User {get; private set;}
        public Username Username {get; private set;}

        private Staff() {}

        public Staff(
            string id,
            StaffFirstName firstName,
            StaffLastName lastName,
            StaffFullName fullName,
            StaffContactInformation contactInformation,
            LicenseNumber licenseNumber,
            List<AvailabilitySlot> availabilitySlots,
            Specialization specialization,
            User user,
            StaffStatus staffStatus) 
        {
            this.Id = new StaffId(id);
            this.FirstName = firstName;
            this.LastName = lastName;
            this.FullName = fullName;
            this.ContactInformation = contactInformation;
            this.AvailabilitySlots = availabilitySlots;
            this.LicenseNumber = licenseNumber;
            this.Specialization = specialization;
            this.User = user;
        }

        public void ChangeSpecialization(Specialization newSpecialization) {
            ArgumentNullException.ThrowIfNull(newSpecialization);
            this.Specialization = newSpecialization;
        }

        public void AddAvailabilitySlot(AvailabilitySlot newAvailabilitySlot) {
            ArgumentNullException.ThrowIfNull(newAvailabilitySlot);
            this.AvailabilitySlots.Add(newAvailabilitySlot);
        }

        public void Deactivate() {
            this.Status = StaffStatus.DEACTIVATED;
        }

        public void RemoveAvailabilitySlot(AvailabilitySlot oldAvailabilitySlot) {
            this.AvailabilitySlots.Remove(oldAvailabilitySlot);
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