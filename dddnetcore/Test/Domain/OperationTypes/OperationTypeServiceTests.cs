using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.OperationTypeSpecializations;
using DDDSample1.Domain.OperationTypesSpecializations;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using Moq;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationTypes
{
    public class OperationTypeServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IOperationTypeRepository> _operationTypeRepoMock;
        private readonly Mock<ISpecializationRepository> _specializationRepoMock;
        private readonly Mock<IOperationTypeSpecializationRepository> _operationTypeSpecializationRepoMock;
        private readonly OperationTypeService _operationTypeService;
        public OperationTypeServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _operationTypeRepoMock = new Mock<IOperationTypeRepository>();
            _specializationRepoMock = new Mock<ISpecializationRepository>();
            _operationTypeSpecializationRepoMock = new Mock<IOperationTypeSpecializationRepository>();

            _operationTypeService = new OperationTypeService(_unitOfWorkMock.Object, _operationTypeRepoMock.Object,
                _specializationRepoMock.Object, _operationTypeSpecializationRepoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsyncWithExistingIdShouldReturnOperationTypeDto() 
        {
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var operationType = new OperationType( new OperationTypeName("ACL Reconstruction Surgery"),new EstimatedDuration(135),new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));

            var specialization = new Specialization(new SpecializationName("Anaesthetist"));
            var operationTypeSpecialization = new OperationTypeSpecialization(operationType, specialization, new NumberOfStaff(2));
            operationType.OperationTypeSpecializations.Add(operationTypeSpecialization);
            OperationTypeId capturedOperationTypeId = null;

            _operationTypeRepoMock.Setup(repo => repo.GetByIdAsync(It.Is<OperationTypeId>(id => id == operationTypeId)))
                .Callback<OperationTypeId>(id => capturedOperationTypeId = id)
                .ReturnsAsync(operationType);

            var result = await _operationTypeService.GetByIdAsync(operationTypeId);

            Assert.NotNull(result);
            Assert.Equal(operationType.Id.AsGuid(), result.Id);
            Assert.Equal("ACL Reconstruction Surgery", result.Name);
            Assert.Equal(135, result.EstimatedDuration);
            Assert.Equal(45, result.AnesthesiaTime);
            Assert.Equal(30, result.CleaningTime);
            Assert.Equal(60, result.SurgeryTime);

            Assert.Single(result.StaffSpecializationDtos);
            Assert.Equal(2, result.StaffSpecializationDtos[0].NumberOfStaff);

            Assert.NotNull(capturedOperationTypeId);
            Assert.Equal(operationTypeId, capturedOperationTypeId);

            _operationTypeRepoMock.Verify(repo => repo.GetByIdAsync(It.Is<OperationTypeId>(id => id == operationTypeId)), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsyncWithNonExistingIdShouldThrowBusinessRuleValidationException()
        {
            var operationTypeId = new OperationTypeId(Guid.NewGuid());

            OperationTypeId capturedOperationTypeId = null;

            _operationTypeRepoMock.Setup(repo => repo.GetByIdAsync(It.Is<OperationTypeId>(id => id == operationTypeId)))
                .Callback<OperationTypeId>(id => capturedOperationTypeId = id)
                .ReturnsAsync((OperationType)null);

            var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() =>
                _operationTypeService.GetByIdAsync(operationTypeId));

            Assert.Equal($"Not Found Operation Type with Id: {operationTypeId}", exception.Message);

            Assert.NotNull(capturedOperationTypeId);
            Assert.Equal(operationTypeId, capturedOperationTypeId);

            _operationTypeRepoMock.Verify(repo => repo.GetByIdAsync(It.Is<OperationTypeId>(id => id == operationTypeId)), Times.Once);
        }


        [Fact]
        public async Task AddAsyncWithValidDataShouldAddOperationTypeAndSpecializations()
        {
            var specialization = new Specialization(new SpecializationName("Anaesthetist"));

            var creatingOperationTypeDto = new CreatingOperationTypeDto {Name = "ACL Reconstruction Surgery",EstimatedDuration = 60,AnesthesiaTime = 30,CleaningTime = 15, SurgeryTime = 45,
            StaffSpecializations = new List<CreatingStaffSpecializationDto>{
                    new CreatingStaffSpecializationDto{
                        SpecializationId = specialization.Id.AsGuid().ToString(),
                        NumberOfStaff = 2 
                    }}};

            _specializationRepoMock.Setup(repo => repo.GetByIdAsync(specialization.Id)).ReturnsAsync(specialization);

            OperationType capturedOperationType = null;

            _operationTypeRepoMock.Setup(repo => repo.AddAsync(It.IsAny<OperationType>()))
                .Callback<OperationType>(op => capturedOperationType = op)
                .ReturnsAsync((OperationType op) => new OperationType(new OperationTypeName(op.Name.Name), 
                    new EstimatedDuration(op.EstimatedDuration.Minutes), new AnesthesiaTime(op.AnesthesiaTime.Minutes), new CleaningTime(op.CleaningTime.Minutes),new SurgeryTime(op.SurgeryTime.Minutes)));

            OperationTypeSpecialization capturedOperationTypeSpecialization = null;

            _operationTypeSpecializationRepoMock.Setup(repo => repo.AddAsync(It.IsAny<OperationTypeSpecialization>()))
                .Callback<OperationTypeSpecialization>(spec => capturedOperationTypeSpecialization = spec)
                .ReturnsAsync((OperationTypeSpecialization spec) => spec); 

            var result = await _operationTypeService.AddAsync(creatingOperationTypeDto);

            Assert.NotNull(result);
            Assert.NotNull(capturedOperationType); 

            Assert.Equal(capturedOperationType.Name.Name, result.Name);
            Assert.Equal(capturedOperationType.EstimatedDuration.Minutes, result.EstimatedDuration);
            Assert.Equal(capturedOperationType.AnesthesiaTime.Minutes, result.AnesthesiaTime);
            Assert.Equal(capturedOperationType.CleaningTime.Minutes, result.CleaningTime);
            Assert.Equal(capturedOperationType.SurgeryTime.Minutes, result.SurgeryTime);

            Assert.NotNull(capturedOperationTypeSpecialization);

            Assert.Equal(capturedOperationType.Id.AsGuid(), capturedOperationTypeSpecialization.Id.OperationTypeId.AsGuid());
            Assert.Equal(specialization.Id.AsGuid(), capturedOperationTypeSpecialization.Id.SpecializationId.AsGuid());


            _operationTypeRepoMock.Verify(repo => repo.AddAsync(It.IsAny<OperationType>()), Times.Once);
            _operationTypeSpecializationRepoMock.Verify(repo => repo.AddAsync(It.IsAny<OperationTypeSpecialization>()), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsyncWithNonExistingSpecializationShouldThrowBusinessRuleValidationException() {
            var specializationId = Guid.NewGuid();

            var creatingOperationTypeDto = new CreatingOperationTypeDto{Name = "ACL Reconstruction Surgery",
                                                                    EstimatedDuration = 60,                                                         AnesthesiaTime = 30, CleaningTime = 15, SurgeryTime = 45,
                                                                    StaffSpecializations = new List<CreatingStaffSpecializationDto> {
                                                                    new CreatingStaffSpecializationDto { 
                                                                        SpecializationId = specializationId.ToString(), 
                                                                        NumberOfStaff = 2 }
                                                                    }};

            SpecializationId capturedSpecializationId = null;

            _specializationRepoMock.Setup(repo => repo.GetByIdAsync(It.Is<SpecializationId>(id => id.Equals(new SpecializationId(specializationId))))).Callback<SpecializationId>(id => capturedSpecializationId = id).ReturnsAsync((Specialization)null);

            var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() =>
                _operationTypeService.AddAsync(creatingOperationTypeDto));

            Assert.Equal($"Not Found Specialization with Id: {specializationId}", exception.Message);

            Assert.NotNull(capturedSpecializationId);
            Assert.Equal(new SpecializationId(specializationId), capturedSpecializationId);

            _specializationRepoMock.Verify(repo => repo.GetByIdAsync(It.Is<SpecializationId>(id => id.Equals(new SpecializationId(specializationId)))), Times.Once);
        }

        [Fact]
        public async Task GetOperationTypesAsyncWithValidParametersShouldReturnListOfOperationTypeDto()
        {
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var operationType = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"),
                new EstimatedDuration(135),new AnesthesiaTime(45),new CleaningTime(30),new SurgeryTime(60));

            var operationTypes = new List<OperationType> { operationType };

            _operationTypeRepoMock.Setup(repo => repo.GetOperationTypesAsync(null, null, null))
                .ReturnsAsync(operationTypes);

            var result = await _operationTypeService.GetOperationTypesAsync();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(operationType.Id.AsGuid(), result[0].Id);
            Assert.Equal("ACL Reconstruction Surgery", result[0].Name);
            Assert.Equal(135, result[0].EstimatedDuration);
            Assert.Equal(45, result[0].AnesthesiaTime);
            Assert.Equal(30, result[0].CleaningTime);
            Assert.Equal(60, result[0].SurgeryTime);
            Assert.Equal(OperationTypeStatus.ACTIVE.ToString(), result[0].OperationTypeStatus);
        }

        [Fact]
        public async Task GetOperationTypesAsyncWithNoMatchingOperationTypesShouldReturnEmptyList(){
            _operationTypeRepoMock.Setup(repo => repo.GetOperationTypesAsync(null, null, null))
                .ReturnsAsync(new List<OperationType>());

            var result = await _operationTypeService.GetOperationTypesAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOperationTypesAsyncWithSpecificNameShouldReturnFilteredList() {
            var operationType1 = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"),
                new EstimatedDuration(135),new AnesthesiaTime(45),new CleaningTime(30),new SurgeryTime(60));

            var operationType2 = new OperationType(new OperationTypeName("Knee Reconstruction Surgery"),new EstimatedDuration(120), new AnesthesiaTime(40), new CleaningTime(25), new SurgeryTime(55));

            var operationTypes = new List<OperationType> { operationType1, operationType2 };

            _operationTypeRepoMock.Setup(repo => repo.GetOperationTypesAsync("ACL Reconstruction Surgery", null, null))
                .ReturnsAsync(new List<OperationType> { operationType1 });

            var result = await _operationTypeService.GetOperationTypesAsync(name: "ACL Reconstruction Surgery");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(operationType1.Id.AsGuid(), result[0].Id);
            Assert.Equal("ACL Reconstruction Surgery", result[0].Name);
            Assert.Equal(135, result[0].EstimatedDuration);
            Assert.Equal(45, result[0].AnesthesiaTime);
            Assert.Equal(30, result[0].CleaningTime);
            Assert.Equal(60, result[0].SurgeryTime);
            Assert.Equal(OperationTypeStatus.ACTIVE.ToString(), result[0].OperationTypeStatus);
        }

        [Fact]
        public async Task GetOperationTypesAsyncWithNonExistingNameShouldReturnEmptyList()
        {
            var name = "ACL Reconstruction Surgery";
            
            _operationTypeRepoMock.Setup(repo => repo.GetOperationTypesAsync(name, null, null))
                .ReturnsAsync(new List<OperationType>());

            var result = await _operationTypeService.GetOperationTypesAsync(name: name);

            Assert.NotNull(result);
            Assert.Empty(result); 
        }


        [Fact]
        public async Task GetOperationTypesAsyncWithNonExistingSpecializationShouldReturnFilteredList() {
            var specializationId = Guid.NewGuid();
            var operationType = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"),new EstimatedDuration(135),new AnesthesiaTime(45),new CleaningTime(30),new SurgeryTime(60));

            var operationTypes = new List<OperationType> { operationType };

            _operationTypeRepoMock.Setup(repo => repo.GetOperationTypesAsync(null, specializationId, null))
                .ReturnsAsync(new List<OperationType>());

            var result = await _operationTypeService.GetOperationTypesAsync(specializationId: specializationId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOperationTypesAsyncWithSpecificStatusShouldReturnFilteredList()
        {
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var operationType = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"),new EstimatedDuration(135),new AnesthesiaTime(45),new CleaningTime(30),new SurgeryTime(60));

            var operationTypes = new List<OperationType> { operationType };

            _operationTypeRepoMock.Setup(repo => repo.GetOperationTypesAsync(null, null, "Active"))
                .ReturnsAsync(operationTypes);

            var result = await _operationTypeService.GetOperationTypesAsync(status: "Active");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(operationType.Id.AsGuid(), result[0].Id);
            Assert.Equal("ACL Reconstruction Surgery", result[0].Name);
            Assert.Equal(135, result[0].EstimatedDuration);
            Assert.Equal(45, result[0].AnesthesiaTime);
            Assert.Equal(30, result[0].CleaningTime);
            Assert.Equal(60, result[0].SurgeryTime);
            Assert.Equal(OperationTypeStatus.ACTIVE.ToString(), result[0].OperationTypeStatus);
        }

        [Fact]
        public async Task GetOperationTypesAsyncWithInvalidStatusShouldReturnEmptyList() {

            _operationTypeRepoMock.Setup(repo => repo.GetOperationTypesAsync(null, null, "InvalidStatus"))
                .ReturnsAsync(new List<OperationType>());

            var result = await _operationTypeService.GetOperationTypesAsync(status: "InvalidStatus");

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOperationTypesAsyncWithNullParametersShouldReturnAllTypes() {
            var operationType1 = new OperationType(new OperationTypeName("ACL Reconstruction Surgery"),
                new EstimatedDuration(135),new AnesthesiaTime(45),new CleaningTime(30),new SurgeryTime(60));

            var operationType2 = new OperationType(new OperationTypeName("Knee Reconstruction Surgery"),new EstimatedDuration(120), new AnesthesiaTime(40), new CleaningTime(25), new SurgeryTime(55));

            var operationTypes = new List<OperationType> { operationType1, operationType2 };

            _operationTypeRepoMock.Setup(repo => repo.GetOperationTypesAsync(null, null, null))
                .ReturnsAsync(operationTypes);

            var result = await _operationTypeService.GetOperationTypesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, item => item.Name == "ACL Reconstruction Surgery");
            Assert.Contains(result, item => item.Name == "Knee Reconstruction Surgery");
        }
    }
}
