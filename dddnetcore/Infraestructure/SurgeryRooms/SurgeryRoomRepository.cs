using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnetcore.Infraestructure.SurgeryRooms
{
    public class SurgeryRoomRepository : BaseRepository<SurgeryRoom, RoomNumber>, ISurgeryRoomRepository
    {
        private readonly DDDSample1DbContext _context;

        public SurgeryRoomRepository(DDDSample1DbContext context):base(context.SurgeryRooms) {
            _context = context;
        }       
    }
}