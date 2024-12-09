
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.SurgeryRooms
{
    public interface ISurgeryRoomRepository : IRepository<SurgeryRoom, RoomNumber> {
        
        Task<bool> IsRoomAvailableAsync(RoomNumber roomNumber, DateTime startTime, DateTime endTime, Guid? excludedAppointmentId = null);
    }
}