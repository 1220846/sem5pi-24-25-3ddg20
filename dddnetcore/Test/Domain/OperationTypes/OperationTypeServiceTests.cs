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
        public async Task GetByIdAsyncWithExistingIdShouldReturnOperationTypeDto() {
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var operationType = new OperationType( new OperationTypeName("ACL Reconstruction Surgery"),new EstimatedDuration(135),
                                                    new AnesthesiaTime(45), new CleaningTime(30), new SurgeryTime(60));

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

            var creatingOperationTypeDto = new CreatingOperationTypeDto {Name = "ACL Reconstruction Surgery",EstimatedDuration = 60,
                                                                        AnesthesiaTime = 30,CleaningTime = 15, SurgeryTime = 45,
                                                                        StaffSpecializations = new List<CreatingStaffSpecializationDto>
                                                                        { new CreatingStaffSpecializationDto{
                                                                            SpecializationId = specialization.Id.AsGuid().ToString(),
                                                                            NumberOfStaff = 2 }}};

            _specializationRepoMock.Setup(repo => repo.GetByIdAsync(specialization.Id)).ReturnsAsync(specialization);

            OperationType capturedOperationType = null;

            _operationTypeRepoMock.Setup(repo => repo.AddAsync(It.IsAny<OperationType>()))
                .Callback<OperationType>(op => capturedOperationType = op)
                .ReturnsAsync((OperationType op) => { return new OperationType( new OperationTypeName(op.Name.Name), 
                                                                                new EstimatedDuration(op.EstimatedDuration.Minutes), 
                                                                                new AnesthesiaTime(op.AnesthesiaTime.Minutes), 
                                                                                new CleaningTime(op.CleaningTime.Minutes),
                                                                                new SurgeryTime(op.SurgeryTime.Minutes));});

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
    }
}
