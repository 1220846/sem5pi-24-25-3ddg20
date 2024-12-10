using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using DDDSample1.Infrastructure;

namespace DDDSample1.Domain.OperationTypes
{
    public interface ISpecializationRepository: IRepository<Specialization, SpecializationId>   
    {
        Task<List<Specialization>> GetSpecializationsAsync(string namePartial = null, string codeExact = null, string descriptionPartial = null);
        Task<Specialization> UpdateAsync(Specialization specialization);
    }
}