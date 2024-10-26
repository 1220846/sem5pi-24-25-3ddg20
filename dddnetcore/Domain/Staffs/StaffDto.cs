using System;
using System.Collections.Generic;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Users;

namespace dddnetcore.Domain.Staffs
{
    public class StaffDto
    {
        public string Id {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string FullName {get;set;}
        public string Email {get;set;}
        public string PhoneNumber {get;set;}
        public string LicenseNumber {get;set;}
        public string Status {get;set;}
        public ICollection<AvailabilitySlotDto> AvailabilitySlots {get;set;}
        public SpecializationDto Specialization {get;set;}
        public UserDto User {get;set;}

        public StaffDto() {}

        public StaffDto(Staff staff) {
            this.Id = staff.Id.Id;
            this.FirstName = staff.FirstName.Name;
            this.LastName = staff.LastName.Name;
            this.FullName = staff.FullName.Name;
            this.Email = staff.ContactInformation.Email.Email;
            this.PhoneNumber = staff.ContactInformation.PhoneNumber.PhoneNumber;
            this.LicenseNumber = staff.LicenseNumber.Number;
            this.AvailabilitySlots = [];
            foreach (var availabilitySlot in staff.AvailabilitySlots) {
                AvailabilitySlots.Add(new AvailabilitySlotDto(availabilitySlot));
            }
            this.Specialization = new SpecializationDto(staff.Specialization);
            this.User = new UserDto(staff.User);
            this.Status = staff.Status.ToString();
        }
    }
}