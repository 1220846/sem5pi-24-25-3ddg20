using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.OperationRequests
{
    public interface IOperationRequestRepository : IRepository<OperationRequest,OperationRequestId>
    {
        Task<OperationRequestDto> addOperationRequestAsync();
    }
}