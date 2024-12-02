using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.RoomTypes;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace DDDSample1.Tests.Domain.RoomTypes
{
    public class RoomTypeServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRoomTypeRepository> _roomTypeRepoMock;

        private readonly RoomTypeService _roomTypeService;

        public RoomTypeServiceTests(){
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _roomTypeRepoMock = new Mock<IRoomTypeRepository>();
            _roomTypeService = new RoomTypeService(_unitOfWorkMock.Object,_roomTypeRepoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsyncExistingIdShouldReturnRoomTypeDto()
        {
            var roomTypeId = new RoomTypeId(Guid.NewGuid());
            var roomType = new RoomType(new RoomTypeName("ICU"));

            RoomTypeId capturedRoomTypeId = null;

            _roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(It.Is<RoomTypeId>(id => id == roomTypeId)))
                .Callback<RoomTypeId>(id => capturedRoomTypeId = id)
                .ReturnsAsync(roomType);

            var result = await _roomTypeService.GetByIdAsync(roomTypeId);

            Assert.NotNull(result);
            Assert.Equal(roomType.Id.AsGuid(), result.Id);
            Assert.Equal("ICU", result.Name);

            Assert.NotNull(capturedRoomTypeId);
            Assert.Equal(roomTypeId, capturedRoomTypeId);

            _roomTypeRepoMock.Verify(repo => repo.GetByIdAsync(It.Is<RoomTypeId>(id => id == roomTypeId)), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsyncNonExistingIdShouldReturnNull() {
            var roomTypeId = new RoomTypeId(Guid.NewGuid());

            RoomTypeId capturedRoomTypeId = null;

            _roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(It.Is<RoomTypeId>(id => id == roomTypeId)))
                .Callback<RoomTypeId>(id => capturedRoomTypeId = id)
                .ReturnsAsync((RoomType)null);

            var result = await _roomTypeService.GetByIdAsync(roomTypeId);

            Assert.Null(result);

            Assert.NotNull(capturedRoomTypeId);
            Assert.Equal(roomTypeId, capturedRoomTypeId);

            _roomTypeRepoMock.Verify(repo => repo.GetByIdAsync(It.Is<RoomTypeId>(id => id == roomTypeId)), Times.Once);
        }

        [Fact]
        public async Task AddAsyncValidDataShouldAddRoomType()
        {
            var creatingRoomTypeDto = new CreatingRoomTypeDto { Name = "ICU" };

            RoomType capturedRoomType = null;

            _roomTypeRepoMock.Setup(repo => repo.AddAsync(It.Is<RoomType>(s => s.Name.Name == "ICU")))
                .Callback<RoomType>(s => capturedRoomType = s)
                .ReturnsAsync((RoomType capturedRoomType) => capturedRoomType);

            var result = await _roomTypeService.AddAsync(creatingRoomTypeDto);

            Assert.NotNull(result);
            Assert.Equal("ICU", result.Name);

            Assert.NotNull(capturedRoomType);
            Assert.Equal("ICU", capturedRoomType.Name.Name);

            _roomTypeRepoMock.Verify(repo => repo.AddAsync(It.Is<RoomType>(s => s.Name.Name == "ICU")), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnListOfRoomTypeDtos()
        {
            var roomTypes = new List<RoomType>
            {
                new RoomType(new RoomTypeName("ICU")),
                new RoomType(new RoomTypeName("General")),
            };

            _roomTypeRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(roomTypes);

            var result = await _roomTypeService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, dto => dto.Name == "ICU");
            Assert.Contains(result, dto => dto.Name == "General");

            _roomTypeRepoMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
        [Fact]
        public async Task AddAsyncExistingRoomTypeNameShouldThrowException()
        {
            var creatingRoomTypeDto = new CreatingRoomTypeDto { Name = "ICU" };

            _roomTypeRepoMock.Setup(repo => repo.AddAsync(It.IsAny<RoomType>()))
                .ThrowsAsync(new Exception("Violation of UNIQUE KEY constraint 'UQ_RoomType_Name'."));

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _roomTypeService.AddAsync(creatingRoomTypeDto));

            Assert.Contains("UNIQUE KEY constraint", exception.Message);

            _roomTypeRepoMock.Verify(repo => repo.AddAsync(It.Is<RoomType>(s => s.Name.Name == "ICU")), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);
        }

    }

}
