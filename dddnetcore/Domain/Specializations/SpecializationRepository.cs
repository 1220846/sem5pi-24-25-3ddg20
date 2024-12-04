using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;

namespace DDDSample1.Domain.OperationTypes
{
    public interface ISpecializationRepository: IRepository<Specialization, SpecializationId>
    {
        Task<Specialization> UpdateAsync(Specialization specialization);
    }
}