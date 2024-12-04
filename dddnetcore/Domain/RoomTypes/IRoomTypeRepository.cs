using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.RoomTypes
{
    public interface IRoomTypeRepository: IRepository<RoomType, RoomTypeCode>
    {
    }
}