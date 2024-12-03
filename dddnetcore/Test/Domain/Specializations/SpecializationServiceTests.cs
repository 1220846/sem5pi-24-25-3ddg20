using System;
using System.Threading.Tasks;
using dddnetcore.Domain.Specializations;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using Moq;
using Xunit;

namespace DDDSample1.Tests.Domain.Specializations
{
    public class SpecializationServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISpecializationRepository> _specializationRepoMock;

        private readonly SpecializationService _specializationService;

        public SpecializationServiceTests(){
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _specializationRepoMock = new Mock<ISpecializationRepository>();
            _specializationService = new SpecializationService(_unitOfWorkMock.Object,_specializationRepoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsyncExistingIdShouldReturnSpecializationDto()
        {
            var specializationId = new SpecializationId(Guid.NewGuid());
            var specialization = new Specialization(new SpecializationName("Anaesthetist"), new SpecializationCode("123456"), new SpecializationDescription("qwerty"));

            SpecializationId capturedSpecializationId = null;

            _specializationRepoMock.Setup(repo => repo.GetByIdAsync(It.Is<SpecializationId>(id => id == specializationId)))
                .Callback<SpecializationId>(id => capturedSpecializationId = id)
                .ReturnsAsync(specialization);

            var result = await _specializationService.GetByIdAsync(specializationId);

            Assert.NotNull(result);
            Assert.Equal(specialization.Id.AsGuid(), result.Id);
            Assert.Equal("Anaesthetist", result.Name);

            Assert.NotNull(capturedSpecializationId);
            Assert.Equal(specializationId, capturedSpecializationId);

            _specializationRepoMock.Verify(repo => repo.GetByIdAsync(It.Is<SpecializationId>(id => id == specializationId)), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsyncNonExistingIdShouldReturnNull() {
            var specializationId = new SpecializationId(Guid.NewGuid());

            SpecializationId capturedSpecializationId = null;

            _specializationRepoMock.Setup(repo => repo.GetByIdAsync(It.Is<SpecializationId>(id => id == specializationId)))
                .Callback<SpecializationId>(id => capturedSpecializationId = id)
                .ReturnsAsync((Specialization)null);

            var result = await _specializationService.GetByIdAsync(specializationId);

            Assert.Null(result);

            Assert.NotNull(capturedSpecializationId);
            Assert.Equal(specializationId, capturedSpecializationId);

            _specializationRepoMock.Verify(repo => repo.GetByIdAsync(It.Is<SpecializationId>(id => id == specializationId)), Times.Once);
        }

        [Fact]
        public async Task AddAsyncValidDataShouldAddSpecialization()
        {
            var creatingSpecializationDto = new CreatingSpecializationDto { Name = "Anaesthetist" };

            Specialization capturedSpecialization = null;

            _specializationRepoMock.Setup(repo => repo.AddAsync(It.Is<Specialization>(s => s.Name.Name == "Anaesthetist")))
                .Callback<Specialization>(s => capturedSpecialization = s)
                .ReturnsAsync((Specialization capturedSpecialization) => capturedSpecialization);

            var result = await _specializationService.AddAsync(creatingSpecializationDto);

            Assert.NotNull(result);
            Assert.Equal("Anaesthetist", result.Name);

            Assert.NotNull(capturedSpecialization);
            Assert.Equal("Anaesthetist", capturedSpecialization.Name.Name);

            _specializationRepoMock.Verify(repo => repo.AddAsync(It.Is<Specialization>(s => s.Name.Name == "Anaesthetist")), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }
    }
}
