using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypesSpecializations;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.OperationTypesSpecializations
{
    public class OperationTypeSpecializationRepository : BaseRepository<OperationTypeSpecialization,OperationTypeSpecializationId> ,IOperationTypeSpecializationRepository{
    
        private readonly DDDSample1DbContext _context;
        public OperationTypeSpecializationRepository(DDDSample1DbContext context):base(context.OperationTypesSpecializations)
        {
            _context = context;
        }

        public async Task<OperationTypeSpecialization> UpdateAsync(OperationTypeSpecialization operationTypeSpecialization)
        {
            _context.OperationTypesSpecializations.Update(operationTypeSpecialization);
            
            await _context.SaveChangesAsync();

            return operationTypeSpecialization;
        }

        public new async Task<List<OperationTypeSpecialization>> GetAllAsync(){
            return await _context.OperationTypesSpecializations.Include(a => a.Specialization).Include(s => s.OperationType).ToListAsync();
        }
    }
}