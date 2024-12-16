using System;
using Xunit;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.AppointmentsStaffs;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.Users;
using System.Collections.Generic;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Domain.RoomTypes;
using dddnetcore.Domain.Specializations;
using dddnetcore.Domain.Staffs;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.Patients;

namespace DDDSample1.Tests.Domain.AppointmentsStaffs
{
    public class AppointmentStaffTests
    {
        private SurgeryRoom CreateExampleSurgeryRoom()
        {
            return new SurgeryRoom(
                new RoomNumber("A123"),
                new RoomType(
                    new RoomTypeCode("ABC12345"), 
                    new RoomTypeDesignation("ICU"), 
                    new RoomTypeDescription("ICU Description"), 
                    new RoomTypeIsSurgical(true)
                ),
                new SurgeryRoomCapacity(10),
                new SurgeryRoomMaintenanceSlots("Mon-Fri: 9am-5pm"),
                new SurgeryRoomAssignedEquipment("Scalpel, Monitor"),
                SurgeryRoomCurrentStatus.AVAILABLE
            );
        }

        private Appointment CreateExampleAppointment()
        {
            var surgeryRoom = CreateExampleSurgeryRoom();
            var operationRequest = new OperationRequest(
                new MedicalRecordNumber("MR12345"),
                new StaffId("O202499999"),
                new OperationTypeId(Guid.NewGuid()),
                new DeadlineDate(DateTime.UtcNow.AddDays(30)),
                Priority.ELECTIVE
            );
            var dateAndTime = new AppointmentDateAndTime(DateTime.Parse("2024-12-01 14:00"));
            return new Appointment(surgeryRoom, operationRequest, dateAndTime);
        }

        private Staff CreateExampleStaff()
        {
            return new Staff(
                "O202499999",
                new StaffFirstName("John"),
                new StaffLastName("Doe"),
                new StaffFullName("John Doe"),
                new StaffContactInformation(
                    new StaffEmail("john@doe.com"), 
                    new StaffPhone("912345678")
                ),
                new LicenseNumber("ABC123"),
                new List<AvailabilitySlot>(),
                new Specialization(
                    new SpecializationName("Unspecified"), 
                    new SpecializationCode("123456"), 
                    new SpecializationDescription("qwerty")
                ),
                new User(
                    new Username("O202499999@sarm.com"), 
                    new Email("john@doe.com"), 
                    Role.TECHNICIAN
                ),
                StaffStatus.ACTIVE
            );
        }

        [Fact]
        public void CreateAppointmentStaff_ShouldInitializeCorrectly()
        {
            // Arrange
            var appointment = CreateExampleAppointment();
            var staff = CreateExampleStaff();

            // Act
            var appointmentStaff = new AppointmentStaff(appointment, staff);

            // Assert
            Assert.NotNull(appointmentStaff);
            Assert.Equal(appointment, appointmentStaff.Appointment);
            Assert.Equal(staff, appointmentStaff.Staff);
            Assert.Equal(
                new AppointmentStaffId(appointment.Id, staff.Id), 
                appointmentStaff.Id
            );
        }

        [Fact]
        public void Equals_ShouldReturnTrue_WhenIdIsEqual()
        {
            // Arrange
            var appointment = CreateExampleAppointment();
            var staff = CreateExampleStaff();
            var appointmentStaff1 = new AppointmentStaff(appointment, staff);
            var appointmentStaff2 = new AppointmentStaff(appointment, staff);

            // Act & Assert
            Assert.True(appointmentStaff1.Equals(appointmentStaff2));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenIdIsDifferent()
        {
            // Arrange
            var appointment1 = CreateExampleAppointment();
            var appointment2 = CreateExampleAppointment();
            var staff1 = CreateExampleStaff();
            var staff2 = CreateExampleStaff();

            var appointmentStaff1 = new AppointmentStaff(appointment1, staff1);
            var appointmentStaff2 = new AppointmentStaff(appointment2, staff2);

            // Act & Assert
            Assert.False(appointmentStaff1.Equals(appointmentStaff2));
        }

        [Fact]
        public void GetHashCode_ShouldBeSame_ForEqualObjects()
        {
            // Arrange
            var appointment = CreateExampleAppointment();
            var staff = CreateExampleStaff();
            var appointmentStaff1 = new AppointmentStaff(appointment, staff);
            var appointmentStaff2 = new AppointmentStaff(appointment, staff);

            // Act & Assert
            Assert.Equal(appointmentStaff1.GetHashCode(), appointmentStaff2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ShouldBeDifferent_ForDifferentObjects()
        {
            // Arrange
            var appointment1 = CreateExampleAppointment();
            var appointment2 = CreateExampleAppointment();
            var staff1 = CreateExampleStaff();
            var staff2 = CreateExampleStaff();

            var appointmentStaff1 = new AppointmentStaff(appointment1, staff1);
            var appointmentStaff2 = new AppointmentStaff(appointment2, staff2);

            // Act & Assert
            Assert.NotEqual(appointmentStaff1.GetHashCode(), appointmentStaff2.GetHashCode());
        }
    }
}