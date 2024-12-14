using System;
using System.Collections.Generic;
using Xunit;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.SurgeryRooms;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.Staffs;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Domain.Specializations;
using dddnetcore.Domain.Specializations;
using dddnetcore.Domain.Staffs;
using DDDSample1.Domain.Users;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.RoomTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.OperationTypes;

public class AppointmentTests
{
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

    private DDDSample1.Domain.Users.User ExampleUser() {
        return new DDDSample1.Domain.Users.User(new Username("O202499999@sarm.com"), new Email("john@doe.com"), Role.TECHNICIAN);
    }

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
    private SurgeryRoom ExampleSurgeryRoom2(){
        var roomNumber = new RoomNumber("A1235");
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

    [Fact]
    public void CreateAppointmentShouldInitializeCorrectly()
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

        var appointment = new Appointment(surgeryRoom, operationRequest, dateAndTime);

        Assert.NotNull(appointment);
        Assert.Equal(surgeryRoom, appointment.SurgeryRoom);
        Assert.Equal(operationRequest, appointment.OperationRequest);
        Assert.Equal(dateAndTime, appointment.DateAndTime);
        Assert.Equal(AppointmentStatus.SCHEDULED, appointment.Status);
        Assert.Empty(appointment.AppointmentStaffs);
    }

    [Fact]
    public void CancelShouldSetStatusToCanceled()
    {
        var appointment = CreateTestAppointment();

        appointment.Cancel();

        Assert.Equal(AppointmentStatus.CANCELED, appointment.Status);
    }

    [Fact]
    public void CompleteShouldSetStatusToCompleted()
    {
        var appointment = CreateTestAppointment();

        appointment.Complete();

        Assert.Equal(AppointmentStatus.COMPLETED, appointment.Status);
    }

    [Fact]
    public void ChangeSurgeryRoomShouldUpdateSurgeryRoom()
    {
        var appointment = CreateTestAppointment();
        var newSurgeryRoom = ExampleSurgeryRoom2();

        appointment.ChangeSurgeryRoom(newSurgeryRoom);

        Assert.Equal(newSurgeryRoom, appointment.SurgeryRoom);
    }

    [Fact]
    public void ChangeSurgeryRoomShouldThrowWhenNewRoomIsNull()
    {
        var appointment = CreateTestAppointment();

        var ex = Assert.Throws<ArgumentNullException>(() => appointment.ChangeSurgeryRoom(null));

        Assert.NotNull(ex);
    }

    [Fact]
    public void ChangeDateAndTimeShouldUpdateDateAndTime()
    {
        var appointment = CreateTestAppointment();
        var newDateAndTime = new AppointmentDateAndTime(DateTime.Parse("2024-12-02 10:00"));

        appointment.ChangeDateAndTime(newDateAndTime);

        Assert.Equal(newDateAndTime, appointment.DateAndTime);
    }

    [Fact]
    public void ChangeDateAndTimeShouldThrowWhenNewDateAndTimeIsNull()
    {
        var appointment = CreateTestAppointment();

        var ex = Assert.Throws<ArgumentNullException>(() => appointment.ChangeDateAndTime(null));

        Assert.NotNull(ex);
    }

    [Fact]
    public void ToStringShouldReturnCorrectFormat()
    {
        var appointment = CreateTestAppointment();

        var result = appointment.ToString();

        Assert.Contains("Appointment: [ID=", result);
        Assert.Contains("RoomNumber=A123", result);
        Assert.Contains("Status=SCHEDULED", result);
        Assert.Contains("DateAndTime=2024-12-01 14:00", result);
    }

    private Appointment CreateTestAppointment()
    {
        var surgeryRoom = ExampleSurgeryRoom();
        var operationRequest = new OperationRequest(
                new MedicalRecordNumber("MR12345"),
                new StaffId(Guid.NewGuid()),
                new OperationTypeId(Guid.NewGuid()),
                new DeadlineDate(DateTime.UtcNow.AddDays(30)),
                Priority.ELECTIVE
            );;
        var dateAndTime = new AppointmentDateAndTime(DateTime.Parse("2024-12-01 14:00"));
        return new Appointment(surgeryRoom, operationRequest, dateAndTime);
    }
}
