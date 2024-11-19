using System.Threading.Tasks;
using Xunit;
using Moq;
using dddnetcore.Domain.Staffs;
using System.Collections.Generic;
using DDDSample1.DataAnnotations.Staffs;
using DDDSample1.Domain.Shared;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.SystemLogs;
using DDDSample1.Domain.Emails;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Specializations;
using System;
using DDDSample1.Domain.Appointments;

public class StaffServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly StaffService _staffService;
    private readonly Mock<IStaffRepository> _mockRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<ISpecializationRepository> _mockSpecializationRepo;
    private readonly Mock<IAvailabilitySlotRepository> _mockAvailabilitySlotRepo;
    private readonly Mock<ISystemLogRepository> _mockSystemLogRepo;
    private Mock<ISystemLogRepository> _mockRepoSystemLog;
    private readonly Mock<IEmailService> _mockEmailService;
    private Mock<IOperationTypeRepository> _mockOperationTypeRepo;
    private Mock<IAppointmentRepository> _mockAppointmentRepo;

    public StaffServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepo = new Mock<IStaffRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _mockSpecializationRepo = new Mock<ISpecializationRepository>();
        _mockAvailabilitySlotRepo = new Mock<IAvailabilitySlotRepository>();
        _mockSystemLogRepo = new Mock<ISystemLogRepository>();
        _mockEmailService = new Mock<IEmailService>();
        _mockRepoSystemLog = new Mock<ISystemLogRepository>();
        _mockOperationTypeRepo = new Mock<IOperationTypeRepository>();
        _mockAppointmentRepo = new Mock<IAppointmentRepository>();

        _staffService = new StaffService(
            _mockUnitOfWork.Object,
            _mockRepo.Object,
            _mockAvailabilitySlotRepo.Object,
            _mockSpecializationRepo.Object,
            _mockUserRepo.Object,
            _mockSystemLogRepo.Object,
            _mockEmailService.Object,_mockOperationTypeRepo.Object,
            _mockAppointmentRepo.Object);
    }

    private Staff ExampleStaff() {
        var staffId = "O202499999";
        var staffFirstName = new StaffFirstName("John"); 
        var staffLastName = new StaffLastName("Doe"); 
        var staffFullName = new StaffFullName("John Doe"); 
        var contactInformation = new StaffContactInformation(new StaffEmail("john@doe.com"), new StaffPhone("912345678"));
        var licenseNumber = new LicenseNumber("ABC123");
        var availabilitySlots = new List<AvailabilitySlot>();
        var specialization = new Specialization(new SpecializationName("Unspecified"));
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

    private User ExampleUser() {
        return new User(new Username("O202499999@sarm.com"), new Email("john@doe.com"), Role.TECHNICIAN);
    }

    [Fact]
    public async Task GetByIdAsyncExistingStaffIdReturnsStaffDto()
    {
        
        var staffId = "O202499999";
        var exampleStaff = ExampleStaff(); 
        
        
        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, staffId, null, null, 1, 10))
                    .ReturnsAsync(new List<Staff> { exampleStaff }); 

        
        var result = await _staffService.GetByIdAsync(staffId);

        
        Assert.NotNull(result);
        Assert.Equal(staffId, result.Id); 
        Assert.Equal(exampleStaff.FirstName.Name, result.FirstName);
        Assert.Equal(exampleStaff.LastName.Name, result.LastName); 
    }

    [Fact]
    public async Task GetByIdAsyncNonExistingStaffIdReturnsNull()
    {
        
        var staffId = "O999999999";
        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, staffId, null, null, 1, 10))
                    .ReturnsAsync(new List<Staff>());

        
        var result = await _staffService.GetByIdAsync(staffId);

        
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsyncShouldAddNewStaffWhenDataIsValid()
    {
        
        var creatingStaffDto = new CreatingStaffDto
        {
            FirstName = "John",
            LastName = "Doe",
            FullName = "John Doe",
            Email = "john@doe.com",
            LicenseNumber = "ABC123",
            SpecializationId = "9ab5b1b5-2d33-4f3a-a234-9b9b2a8a0e8b",
            UserEmail = "O202499999@sarm.com",
            PhoneNumber = "912345678"
        };

        var specialization = new Specialization(new SpecializationName("Unspecified"));

        var user = ExampleUser();
        _mockUserRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Username>()))
             .ReturnsAsync(user);
        _mockSpecializationRepo
            .Setup(repo => repo.GetByIdAsync(It.IsAny<SpecializationId>()))
            .ReturnsAsync(specialization);

        _mockRepo
            .Setup(repo => repo.AddAsync(It.IsAny<Staff>()))
            .ReturnsAsync((Staff staff) => staff);

        _mockUnitOfWork
            .Setup(uow => uow.CommitAsync())
            .Returns(Task.FromResult(1));

        
        var result = await _staffService.AddAsync(creatingStaffDto);

        
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);

        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Staff>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task AddAsyncShouldThrowArgumentNullExceptionWhenDataIsMissing()
    {
        
        var incompleteDto = new CreatingStaffDto
        {
            FirstName = "John"
            
        };

        
        await Assert.ThrowsAsync<ArgumentNullException>(() => _staffService.AddAsync(incompleteDto));
    }

    [Fact]
    public async Task GetStaffsAsyncShouldReturnListOfStaffDtoWhenStaffsExist()
    {
        
        var staff = new Staff(
            "O202400000",
            new StaffFirstName("John"),
            new StaffLastName("Doe"),
            new StaffFullName("John Doe"),
            new StaffContactInformation(new StaffEmail("john@doe.com"), new StaffPhone("912345678")),
            new LicenseNumber("ABC123"),
            new List<AvailabilitySlot>(),
            new Specialization(new SpecializationName("Unspecified")),
            new User(new Username("O202499999@sarm.com"), new Email("john@doe.com"), Role.TECHNICIAN),
            StaffStatus.ACTIVE
        );

        var staffList = new List<Staff> { staff };

        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, null, null, null, 1, 10))
                 .ReturnsAsync(staffList);

        
        var result = await _staffService.GetStaffsAsync();

        
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("John", result[0].FirstName);
        Assert.Equal("Doe", result[0].LastName);
        Assert.Equal("john@doe.com", result[0].Email);
    }

    [Fact]
    public async Task GetStaffsAsyncShouldReturnEmptyListWhenNoStaffsExist()
    {
        
        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, null, null, null, 1, 10))
                 .ReturnsAsync(new List<Staff>());

        
        var result = await _staffService.GetStaffsAsync();

        
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetStaffsAsyncShouldHandleBusinessRuleValidationException()
    {
        
        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, null, null, null, 1, 10))
                 .ThrowsAsync(new BusinessRuleValidationException(""));

        
        var result = await _staffService.GetStaffsAsync();

        
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetStaffsAsyncShouldReturnFilteredStaffsWhenFilterIsProvided()
    {
        
        var staff = new Staff(
            "O202400001",
            new StaffFirstName("Jane"),
            new StaffLastName("Smith"),
            new StaffFullName("Jane Smith"),
            new StaffContactInformation(new StaffEmail("jane@doe.com"), new StaffPhone("912345679")),
            new LicenseNumber("XYZ456"),
            new List<AvailabilitySlot>(),
            new Specialization(new SpecializationName("Specified")),
            new User(new Username("O202499998@sarm.com"), new Email("jane@doe.com"), Role.TECHNICIAN),
            StaffStatus.ACTIVE
        );

        var staffList = new List<Staff> { staff };

        _mockRepo.Setup(repo => repo.GetStaffsAsync("Jane", null, null, null, null, null, null, null, null, 1, 10))
                 .ReturnsAsync(staffList);

        
        var result = await _staffService.GetStaffsAsync(firstName: "Jane");

        
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Jane", result[0].FirstName);
    }

    [Fact]
    public async Task EditStaffAsyncShouldUpdateStaffWhenDataIsValid()
    {
        
        var staffId = "O202400000";
        var existingStaff = new Staff(
            staffId,
            new StaffFirstName("John"),
            new StaffLastName("Doe"),
            new StaffFullName("John Doe"),
            new StaffContactInformation(new StaffEmail("john@doe.com"), new StaffPhone("912345678")),
            new LicenseNumber("ABC123"),
            new List<AvailabilitySlot>(),
            new Specialization(new SpecializationName("Unspecified")),
            new User(new Username("O202499999@sarm.com"), new Email("john@doe.com"), Role.TECHNICIAN),
            StaffStatus.ACTIVE
        );

        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, staffId, null, null, 1, 10))
                .ReturnsAsync(new List<Staff> { existingStaff });

        var newSpecialization = new Specialization(new SpecializationName("New Spec"));
        _mockSpecializationRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<SpecializationId>()))
                            .ReturnsAsync(newSpecialization);

        _mockAvailabilitySlotRepo.Setup(repo => repo.AddAsync(It.IsAny<AvailabilitySlot>()))
                         .ReturnsAsync((AvailabilitySlot slot) => slot);

        _mockRepoSystemLog.Setup(repo => repo.AddAsync(It.IsAny<SystemLog>()))
                        .ReturnsAsync((SystemLog log) => log);

        _mockUnitOfWork.Setup(uow => uow.CommitAsync())
                    .ReturnsAsync(1);

        var dto = new EditingStaffDto
        {
            SpecializationId = Guid.NewGuid(),
            Email = "john.new@doe.com",
            PhoneNumber = "912345679",
            NewAvailabilitySlotStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            NewAvailabilitySlotEndTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()
        };

        
        var result = await _staffService.EditStaffAsync(staffId, dto);

        
        Assert.NotNull(result);
        Assert.Equal("john.new@doe.com", result.Email);
        Assert.Equal("912345679", result.PhoneNumber);
        Assert.Equal(newSpecialization.Name, existingStaff.Specialization.Name);
    }

    [Fact]
    public async Task EditStaffAsyncShouldThrowExceptionWhenStaffNotFound()
    {
        
        var staffId = "O202400000";
        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, staffId, null, null, 1, 10)).ReturnsAsync(new List<Staff>());

        var dto = new EditingStaffDto();

        
        await Assert.ThrowsAsync<NullReferenceException>(() => _staffService.EditStaffAsync(staffId, dto));
    }

    [Fact]
    public async Task EditStaffAsyncShouldThrowExceptionWhenSpecializationNotFound()
    {
        
        var staffId = "O202400000";
        var existingStaff = new Staff(
            staffId,
            new StaffFirstName("John"),
            new StaffLastName("Doe"),
            new StaffFullName("John Doe"),
            new StaffContactInformation(new StaffEmail("john@doe.com"), new StaffPhone("912345678")),
            new LicenseNumber("ABC123"),
            new List<AvailabilitySlot>(),
            new Specialization(new SpecializationName("Unspecified")),
            new User(new Username("O202499999@sarm.com"), new Email("john@doe.com"), Role.TECHNICIAN),
            StaffStatus.ACTIVE
        );

        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, staffId, null, null, 1, 10)).ReturnsAsync(new List<Staff> { existingStaff });

        var dto = new EditingStaffDto
        {
            SpecializationId = Guid.NewGuid()
        };

        _mockSpecializationRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<SpecializationId>()))
            .ReturnsAsync((Specialization)null); 

        
        await Assert.ThrowsAsync<NullReferenceException>(() => _staffService.EditStaffAsync(staffId, dto));
    }

    [Fact]
    public async Task EditStaffAsyncShouldSendEmailWhenContactInfoChanges()
    {
        
        var staffId = "O202400000";
        var existingStaff = new Staff(
            staffId,
            new StaffFirstName("John"),
            new StaffLastName("Doe"),
            new StaffFullName("John Doe"),
            new StaffContactInformation(new StaffEmail("john@doe.com"), new StaffPhone("912345678")),
            new LicenseNumber("ABC123"),
            new List<AvailabilitySlot>(),
            new Specialization(new SpecializationName("Unspecified")),
            new User(new Username("O202499999@sarm.com"), new Email("john@doe.com"), Role.TECHNICIAN),
            StaffStatus.ACTIVE
        );

        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, staffId, null, null, 1, 10)).ReturnsAsync(new List<Staff> { existingStaff });

        var dto = new EditingStaffDto
        {
            Email = "john.new@doe.com",
            PhoneNumber = "912345679"
        };

        
        await _staffService.EditStaffAsync(staffId, dto);

        
        _mockEmailService.Verify(es => es.SendEmailAsync(
            It.IsAny<List<string>>(),
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task RemoveAsyncShouldDeactivateStaffWhenStaffExists()
    {
        
        var staff = ExampleStaff();  
        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, staff.Id.Id, null, null, 1, 10))
            .ReturnsAsync(new List<Staff> { staff });
        _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Staff>()))
            .ReturnsAsync((Staff staff) => staff);
        _mockUnitOfWork.Setup(uow => uow.CommitAsync()).Returns(Task.FromResult(1));

        
        var result = await _staffService.RemoveAsync(staff.Id.Id);

        
        Assert.NotNull(result);
        Assert.Equal(staff.Id.Id, result.Id);
        Assert.Equal(StaffStatus.DEACTIVATED, staff.Status);  
        _mockRepo.Verify(repo => repo.UpdateAsync(It.Is<Staff>(s => s.Id.Id == staff.Id.Id && s.Status == StaffStatus.DEACTIVATED)), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoveAsyncShouldThrowNullReferenceExceptionWhenStaffNotFound()
    {
        
        _mockRepo.Setup(repo => repo.GetStaffsAsync(null, null, null, null, null, null, "O202499999", null, null, 1, 10))
            .ReturnsAsync(new List<Staff>());

        
        await Assert.ThrowsAsync<NullReferenceException>(() => _staffService.RemoveAsync("O202499999"));
    }

}
