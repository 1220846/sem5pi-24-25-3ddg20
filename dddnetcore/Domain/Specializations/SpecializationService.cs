using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using System.Reflection.Emit;
using DDDSample1.Domain.OperationTypes;
using dddnetcore.Domain.Specializations;
using System;

namespace DDDSample1.Domain.Specializations
{
    public class SpecializationService{

        private readonly IUnitOfWork _unitOfWork;
        
        private readonly ISpecializationRepository _repo;

        public SpecializationService(IUnitOfWork unitOfWork, ISpecializationRepository repo){

            this._unitOfWork = unitOfWork;
            this._repo = repo;
        }

        public async Task<List<SpecializationDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();
            
            List<SpecializationDto> listDto = list.ConvertAll<SpecializationDto>(specialization => 
                new SpecializationDto(specialization));

            return listDto;
        }

        public async Task<SpecializationDto> GetByIdAsync(SpecializationId id)
        {
            var specialization = await this._repo.GetByIdAsync(id);
            
            if(specialization == null)
                return null;

            return new SpecializationDto(specialization);
        }

        public async Task<SpecializationDto> AddAsync(CreatingSpecializationDto dto)
        {
            var specialization = new Specialization(new SpecializationName(dto.Name), new SpecializationCode(dto.Code), new SpecializationDescription(dto.Description));

            await this._repo.AddAsync(specialization);

            await this._unitOfWork.CommitAsync();

            return new SpecializationDto(specialization);
        }

        public async Task<SpecializationDto> EditSpecializationAsync(Guid id, EditingSpecializationDto dto) {
            Specialization specialization = await this._repo.GetByIdAsync(new SpecializationId(id)) ?? throw new NullReferenceException("Not Found Specialization: " + id);
            if (dto.Name != null) {
                specialization.ChangeName(new SpecializationName(dto.Name));
            }

            if (dto.Description != null) {
                specialization.ChangeDescription(new SpecializationDescription(dto.Description));
            }

            await this._repo.UpdateAsync(specialization);
            await this._unitOfWork.CommitAsync();
            return new SpecializationDto(specialization);
        }
    }
}