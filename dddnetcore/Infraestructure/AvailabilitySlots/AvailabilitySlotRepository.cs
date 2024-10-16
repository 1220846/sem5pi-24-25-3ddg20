using System;
using dddnetcore.Domain.AvailabilitySlots;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.Shared;

namespace dddnetcore.Infraestructure.AvailabilitySlots
{
    public class AvailabilitySlotRepository : BaseRepository<AvailabilitySlot, AvailabilitySlotId>, IAvailabilitySlotRepository 
    {
        private readonly DDDSample1DbContext _context;

        public AvailabilitySlotRepository(DDDSample1DbContext context):base(context.AvailabilitySlots) {
            _context = context;
        }
    }
}