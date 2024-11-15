using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRooms
{
    public interface ISurgeryRoomRepository : IRepository<SurgeryRoom, RoomNumber> {
        
    }
}