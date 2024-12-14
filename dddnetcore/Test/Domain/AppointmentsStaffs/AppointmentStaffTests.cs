using System;
using Xunit;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.AppointmentsStaffs;
using dddnetcore.Domain.AvailabilitySlots;
using System.Collections.Generic;
using DDDSample1.Domain.Specializations;
using dddnetcore.Domain.Specializations;
using DDDSample1.Domain.Users;
using dddnetcore.Domain.Staffs;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.RoomTypes;

namespace DDDSample1.Tests.Domain.AppointmentsStaffs{
    public class AppointmentStaffTests
    {
        private SurgeryRoom ExampleSurgeryRoom(){
            var roomNumber = new RoomNumber("A123");
                var roomType = new RoomType(new RoomTypeCode("ABC12345"),new RoomTypeDesignation("ICU"), new RoomTypeDescription("ICU Description"),new RoomTypeIsSurgical(true));
                var roomCapacity = new SurgeryRoomCapacity(10);
                var maintenanceSlots = new SurgeryRoomMaintenanceSlots("Mon-Fri: 9am-5pm");
                var assignedEquipment = new SurgeryRoomAssignedEquipment("Scalpel, Monitor");
                var currentStatus = SurgeryRoomCurrentStatus.AVAILABLE;

                var surgeryRoom = new SurgeryRoom(
                    roomNumber, roomType, roomCapacity, maintenanceSlots, assignedEquipment, currentStatus
                );

                return surgeryRoom;
        }
        private Appointment ExampleAppointment()
        {
            var surgeryRoom = ExampleSurgeryRoom();
            var operationRequest = new OperationRequest(
                new MedicalRecordNumber("MR12345"),
                new StaffId(Guid.NewGuid()),
                new OperationTypeId(Guid.NewGuid()),
                new DeadlineDate(DateTime.UtcNow.AddDays(30)),
                Priority.ELECTIVE
            );
            var dateAndTime = new AppointmentDateAndTime(DateTime.Parse("2024-12-01 14:00"));
            return new Appointment(surgeryRoom, operationRequest, dateAndTime);
        }

        private Staff ExampleStaff()
        {
            var staffId = "O202499999";
            var staffFirstName = new StaffFirstName("John"); 
            var staffLastName = new StaffLastName("Doe"); 
            var staffFullName = new StaffFullName("John Doe"); 
            var contactInformation = new StaffContactInformation(new StaffEmail("john@doe.com"), new StaffPhone("912345678"));
            var licenseNumber = new LicenseNumber("ABC123");
            var availabilitySlots = new List<AvailabilitySlot>();
            var specialization = new Specialization(new SpecializationName("Unspecified"), new SpecializationCode("123456"), new SpecializationDescription("qwerty"));
            var user = new DDDSample1.Domain.Users.User(new DDDSample1.Domain.Users.Username("O202499999@sarm.com"), new Email("john@doe.com"), DDDSample1.Domain.Users.Role.TECHNICIAN);
            var staffStatus = StaffStatus.ACTIVE;

            return new Staff(
                staffId,
                staffFirstName,
                staffLastName,
                staffFullName,
                contactInformation,
                licenseNumber,
                availabilitySlots,
                specialization,
                user,
                staffStatus
            );
        }

        [Fact]
        public void CreateAppointmentStaff_ShouldInitializeCorrectly()
        {
            var appointment = ExampleAppointment();
            var staff = ExampleStaff();

            var appointmentStaff = new AppointmentStaff(appointment, staff);

            Assert.NotNull(appointmentStaff);
            Assert.Equal(appointment, appointmentStaff.Appointment);
            Assert.Equal(staff, appointmentStaff.Staff);
            Assert.Equal(new AppointmentStaffId(appointment.Id, staff.Id), appointmentStaff.Id);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenIdIsEqual()
        {
            var appointment = ExampleAppointment();
            var staff = ExampleStaff();
            var appointmentStaff1 = new AppointmentStaff(appointment, staff);
            var appointmentStaff2 = new AppointmentStaff(appointment, staff);

            Assert.True(appointmentStaff1.Equals(appointmentStaff2));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenIdIsDifferent()
        {
            var appointment = ExampleAppointment();
            var staff1 = ExampleStaff();
            var staff2 = new Staff(
                "O202400000",
                new StaffFirstName("John"),
                new StaffLastName("Doe"),
                new StaffFullName("John Doe"),
                new StaffContactInformation(new StaffEmail("john@doe.com"), new StaffPhone("912345678")),
                new LicenseNumber("ABC123"),
                new List<AvailabilitySlot>(),
                new Specialization(new SpecializationName("Unspecified"), new SpecializationCode("123456"), new SpecializationDescription("qwerty")),
                new DDDSample1.Domain.Users.User(new Username("O202499999@sarm.com"), new Email("john@doe.com"), Role.TECHNICIAN),
                StaffStatus.ACTIVE
            ); 
            var appointmentStaff1 = new AppointmentStaff(appointment, staff1);
            var appointmentStaff2 = new AppointmentStaff(appointment, staff2);

            // Act & 
            Assert.False(appointmentStaff1.Equals(appointmentStaff2));
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_ForEqualObjects()
        {
            // Arrange
            var appointment = ExampleAppointment();
            var staff = ExampleStaff();
            var appointmentStaff1 = new AppointmentStaff(appointment, staff);
            var appointmentStaff2 = new AppointmentStaff(appointment, staff);

            // Act & Assert
            Assert.Equal(appointmentStaff1.GetHashCode(), appointmentStaff2.GetHashCode());
        }
    }
}
