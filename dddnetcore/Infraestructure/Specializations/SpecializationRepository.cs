using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Specializations;
using DDDSample1.Infrastructure.Shared;

namespace DDDSample1.Infrastructure.Specializations
{
    public class SpecializationRepository : BaseRepository<Specialization,SpecializationId> ,ISpecializationRepository{
        
        private readonly DDDSample1DbContext _context;

        public SpecializationRepository(DDDSample1DbContext context):base(context.Specializations)
        {
            _context = context;
        }

        public async Task<Specialization> UpdateAsync(Specialization specialization)
        {
            _context.Specializations.Update(specialization);
            await _context.SaveChangesAsync();
            return specialization;
        }
    }
}