using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.OperationRequests
{
    public interface IOperationRequestRepository : IRepository<OperationRequest, OperationRequestId>
    {
        public Task<List<OperationRequest>> GetOperationRequestsAsync(string patientId = null, Guid? operationTypeId = null, string priority=null ,string status = null);
        Task<OperationRequest> UpdateAsync(OperationRequest operationRequest);
        public Task<List<OperationRequest>> GetByDoctorIdAndStatusAsync(StaffId doctorId,OperationRequestStatus status);
    }
}