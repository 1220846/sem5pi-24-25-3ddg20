using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.OperationTypes
{
    public class OperationTypeRepository : BaseRepository<OperationType,OperationTypeId> ,IOperationTypeRepository{

        private readonly DDDSample1DbContext _context;
        
        public OperationTypeRepository(DDDSample1DbContext context):base(context.OperationTypes)
        {
            _context = context;
        }

        public async Task<List<OperationType>> GetOperationTypesAsync(string name = null, Guid? specializationId = null, string status = null)
        {
            var query = _context.OperationTypes.AsQueryable();

            if (!string.IsNullOrEmpty(name)){
                query = query.Where(operationType => operationType.Name.Name.Equals(name));
            }

            if (specializationId.HasValue && specializationId.Value != Guid.Empty){
                query = query.Where(operationType => operationType.OperationTypeSpecializations
                    .Any(specialization => specialization.Id.ToString() == specializationId.Value.ToString()));
            }

            if (!string.IsNullOrEmpty(status)) {
                query = query.Where(operationType => operationType.OperationTypeStatus.ToString().ToUpper() == status.ToUpper());
            }

            query = query.Include(o => o.OperationTypeSpecializations)
                 .ThenInclude(ots => ots.Specialization);

            return await query.ToListAsync();
        }
    }
}