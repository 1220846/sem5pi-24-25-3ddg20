using DDDSample1.Domain.RoomTypes;
using DDDSample1.Infrastructure.Shared;

namespace DDDSample1.Infrastructure.RoomTypes
{
    public class RoomTypeRepository : BaseRepository<RoomType,RoomTypeId> ,IRoomTypeRepository{
        
        public RoomTypeRepository(DDDSample1DbContext context):base(context.RoomTypes)
        {
           
        }

    }
}