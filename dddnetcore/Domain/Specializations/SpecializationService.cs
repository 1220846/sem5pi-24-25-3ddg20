using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using System.Reflection.Emit;
using DDDSample1.Domain.OperationTypes;

namespace DDDSample1.Domain.Specializations
{
    public class SpecializationService{

        private readonly IUnitOfWork _unitOfWork;
        
        private readonly ISpecializationRepository _repo;

        public SpecializationService(IUnitOfWork unitOfWork, ISpecializationRepository repo){

            this._unitOfWork = unitOfWork;
            this._repo = repo;
        }

        public async Task<SpecializationDto> GetByIdAsync(SpecializationId id)
        {
            var specialization = await this._repo.GetByIdAsync(id);
            
            if(specialization == null)
                return null;

            return new SpecializationDto{Id = specialization.Id.AsGuid(), Name = specialization.Name.Name};
        }

        public async Task<SpecializationDto> AddAsync(CreatingSpecializationDto dto)
        {
            var specialization = new Specialization(new SpecializationName(dto.Name));

            await this._repo.AddAsync(specialization);

            await this._unitOfWork.CommitAsync();

            return new SpecializationDto { Id = specialization.Id.AsGuid(), Name = specialization.Name.Name};
        }
    }
}