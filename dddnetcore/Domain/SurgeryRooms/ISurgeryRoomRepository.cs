
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRooms
{
    public interface ISurgeryRoomRepository : IRepository<SurgeryRoom, RoomNumber>
    {
        
    }
}