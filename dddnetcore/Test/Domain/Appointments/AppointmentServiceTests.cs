using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.AppointmentsStaffs;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.RoomTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.SurgeryRooms;
using dddnetcore.Domain.Specializations;
using dddnetcore.Domain.AvailabilitySlots;
using dddnetcore.Domain.Staffs;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.DataAnnotations.Staffs;

namespace DDDSample1.Tests.UnitTests.Application.Services
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IAppointmentRepository> _repo;
        private readonly Mock<ISurgeryRoomRepository> _surgeryRoomRepo;
        private readonly Mock<IOperationRequestRepository> _operationRequestRepo;
        private readonly Mock<IOperationTypeRepository> _operationTypeRepo;
        private readonly Mock<IStaffRepository> _staffRepo;
        private readonly Mock<IAppointmentStaffRepository> _appointmentStaffRepo;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repo = new Mock<IAppointmentRepository>();
            _surgeryRoomRepo = new Mock<ISurgeryRoomRepository>();
            _operationRequestRepo = new Mock<IOperationRequestRepository>();
            _operationTypeRepo = new Mock<IOperationTypeRepository>();
            _staffRepo = new Mock<IStaffRepository>();
            _appointmentStaffRepo = new Mock<IAppointmentStaffRepository>();

            _appointmentService = new AppointmentService(
                _unitOfWork.Object,
                _repo.Object,
                _surgeryRoomRepo.Object,
                _operationRequestRepo.Object,
                _operationTypeRepo.Object,
                _staffRepo.Object,
                _appointmentStaffRepo.Object
            );
        }

        /*[Fact]
        public async Task GetAllAsync_ReturnsAppointmentDtos()
        {
            // Arrange
            var operationType = new OperationType(
                new OperationTypeName("Test Operation Type"),
                new EstimatedDuration(100),
                new AnesthesiaTime(45),
                new CleaningTime(10),
                new SurgeryTime(45)
            );

            var staff = ExampleStaff(); 
            var operationRequest = new OperationRequest(
                new MedicalRecordNumber("202412000001"),
                new StaffId(staff.Id.Id),
                new OperationTypeId(operationType.Id.Value),
                new DeadlineDate(DateTime.UtcNow.AddDays(30)),
                Priority.EMERGENCY
            );

            var surgeryRoom = new SurgeryRoom(
                new RoomNumber("1"),
                new RoomType(
                    new RoomTypeCode("OPR-0001"),
                    new RoomTypeDesignation("Operation Room"),
                    new RoomTypeDescription(null),
                    new RoomTypeIsSurgical(false)
                ),
                new SurgeryRoomCapacity(15),
                new SurgeryRoomMaintenanceSlots("Not defined"),
                new SurgeryRoomAssignedEquipment("Surgical Table"),
                SurgeryRoomCurrentStatus.AVAILABLE
            );

            var appointment = new Appointment(
                surgeryRoom,
                operationRequest,
                new AppointmentDateAndTime(DateTime.UtcNow)
            );

            var appointmentStaff = new AppointmentStaff(appointment, staff);

            var appointmentList = new List<Appointment> { appointment };
            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(appointmentList);

            _operationRequestRepo.Setup(r => r.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(operationRequest);
            _operationTypeRepo.Setup(r => r.GetByIdAsync(It.IsAny<OperationTypeId>())).ReturnsAsync(operationType);
            _staffRepo.Setup(r => r.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);

            _appointmentStaffRepo.Setup(r => r.GetByAppointmentId(It.IsAny<AppointmentId>()))
                .ReturnsAsync(new List<AppointmentStaff> { appointmentStaff });


            // Act
            var result = await _appointmentService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);

            var appointmentDto = result[0];
            Assert.Equal(appointment.Id.AsGuid(), appointmentDto.Id);
            Assert.Equal("1", appointmentDto.SurgeryRoomDto.Number); 
            Assert.Equal("Test Operation Type", appointmentDto.OperationRequestDto.OperationType.Name);
            Assert.Single(appointmentDto.Team); // Verifica se a equipe está retornando corretamente
            Assert.Equal(staff.Id.Id, appointmentDto.Team[0].Id);
            Assert.Equal("John Doe", appointmentDto.Team[0].FullName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsAppointmentDto()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment(
                new SurgeryRoom(
                    new RoomNumber("1"),
                    new RoomType(
                        new RoomTypeCode("OPR-0001"),
                        new RoomTypeDesignation("Operation Room"),
                        new RoomTypeDescription(null),
                        new RoomTypeIsSurgical(false)
                    ),
                    new SurgeryRoomCapacity(15),
                    new SurgeryRoomMaintenanceSlots("Not defined"),
                    new SurgeryRoomAssignedEquipment("Surgical Table"),
                    SurgeryRoomCurrentStatus.AVAILABLE
                ),
                new OperationRequest(
                    new MedicalRecordNumber("202412000001"),
                    new StaffId("O202499999"),
                    new OperationTypeId(Guid.NewGuid()),
                    new DeadlineDate(DateTime.UtcNow.AddDays(30)),
                    Priority.EMERGENCY
                ),
                new AppointmentDateAndTime(DateTime.UtcNow)
            );

            var operationType = new OperationType(
                new OperationTypeName("Test Operation Type"),
                new EstimatedDuration(100),
                new AnesthesiaTime(45),
                new CleaningTime(10),
                new SurgeryTime(45)
            );

            var staff = ExampleStaff();
            var appointmentStaff = new AppointmentStaff(appointment, staff);

            _repo.Setup(r => r.GetByIdAsync(It.IsAny<AppointmentId>())).ReturnsAsync(appointment);
            _operationRequestRepo.Setup(r => r.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(appointment.OperationRequest);
            _operationTypeRepo.Setup(r => r.GetByIdAsync(It.IsAny<OperationTypeId>())).ReturnsAsync(operationType);
            _staffRepo.Setup(r => r.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staff);
            _appointmentStaffRepo.Setup(r => r.GetByIdAsync(It.IsAny<AppointmentStaffId>())).ReturnsAsync(appointmentStaff);

            // Act
            var result = await _appointmentService.GetByIdAsync(new AppointmentId(appointmentId));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(appointment.Id.AsGuid(), result.Id);
            Assert.Equal("1", result.SurgeryRoomDto.Number);

            // Verificar que há no mínimo um elemento na equipe
            Assert.NotEmpty(result.Team);

            Assert.Equal("Test Operation Type", result.OperationRequestDto.OperationType.Name);
            Assert.Equal(staff.Id.Id, result.Team[0].Id);
            Assert.Equal("John Doe", result.Team[0].FullName);
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

        private DDDSample1.Domain.Users.User ExampleUser()
        {
            return new DDDSample1.Domain.Users.User(new Username("O202499999@sarm.com"), new Email("john@doe.com"), Role.TECHNICIAN);
        }*/
    }
}
