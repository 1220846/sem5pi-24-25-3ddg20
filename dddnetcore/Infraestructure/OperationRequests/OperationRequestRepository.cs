using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.OperationRequests{
    public class OperationRequestRepository : BaseRepository<OperationRequest,OperationRequestId> ,IOperationRequestRepository{
        
        private readonly DDDSample1DbContext _context;

        public OperationRequestRepository(DDDSample1DbContext context):base(context.OperationRequest)
        {
           _context=context;
        }

        public new async Task<OperationRequest> GetByIdAsync(OperationRequestId id) {
        
            return await _context.OperationRequest.FirstOrDefaultAsync(op => op.Id == id);
        }

        public async Task<List<OperationRequest>> GetOperationRequestsAsync(string patientId = null, Guid? operationTypeId = null, string priority = null, string status = null)
        {
            try
            {
                var query = _context.OperationRequest.AsQueryable();

                if (!string.IsNullOrEmpty(patientId))
                {
                    query = query.Where(or => or.MedicalRecordNumber == new MedicalRecordNumber(patientId));
                }

                if (operationTypeId.HasValue && operationTypeId.Value != Guid.Empty)
                {
                    query = query.Where(or => or.OperationTypeId == new OperationTypeId(operationTypeId.Value));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    if (Enum.TryParse<OperationRequestStatus>(status, true, out var parsedStatus))
                    {
                        query = query.Where(or => or.Status == parsedStatus);
                    }
                    else
                    {
                        return new List<OperationRequest>();
                    }
                }

                if (!string.IsNullOrEmpty(priority))
                {
                    if (Enum.TryParse<Priority>(priority, true, out var parsedPriority))
                    {
                        query = query.Where(or => or.Priority == parsedPriority);
                    }
                    else
                    {
                        return new List<OperationRequest>();
                    }
                }

                return await query.ToListAsync();
            }
            catch (BusinessRuleValidationException)
            {
                return new List<OperationRequest>();
            }
        }

        public async Task<OperationRequest> UpdateAsync(OperationRequest operationRequest)
        {
            _context.OperationRequest.Update(operationRequest);
            
            await _context.SaveChangesAsync();

            return operationRequest;
        }
    }
}