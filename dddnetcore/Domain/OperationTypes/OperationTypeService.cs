using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationTypesSpecializations;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.OperationTypeSpecializations;
using System.Collections.Generic;
using System;
using System.Linq;
using DDDSample1.Domain.SystemLogs;
using Microsoft.IdentityModel.Tokens;

namespace DDDSample1.Domain.OperationTypes
{
    public class OperationTypeService{

        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IOperationTypeRepository _repo;

        private readonly ISpecializationRepository _specializationRepo;

        private readonly IOperationTypeSpecializationRepository _operationTypeSpecializationRepo;

        private readonly ISystemLogRepository _systemLogRepository;

        public OperationTypeService(IUnitOfWork unitOfWork, IOperationTypeRepository repo,ISpecializationRepository specializationRepo,IOperationTypeSpecializationRepository operationTypeSpecializationRepo, ISystemLogRepository systemLogRepository){

            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._specializationRepo = specializationRepo; 
            this._operationTypeSpecializationRepo = operationTypeSpecializationRepo;
            this._systemLogRepository = systemLogRepository;
        }

        public async Task<OperationTypeDto> GetByIdAsync(OperationTypeId id)
        {
            var operationType = await this._repo.GetByIdAsync(id);
            
            if(operationType == null)
                return null;

            return new OperationTypeDto{Id = operationType.Id.AsGuid(), Name = operationType.Name.Name, EstimatedDuration = operationType.EstimatedDuration.Minutes, AnesthesiaTime = operationType.AnesthesiaTime.Minutes,
            CleaningTime = operationType.CleaningTime.Minutes, SurgeryTime = operationType.SurgeryTime.Minutes, OperationTypeStatus = operationType.OperationTypeStatus.ToString(),StaffSpecializationDtos = operationType.OperationTypeSpecializations.Select(ots => new StaffSpecializationDto {
            SpecializationId = ots.Specialization.Id.AsGuid().ToString(),SpecializationName = ots.Specialization.Name.Name,NumberOfStaff = ots.NumberOfStaff.Number }).ToList()};
        }

        public async Task<OperationTypeDto> AddAsync(CreatingOperationTypeDto creatingOperationTypeDto){      

            var operationType = new OperationType(new OperationTypeName(creatingOperationTypeDto.Name),new EstimatedDuration(creatingOperationTypeDto.EstimatedDuration), new AnesthesiaTime(creatingOperationTypeDto.AnesthesiaTime),new CleaningTime(creatingOperationTypeDto.CleaningTime),new SurgeryTime(creatingOperationTypeDto.SurgeryTime));
            
            await this._repo.AddAsync(operationType);

            foreach(var staffSpecialization in creatingOperationTypeDto.StaffSpecializations){

                var specialization = await this._specializationRepo.GetByIdAsync(new SpecializationId(staffSpecialization.SpecializationId));

                if (specialization == null) {
                    throw new NullReferenceException($"Not Found Specialization with Id: {staffSpecialization.SpecializationId}");
                }

                var operationTypeSpecialization = new OperationTypeSpecialization(operationType, specialization,
                                                    new NumberOfStaff(staffSpecialization.NumberOfStaff));

                await this._operationTypeSpecializationRepo.AddAsync(operationTypeSpecialization);
            }

            await this._systemLogRepository.AddAsync(new SystemLog(Operation.INSERT,Entity.OPERATION_TYPE,operationType.ToString(),operationType.Id.AsString()));

            await this._unitOfWork.CommitAsync();

            await this._operationTypeSpecializationRepo.GetAllAsync();

            return new OperationTypeDto { Id = operationType.Id.AsGuid(), Name = operationType.Name.Name, EstimatedDuration = operationType.EstimatedDuration.Minutes, AnesthesiaTime = operationType.AnesthesiaTime.Minutes,CleaningTime = operationType.CleaningTime.Minutes, SurgeryTime = operationType.SurgeryTime.Minutes,OperationTypeStatus = operationType.OperationTypeStatus.ToString(),StaffSpecializationDtos = operationType.OperationTypeSpecializations.Select(ots => new StaffSpecializationDto {
            SpecializationId = ots.Specialization.Id.AsGuid().ToString(),SpecializationName = ots.Specialization.Name.Name,NumberOfStaff = ots.NumberOfStaff.Number }).ToList()};
        }

        public async Task<List<OperationTypeDto>> GetOperationTypesAsync(string name = null, Guid? specializationId = null, string status = null) {
            var operationTypes = await this._repo.GetOperationTypesAsync(name,specializationId,status);
            
            List<OperationTypeDto> operationTypesDto = operationTypes.ConvertAll<OperationTypeDto>(operationType => new OperationTypeDto {Id = operationType.Id.AsGuid(), Name = operationType.Name.Name, EstimatedDuration = operationType.EstimatedDuration.Minutes, AnesthesiaTime = operationType.AnesthesiaTime.Minutes,CleaningTime = operationType.CleaningTime.Minutes, SurgeryTime = operationType.SurgeryTime.Minutes,OperationTypeStatus = operationType.OperationTypeStatus.ToString(),
            StaffSpecializationDtos = operationType.OperationTypeSpecializations.Select(ots => new StaffSpecializationDto {
            SpecializationId = ots.Specialization.Id.AsGuid().ToString(),SpecializationName = ots.Specialization.Name.Name,NumberOfStaff = ots.NumberOfStaff.Number }).ToList()});

            return operationTypesDto;
        }

        public async Task<OperationTypeDto> RemoveAsync(Guid id){
            
            var operationType = await this._repo.GetByIdAsync(new OperationTypeId(id)) ?? throw new NullReferenceException("Not Found Operation Type: " + id);

            operationType.Disable();

            await this._repo.UpdateAsync(operationType); 

            await this._unitOfWork.CommitAsync();

            return new OperationTypeDto{Id = operationType.Id.AsGuid(), Name = operationType.Name.Name, EstimatedDuration = operationType.EstimatedDuration.Minutes, AnesthesiaTime = operationType.AnesthesiaTime.Minutes,
            CleaningTime = operationType.CleaningTime.Minutes, SurgeryTime = operationType.SurgeryTime.Minutes, OperationTypeStatus = operationType.OperationTypeStatus.ToString(),StaffSpecializationDtos = operationType.OperationTypeSpecializations.Select(ots => new StaffSpecializationDto {
            SpecializationId = ots.Specialization.Id.AsGuid().ToString(),SpecializationName = ots.Specialization.Name.Name,NumberOfStaff = ots.NumberOfStaff.Number }).ToList()};
        }

        public async Task<OperationTypeDto> EditOperationType(Guid id, EditingOperationTypeDto dto){
            var operationType = await this._repo.GetByIdAsync(new OperationTypeId(id)) ?? throw new NullReferenceException("Not Found Operation Type: " + id);
            if (dto.Name!=null){
                operationType.ChangeName(new OperationTypeName(dto.Name));
            }
            if (dto.EstimatedDuration!=null){
                operationType.ChangeEstimatedDuration(new EstimatedDuration(dto.EstimatedDuration.Value));
            }
            if (!dto.StaffBySpecializations.IsNullOrEmpty()){
                foreach(var staffBySpecialization in dto.StaffBySpecializations){
                    var id1 = new OperationTypeSpecializationId(new OperationTypeId(id), new SpecializationId(staffBySpecialization.Key));
                    var operationTypeSpecialization = await _operationTypeSpecializationRepo.GetByIdAsync(id1) ?? throw new NullReferenceException("Not Found Operation Type: " + id1);
                    operationTypeSpecialization.ChangeNumberOfStaff(new NumberOfStaff(staffBySpecialization.Value));
                    await this._operationTypeSpecializationRepo.UpdateAsync(operationTypeSpecialization);
                }
            }
            await this._repo.UpdateAsync(operationType);
            await this._unitOfWork.CommitAsync();
            return new OperationTypeDto{Id = operationType.Id.AsGuid(), Name = operationType.Name.Name, EstimatedDuration = operationType.EstimatedDuration.Minutes, AnesthesiaTime = operationType.AnesthesiaTime.Minutes,
            CleaningTime = operationType.CleaningTime.Minutes, SurgeryTime = operationType.SurgeryTime.Minutes, OperationTypeStatus = operationType.OperationTypeStatus.ToString(),StaffSpecializationDtos = operationType.OperationTypeSpecializations.Select(ots => new StaffSpecializationDto {
            SpecializationId = ots.Specialization.Id.AsGuid().ToString(),SpecializationName = ots.Specialization.Name.Name,NumberOfStaff = ots.NumberOfStaff.Number }).ToList()};
        }
    }
}