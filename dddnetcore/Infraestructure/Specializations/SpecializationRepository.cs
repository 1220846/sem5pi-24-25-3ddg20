using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.Specializations;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.Specializations
{
    public class SpecializationRepository : BaseRepository<Specialization,SpecializationId> ,ISpecializationRepository{
        
        private readonly DDDSample1DbContext _context;

        public SpecializationRepository(DDDSample1DbContext context):base(context.Specializations)
        {
            _context = context;
        }

        public async Task<List<Specialization>> GetSpecializationsAsync(string namePartial = null, string codeExact = null, string descriptionPartial = null) {
            try {
                var query = _context.Specializations.AsQueryable();


                if (!string.IsNullOrEmpty(codeExact))
                    query = query.Where(specialization => specialization.Code.Equals(new SpecializationCode(codeExact)));

                var specializations = await query.ToListAsync();

                if (!string.IsNullOrEmpty(namePartial))
                    specializations = specializations
                        .Where(specialization => specialization.Name.Name.Contains(namePartial, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                if (!string.IsNullOrEmpty(descriptionPartial))
                    specializations = specializations
                        .Where(specialization => specialization.Description.Description.Contains(descriptionPartial, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                return specializations;

            } catch (BusinessRuleValidationException) {
                throw new BusinessRuleValidationException("Filters badly formatted!");
            }
        }

        public async Task<Specialization> UpdateAsync(Specialization specialization)
        {
            _context.Specializations.Update(specialization);
            await _context.SaveChangesAsync();
            return specialization;
        }
    }
}