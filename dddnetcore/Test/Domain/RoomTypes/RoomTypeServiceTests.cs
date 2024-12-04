using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.RoomTypes;
using DDDSample1.Domain.Shared;
using Moq;
using Xunit;

namespace DDDSample1.Tests.Domain.RoomTypes
{
    public class RoomTypeServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRoomTypeRepository> _roomTypeRepoMock;
        private readonly RoomTypeService _roomTypeService;

        public RoomTypeServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _roomTypeRepoMock = new Mock<IRoomTypeRepository>();
            _roomTypeService = new RoomTypeService(_unitOfWorkMock.Object, _roomTypeRepoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnRoomTypeDtoIfExists()
        {
            // Arrange
            var roomTypeId = new RoomTypeCode("ABC12345");
            var roomType = new RoomType(
                roomTypeId,
                new RoomTypeDesignation("ICU"),
                new RoomTypeDescription("Intensive Care Unit"),
                new RoomTypeIsSurgical(true)
            );

            _roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(It.Is<RoomTypeCode>(id => id == roomTypeId)))
                .ReturnsAsync(roomType);

            // Act
            var result = await _roomTypeService.GetByIdAsync(roomTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ABC12345", result.Code);
            Assert.Equal("ICU", result.Designation);
            Assert.Equal("Intensive Care Unit", result.Description);
            Assert.True(result.IsSurgical);

            _roomTypeRepoMock.Verify(repo => repo.GetByIdAsync(It.Is<RoomTypeCode>(id => id == roomTypeId)), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnNullIfNotExists()
        {
            // Arrange
            var roomTypeId = new RoomTypeCode("ABC12345");

            _roomTypeRepoMock.Setup(repo => repo.GetByIdAsync(It.Is<RoomTypeCode>(id => id == roomTypeId)))
                .ReturnsAsync((RoomType)null);

            // Act
            var result = await _roomTypeService.GetByIdAsync(roomTypeId);

            // Assert
            Assert.Null(result);

            _roomTypeRepoMock.Verify(repo => repo.GetByIdAsync(It.Is<RoomTypeCode>(id => id == roomTypeId)), Times.Once);
        }

        [Fact]
        public async Task AddAsyncShouldAddRoomTypeIfValid()
        {
            var creatingRoomTypeDto = new CreatingRoomTypeDto
            {
                Code = "ABC12345",
                Designation = "ICU",
                Description = "Intensive Care Unit",
                IsSurgical = true
            };

            RoomType capturedRoomType = null;

            _roomTypeRepoMock.Setup(repo => repo.AddAsync(It.IsAny<RoomType>()))
                .Callback<RoomType>(roomType => capturedRoomType = roomType)
                .ReturnsAsync((RoomType roomType) => roomType);

            var result = await _roomTypeService.AddAsync(creatingRoomTypeDto);

            Assert.NotNull(result);
            Assert.Equal("ABC12345", result.Code);
            Assert.Equal("ICU", result.Designation);
            Assert.Equal("Intensive Care Unit", result.Description);
            Assert.True(result.IsSurgical);

            Assert.NotNull(capturedRoomType);
            Assert.Equal("ABC12345", capturedRoomType.Id.AsString());
            Assert.Equal("ICU", capturedRoomType.Designation.Designation);
            Assert.Equal("Intensive Care Unit", capturedRoomType.Description.Description);
            Assert.True(capturedRoomType.IsSurgical.IsSurgical);

            _roomTypeRepoMock.Verify(repo => repo.AddAsync(It.IsAny<RoomType>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsyncShouldThrowExceptionIfDesignationExists()
        {
            var creatingRoomTypeDto = new CreatingRoomTypeDto
            {
                Code = "ABC12345",
                Designation = "ICU",
                Description = "Intensive Care Unit",
                IsSurgical = false
            };

            _roomTypeRepoMock.Setup(repo => repo.AddAsync(It.IsAny<RoomType>()))
                .ThrowsAsync(new Exception("Violation of UNIQUE KEY constraint 'UQ_RoomType_Designation'"));

            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _roomTypeService.AddAsync(creatingRoomTypeDto));

            Assert.Contains("UNIQUE KEY constraint", exception.Message);

            _roomTypeRepoMock.Verify(repo => repo.AddAsync(It.IsAny<RoomType>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnRoomTypeDtosIfExist()
        {
            var roomTypes = new List<RoomType>
            {
                new RoomType(
                    new RoomTypeCode("ABC12345"),
                    new RoomTypeDesignation("ICU"),
                    new RoomTypeDescription("Intensive Care Unit"),
                    new RoomTypeIsSurgical(true)
                ),
                new RoomType(
                    new RoomTypeCode("DEF67890"),
                    new RoomTypeDesignation("Operation Room"),
                    new RoomTypeDescription("Operation Room Description"),
                    new RoomTypeIsSurgical(false)
                )
            };

            _roomTypeRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(roomTypes);

            var result = await _roomTypeService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, dto => dto.Code == "ABC12345" && dto.Designation == "ICU" && dto.IsSurgical);
            Assert.Contains(result, dto => dto.Code == "DEF67890" && dto.Designation == "Operation Room" && !dto.IsSurgical);

            _roomTypeRepoMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}
