using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
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

        public new async Task<OperationType> GetByIdAsync(OperationTypeId id) {
        
            return await _context.OperationTypes.Include(op => op.OperationTypeSpecializations).ThenInclude(ots => ots.Specialization).FirstOrDefaultAsync(op => op.Id == id);
        }

        public async Task<List<OperationType>> GetOperationTypesAsync(string name = null, Guid? specializationId = null, string status = null){
            try{
                var query = _context.OperationTypes.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(operationType => operationType.Name.Equals(new OperationTypeName(name)));
                }

                if (specializationId.HasValue && specializationId.Value != Guid.Empty)
                {
                    query = query.Where(operationType => operationType.OperationTypeSpecializations.Any(specialization => specialization.Id == new SpecializationId(specializationId.Value)));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    if (Enum.TryParse<OperationTypeStatus>(status, true, out var parsedStatus)){
                        query = query.Where(operationType => operationType.OperationTypeStatus == parsedStatus);
                    }
                    else{
                        return new List<OperationType>(); 
                    }
                }

                query = query.Include(o => o.OperationTypeSpecializations)
                            .ThenInclude(ots => ots.Specialization);

                return await query.ToListAsync();
            }
            catch (BusinessRuleValidationException){
                return new List<OperationType>();
            }
        }

        public async Task<OperationType> UpdateAsync(OperationType operationType) {
            _context.OperationTypes.Update(operationType);
            
            await _context.SaveChangesAsync();

            return operationType;
        }
    }
}