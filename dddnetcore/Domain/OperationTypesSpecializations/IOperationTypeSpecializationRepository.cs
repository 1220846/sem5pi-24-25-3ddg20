using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationTypesSpecializations
{
    public interface IOperationTypeSpecializationRepository : IRepository<OperationTypeSpecialization, OperationTypeSpecializationId>
    {
        Task<OperationTypeSpecialization> UpdateAsync(OperationTypeSpecialization operationTypeSpecialization);
    }
}