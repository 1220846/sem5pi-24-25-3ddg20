using System;
using DDDSample1.Domain.AppointmentsStaffs;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Specializations;
using Xunit;
using dddnetcore.Domain.Specializations;
using DDDSample1.Domain.Staffs;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Domain.Users;
using dddnetcore.Domain.Staffs;
using System.Collections.Generic;
using DDDSample1.Domain.Appointments;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.RoomTypes;

namespace DDDSample1.Tests.Domain.AppointmentsStaffs
{
    public class AppointmentStaffTests
    {
        /*private User ExampleUser() {
            return new User(new Username("O202499999@sarm.com"), new Email("john@doe.com"), Role.TECHNICIAN);
        }
        
        private Staff ExampleStaff() {
            var staffId = "O202499999";
            var staffFirstName = new StaffFirstName("John"); 
            var staffLastName = new StaffLastName("Doe"); 
            var staffFullName = new StaffFullName("John Doe"); 
            var contactInformation = new StaffContactInformation(new StaffEmail("john@doe.com"), new StaffPhone("912345678"));
            var licenseNumber = new LicenseNumber("ABC123");
            var availabilitySlots = new List<AvailabilitySlot>();
            var specialization = new Specialization(new SpecializationName("Unspecified"), new SpecializationCode("123456"), new SpecializationDescription("qwerty"));
            var user = ExampleUser();
            var staffStatus = StaffStatus.ACTIVE;


            return new Staff(staffId,staffFirstName,staffLastName,staffFullName,contactInformation,licenseNumber,
                availabilitySlots,specialization,user,staffStatus);
        }
        [Fact]
        public void ConstructorValidParametersShouldCreateAppointmentStaff()
        {
            var operationType = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var staff = new Specialization(new SpecializationName("Anaesthetist"), new SpecializationCode("789012"), new SpecializationDescription("asdf"));

            var operationTypeSpecialization = new AppointmentStaff(operationType, staff);

            Assert.NotNull(operationTypeSpecialization);
            Assert.Equal(operationType.Id, operationTypeSpecialization.Id.OperationTypeId);
            Assert.Equal(staff.Id, operationTypeSpecialization.Id.SpecializationId);
        }

        [Fact]
        public void EqualsSameIdShouldReturnTrue()
        {
            var operationType = new OperationType(new OperationTypeName("Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var staff = ExampleStaff();

            var operationTypeSpecialization1 = new AppointmentStaff(operationType, staff);
            var operationTypeSpecialization2 = new AppointmentStaff(operationType, staff);

            bool result = operationTypeSpecialization1.Equals(operationTypeSpecialization2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsWithDifferentIdsShouldReturnFalse()
        {
            var operationType1 = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var operationType2 = new OperationType(new OperationTypeName("Knee Reconstruction Surgery"), new EstimatedDuration(120), new AnesthesiaTime(30), new CleaningTime(20), new SurgeryTime(50));
            var staff = ExampleStaff();

            var operationTypeSpecialization1 = new AppointmentStaff(operationType1, staff);
            var operationTypeSpecialization2 = new AppointmentStaff(operationType2, staff);

            bool result = operationTypeSpecialization1.Equals(operationTypeSpecialization2);

            Assert.False(result);
        }*/
    }
    
}
