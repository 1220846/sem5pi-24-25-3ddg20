using DDDSample1.Domain.Shared;
using dddnetcore.Domain.AvailabilitySlots;

namespace dddnetcore.Domain.AvailabilitySlots{
    public interface IAvailabilitySlotRepository : IRepository<AvailabilitySlot, AvailabilitySlotId> {}
}