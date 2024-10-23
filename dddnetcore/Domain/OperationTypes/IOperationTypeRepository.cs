using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationTypes
{
    public interface IOperationTypeRepository: IRepository<OperationType, OperationTypeId>
    {

        public Task<List<OperationType>> GetOperationTypesAsync(string name = null, Guid? specializationId = null, string status = null);

        Task<OperationType> UpdateAsync(OperationType operationType);

    }
}