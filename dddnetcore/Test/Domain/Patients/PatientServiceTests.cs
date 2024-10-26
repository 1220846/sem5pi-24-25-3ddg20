using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.DataAnnotations.Patients;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Emails;
using DDDSample1.Domain.SystemLogs;
using DDDSample1.Domain.Auth;
using dddnetcore.Domain.Patients;

public class PatientServiceTests
{
/*    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPatientRepository> _mockPatientRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IAnonymizedPatientDataRepository> _mockAnonymizedPatientDataRepo;
    private readonly Mock<ISystemLogRepository> _mockSystemLogRepo;
    private readonly Mock<AuthenticationService> _mockAuthService;
    private readonly PatientService _patientService;

    public PatientServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPatientRepo = new Mock<IPatientRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockEmailService = new Mock<IEmailService>();
        _mockAnonymizedPatientDataRepo = new Mock<IAnonymizedPatientDataRepository>();
        _mockSystemLogRepo = new Mock<ISystemLogRepository>();
        _mockAuthService = new Mock<AuthenticationService>();

        _patientService = new PatientService(
            _mockUnitOfWork.Object,
            _mockPatientRepo.Object,
            _mockUserRepo.Object,
            _mockEmailService.Object,
            _mockAnonymizedPatientDataRepo.Object,
            _mockSystemLogRepo.Object,
            _mockAuthService.Object);
    }

    private Patient ExamplePatient()
    {
        var medicalRecordNumber = new MedicalRecordNumber("202410000001");
        var appointmentHistory = new AppointmentHistory("No appointments yet.");
        var dateOfBirth = new DateOfBirth(new DateTime(1990, 1, 1));
        var emergencyContact = new EmergencyContact("Jane Doe");
        var gender = Gender.MALE;
        var medicalConditions = new MedicalConditions("None");
        var contactInformation = new PatientContactInformation(
            new PatientEmail("john.doe@example.com"),
            new PatientPhone("123456789")
        );
        var firstName = new PatientFirstName("John");
        var lastName = new PatientLastName("Doe");
        var fullName = new PatientFullName("John Doe");

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
            null
        );
    }

    [Fact]
    public async Task GetByIdAsyncReturnsPatientDtoWhenPatientExists()
    {
        // Arrange
        var patient = ExamplePatient();
        var medicalRecordNumber = new MedicalRecordNumber("202410000001");
        _mockPatientRepo.Setup(repo => repo.GetByIdAsync(medicalRecordNumber)).ReturnsAsync(patient);

        // Act
        var result = await _patientService.GetByIdAsync(medicalRecordNumber);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(patient.ContactInformation.Email.Email, result.Email);
        Assert.Equal(patient.FirstName.Name, result.FirstName);
    }

    [Fact]
    public async Task GetByIdAsyncThrowsExceptionWhenPatientDoesNotExist()
    {
        // Arrange
        var medicalRecordNumber = new MedicalRecordNumber("202410000001");
        _mockPatientRepo.Setup(repo => repo.GetByIdAsync(medicalRecordNumber)).ReturnsAsync((Patient)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NullReferenceException>(() => _patientService.GetByIdAsync(medicalRecordNumber));
        Assert.Equal($"Not Found Patient with Id: {medicalRecordNumber}", exception.Message);
    }

    [Fact]
    public async Task AddAsyncCreatesNewPatientAndCommits()
    {
        // Arrange
        var dto = new CreatingPatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            FullName = "John Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123456789",
            DateOfBirth = "1990-01-01",
            EmergencyContact = "Jane Doe",
            Gender = "male"
        };

        var patient = ExamplePatient();
        _mockPatientRepo.Setup(repo => repo.LastPatientCreatedAsync()).ReturnsAsync("202410000001");
        _mockPatientRepo.Setup(repo => repo.AddAsync(It.IsAny<Patient>())).ReturnsAsync(patient);

        // Act
        var result = await _patientService.AddAsync(dto);

        // Assert
        Assert.NotNull(result);
        _mockPatientRepo.Verify(repo => repo.AddAsync(It.IsAny<Patient>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task EditPatientAsyncUpdatesPatientAndSendsEmail()
    {
        // Arrange
        var patient = ExamplePatient();
        var editingDto = new EditingPatientDto
        {
            FirstName = "Johnathan",
            Email = "johnathan.doe@example.com"
        };

        _mockPatientRepo.Setup(repo => repo.GetByIdAsync(patient.Id)).ReturnsAsync(patient);
        _mockEmailService.Setup(emailService => emailService.SendEmailAsync(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        var result = await _patientService.EditPatientAsync(patient.Id.Id, editingDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(editingDto.FirstName, result.FirstName);
        _mockPatientRepo.Verify(repo => repo.UpdateAsync(patient), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        _mockEmailService.Verify(emailService => emailService.SendEmailAsync(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task GetPatientsAsyncReturnsListOfPatients()
    {
        // Arrange
        var patients = new List<Patient> { ExamplePatient() };
        _mockPatientRepo.Setup(repo => repo.GetPatientsAsync(null, null, null, null, null, null, null, null, 1, 10)).ReturnsAsync(patients);

        // Act
        var result = await _patientService.GetPatientsAsync(null, null, null, null, null, null, null, null, 1, 10);

        // Assert
        Assert.NotNull(result);
    }
    */
}
