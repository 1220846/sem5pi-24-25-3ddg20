using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.SystemLogs;
using DDDSample1.Domain.Shared;
using DDDSample1.DataAnnotations.Patients;
using DDDSample1.DataAnnotations.Staffs;
using DDDSample1.Domain.Auth;
using DDDSample1.Domain.Specializations;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Domain.Users;
using dddnetcore.Domain.Staffs;
using dddnetcore.Domain.OperationRequests.UpdateOperationRequestDto;
using dddnetcore.Domain.OperationRequests;

namespace DDDSample1.Tests
{
    public class OperationRequestServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IOperationRequestRepository> _operationRequestRepoMock;
        private readonly Mock<ISystemLogRepository> _systemLogRepoMock;
        private readonly Mock<IOperationTypeRepository> _operationTypeRepoMock;
        private readonly Mock<IPatientRepository> _patientRepoMock;
        private readonly Mock<IStaffRepository> _staffRepoMock;
        private readonly Mock<AuthenticationService> _authServiceMock;

        private readonly OperationRequestService _service;

        public OperationRequestServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _operationRequestRepoMock = new Mock<IOperationRequestRepository>();
            _systemLogRepoMock = new Mock<ISystemLogRepository>();
            _operationTypeRepoMock = new Mock<IOperationTypeRepository>();
            _patientRepoMock = new Mock<IPatientRepository>();
            _staffRepoMock = new Mock<IStaffRepository>();
            _authServiceMock = new Mock<AuthenticationService>();

            _service = new OperationRequestService(
                _unitOfWorkMock.Object,
                _operationRequestRepoMock.Object,
                _staffRepoMock.Object,
                _operationTypeRepoMock.Object,
                _patientRepoMock.Object,
                _systemLogRepoMock.Object,
                _authServiceMock.Object
            );
        }

        private User ExampleUser1() {
         return new User(new Username("O202499999@sarm.com"), new Email("john@doe.com"), Role.TECHNICIAN);
        }

        private User ExampleUser2() {
         return new User(new Username("O202499998@sarm.com"), new Email("joana@doe.com"), Role.TECHNICIAN);
        }

        private User ExampleUser3() {
         return new User(new Username("iiiillia@doe.com"), new Email("iiiillia@doe.com"), Role.PATIENT);
        }

        private Staff ExampleStaff1() {
        var staffId = "O202499999";
        var staffFirstName = new StaffFirstName("John"); 
        var staffLastName = new StaffLastName("Doe"); 
        var staffFullName = new StaffFullName("John Doe"); 
        var contactInformation = new StaffContactInformation(new StaffEmail("john@doe.com"), new StaffPhone("912345678"));
        var licenseNumber = new LicenseNumber("ABC123");
        var availabilitySlots = new List<AvailabilitySlot>();
        var specialization = new Specialization(new SpecializationName("Unspecified"));
        var user = ExampleUser1();
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

        private Staff ExampleStaff2() {
        var staffId = "O202499999";
        var staffFirstName = new StaffFirstName("John"); 
        var staffLastName = new StaffLastName("Doe"); 
        var staffFullName = new StaffFullName("John Doe"); 
        var contactInformation = new StaffContactInformation(new StaffEmail("john@doe.com"), new StaffPhone("912345678"));
        var licenseNumber = new LicenseNumber("ABC123");
        var availabilitySlots = new List<AvailabilitySlot>();
        var specialization = new Specialization(new SpecializationName("ACL Reconstruction Surgery"));
        var user = ExampleUser2();
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

        private Patient ExamplePatient() {
            var medicalRecordNumber = new MedicalRecordNumber("MRN123456");
            var appointmentHistory = new AppointmentHistory("");
            var dateOfBirth = new DateOfBirth(new DateTime(1990, 1, 1));
            var emergencyContact = new EmergencyContact("917654321");
            var gender = Gender.MALE; 
            var medicalConditions = new MedicalConditions(""); 
            var contactInformation = new PatientContactInformation(new PatientEmail("john.doe@example.com"), new PatientPhone("912345678"));
            var firstName = new PatientFirstName("John");
            var lastName = new PatientLastName("Doe");
            var fullName = new PatientFullName("John Doe");
            var user = ExampleUser3();

            return new Patient(
                medicalRecordNumber,
                appointmentHistory,
                dateOfBirth,
                emergencyContact,
                gender,
                medicalConditions,
                contactInformation,
                firstName,
                lastName,
                fullName,
                user
            );
        }

        


        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsOperationRequestDto()
        {
            var operationRequestId = new OperationRequestId(Guid.NewGuid());
            var operationRequest = new OperationRequest(ExamplePatient().Id,ExampleStaff1().Id,new OperationTypeId(Guid.NewGuid()),new DeadlineDate(DateTime.UtcNow.AddDays(1)), Priority.ELECTIVE);
            _operationRequestRepoMock.Setup(repo => repo.GetByIdAsync(operationRequestId))
                .ReturnsAsync(operationRequest);

            var result = await _service.GetByIdAsync(operationRequestId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            
            var operationRequestId = new OperationRequestId(Guid.NewGuid());

            
            
            _operationRequestRepoMock.Setup(repo => repo.GetByIdAsync(operationRequestId))
                .ReturnsAsync((OperationRequest)null); 



            var result = await _service.GetByIdAsync(operationRequestId);

            Assert.Null(result);
        }


        [Fact]
        public async Task AddOperationRequestAsync_ValidDto_AddsOperationRequest()
        {
            var doctor = ExampleStaff2();
            
            var operationType = new OperationType( new OperationTypeName("ACL Reconstruction Surgery"),new EstimatedDuration(135),new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            
            var patient =ExamplePatient();

            
            operationType.OperationTypeSpecializations.Add(new DDDSample1.Domain.OperationTypesSpecializations.OperationTypeSpecialization(operationType,doctor.Specialization,new DDDSample1.Domain.OperationTypeSpecializations.NumberOfStaff(5)));
            _staffRepoMock.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, doctor.Id.Value, null, null, 1, 10))
                    .ReturnsAsync(new List<Staff> { doctor }); 
            
            
            
            _operationTypeRepoMock.Setup(repo => repo.GetByIdAsync(new OperationTypeId(operationType.Id.AsGuid())))
                .ReturnsAsync(operationType);
            
            
            _patientRepoMock.Setup(repo => repo.GetByIdAsync(new MedicalRecordNumber(patient.Id.AsString())))
                .ReturnsAsync(patient);

            
            
            var OpDto=new CreatingOperationRequestDto{
                DoctorId=doctor.Id.Id,
                OperationTypeId=operationType.Id.Value,
                MedicalRecordNumber=patient.Id.Id,
                Deadline=DateTime.UtcNow.AddDays(1).ToString(),
                Priority=Priority.ELECTIVE.ToString()};

            
            var result = await _service.AddOperationRequestAsync(OpDto);

            
            _operationRequestRepoMock.Verify(repo => repo.AddAsync(It.IsAny<OperationRequest>()), Times.Once);
            
            
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
            
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddOperationRequestAsync_OperationTypeNotFound_ThrowsException()
        {

            var doctor = ExampleStaff2();
            var patient = ExamplePatient();

            _operationTypeRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationTypeId>()))
                .ReturnsAsync((OperationType)null); 

            var OpDto = new CreatingOperationRequestDto
            {
                DoctorId = doctor.Id.Id,
                OperationTypeId = new OperationTypeId(Guid.NewGuid()).ToString(),
                MedicalRecordNumber = patient.Id.Id,
                Deadline = DateTime.UtcNow.AddDays(1).ToString(),
                Priority = Priority.ELECTIVE.ToString()
            };

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddOperationRequestAsync(OpDto));

            _operationRequestRepoMock.Verify(repo => repo.AddAsync(It.IsAny<OperationRequest>()), Times.Never); 

            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never); 

        }

        [Fact]
        public async Task AddOperationRequestAsync_DoctorNotFound_ThrowsException()
        {
            var operationType = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            
            _staffRepoMock.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, It.IsAny<String>(), null, null, 1, 10))
                .ReturnsAsync(new List<Staff>());

            _operationTypeRepoMock.Setup(repo => repo.GetByIdAsync(new OperationTypeId(operationType.Id.AsGuid())))
                .ReturnsAsync(operationType);

            var patient = ExamplePatient();

            var OpDto = new CreatingOperationRequestDto
            {
                DoctorId = new StaffId("D202400010").ToString(), 
                OperationTypeId = operationType.Id.Value,
                MedicalRecordNumber = patient.Id.Id,
                Deadline = DateTime.UtcNow.AddDays(1).ToString(),
                Priority = Priority.ELECTIVE.ToString()
            };

            var exception = await Assert.ThrowsAsync<NullReferenceException>(() => _service.AddOperationRequestAsync(OpDto));

            _operationRequestRepoMock.Verify(repo => repo.AddAsync(It.IsAny<OperationRequest>()), Times.Never); 
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task AddOperationRequestAsync_PatientNotFound_ThrowsException()
        {
            var doctor = ExampleStaff2();
            var operationType = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"), new EstimatedDuration(135), new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));

            _patientRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<MedicalRecordNumber>()))
                .ReturnsAsync((Patient)null);

            _operationTypeRepoMock.Setup(repo => repo.GetByIdAsync(new OperationTypeId(operationType.Id.AsGuid())))
                .ReturnsAsync(operationType);

            var OpDto = new CreatingOperationRequestDto
            {
                DoctorId = doctor.Id.Id,
                OperationTypeId = operationType.Id.Value,
                MedicalRecordNumber = Guid.NewGuid().ToString(),
                Deadline = DateTime.UtcNow.AddDays(1).ToString(),
                Priority = Priority.ELECTIVE.ToString()
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddOperationRequestAsync(OpDto));

            _operationRequestRepoMock.Verify(repo => repo.AddAsync(It.IsAny<OperationRequest>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never); 
        }


        [Fact]
        public async Task AddOperationRequestAsync_InvalidSpecialization_AddsOperationRequest()
        {
            var doctor = ExampleStaff1();
            var operationType = new OperationType( new OperationTypeName("ACL Reconstruction Surgery"),new EstimatedDuration(135),new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var patient =ExamplePatient();

            
            _staffRepoMock.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, doctor.Id.Value, null, null, 1, 10))
                    .ReturnsAsync(new List<Staff> { doctor }); 
            _operationTypeRepoMock.Setup(repo => repo.GetByIdAsync(new OperationTypeId(operationType.Id.AsGuid())))
                .ReturnsAsync(operationType);
            _patientRepoMock.Setup(repo => repo.GetByIdAsync(new MedicalRecordNumber(patient.Id.AsString())))
                .ReturnsAsync(patient);

            var OpDto=new CreatingOperationRequestDto{
                DoctorId=doctor.Id.Id,
                OperationTypeId=operationType.Id.Value,
                MedicalRecordNumber=patient.Id.Id,
                Deadline = DateTime.UtcNow.AddDays(1).ToString(),
                Priority=Priority.ELECTIVE.ToString()};

            await Assert.ThrowsAsync<BusinessRuleValidationException>(async () => 
                await _service.AddOperationRequestAsync(OpDto));
        }

        [Fact]
        public async Task UpdateOperationRequestAsync_ValidDto_UpdatesOperationRequest()
        {
            var operationRequestId = Guid.NewGuid();
            var operationType = new OperationType( new OperationTypeName("ACL Reconstruction Surgery"),new EstimatedDuration(135),new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));
            var operationRequest = new OperationRequest(ExamplePatient().Id,ExampleStaff2().Id,operationType.Id,new DeadlineDate(DateTime.UtcNow.AddDays(1)),Priority.ELECTIVE);
            var updateDto = new UpdateOperationRequestDto {
                Deadline="3000-10-20",
                Priority=Priority.EMERGENCY.ToString()
            };

            _operationRequestRepoMock.Setup(repo => repo.GetByIdAsync(new OperationRequestId(operationRequestId)))
                .ReturnsAsync(operationRequest);

            var result = await _service.UpdateOperationRequestAsync(operationRequestId, updateDto);

            _operationRequestRepoMock.Verify(repo => repo.UpdateAsync(It.IsAny<OperationRequest>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateOperationRequestAsync_InvalidDeadline_ThrowsException()
        {
            // Arrange
            var operationRequestId = Guid.NewGuid();
            var operationRequest = new OperationRequest(ExamplePatient().Id, ExampleStaff2().Id, new OperationTypeId(Guid.NewGuid()), new DeadlineDate(DateTime.UtcNow.AddDays(1)), Priority.ELECTIVE);

            var updateDto = new UpdateOperationRequestDto {
                Deadline = "invalid-date-format",
                Priority = Priority.EMERGENCY.ToString()
            };

            _operationRequestRepoMock.Setup(repo => repo.GetByIdAsync(new OperationRequestId(operationRequestId)))
                .ReturnsAsync(operationRequest);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleValidationException>(async () =>
                await _service.UpdateOperationRequestAsync(operationRequestId, updateDto));
        }
    }
}
