

using System;
using System.Threading.Tasks;
using DDDSample1.Controllers;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.OperationTypesSpecializations;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.SystemLogs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DDDSample1.Tests.Controllers.OperationTypes
{
    public class OperationTypesControllerTests{
        /*private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IOperationTypeRepository> _operationTypeRepoMock;
        private readonly Mock<ISpecializationRepository> _specializationRepoMock;
        private readonly Mock<IOperationTypeSpecializationRepository> _operationTypeSpecializationRepoMock;
        private readonly Mock<ISystemLogRepository> _systemLogRepoMock;
        private readonly OperationTypeService _operationTypeService;
        private readonly OperationTypesController _controller;

        public OperationTypesControllerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _operationTypeRepoMock = new Mock<IOperationTypeRepository>();
            _specializationRepoMock = new Mock<ISpecializationRepository>();
            _operationTypeSpecializationRepoMock = new Mock<IOperationTypeSpecializationRepository>();
            _systemLogRepoMock = new Mock<ISystemLogRepository>();

            _operationTypeService = new OperationTypeService(
                _unitOfWorkMock.Object,
                _operationTypeRepoMock.Object,
                _specializationRepoMock.Object,
                _operationTypeSpecializationRepoMock.Object,
                _systemLogRepoMock.Object
            );

            _controller = new OperationTypesController(_operationTypeService);
        }

        public async Task GetGetByIdExistingIdReturnsOkResult(){
            var operationTypeId = Guid.NewGuid();
            
            var operationType = new OperationType(
                new OperationTypeName("Test Operation"),
                new EstimatedDuration(30),
                new AnesthesiaTime(15),
                new CleaningTime(10),
                new SurgeryTime(5)
            );
            
            _operationTypeRepoMock.Setup(repo => repo.GetByIdAsync(new OperationTypeId(operationTypeId)))
                .ReturnsAsync(operationType);

            var result = await _controller.GetGetById(operationTypeId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDto = Assert.IsType<OperationTypeDto>(okResult.Value);

            Assert.Equal(operationTypeId, returnedDto.Id);
            Assert.Equal("Test Operation", returnedDto.Name);
            Assert.Equal(30, returnedDto.EstimatedDuration);
            Assert.Equal(15, returnedDto.AnesthesiaTime);
            Assert.Equal(10, returnedDto.CleaningTime);
            Assert.Equal(5, returnedDto.SurgeryTime);
            Assert.Equal(OperationTypeStatus.ACTIVE.ToString(), returnedDto.OperationTypeStatus);
        }*/
    }
}