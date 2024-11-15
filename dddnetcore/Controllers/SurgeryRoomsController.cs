using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.SurgeryRooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dddnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurgeryRoomsController : ControllerBase
    {
        private readonly SurgeryRoomService _service;

        public SurgeryRoomsController(SurgeryRoomService service) {
            this._service = service;
        }

        [HttpGet]
        [Authorize(Policy = "RequiredBackofficeRole")]
        public async Task<ActionResult<IEnumerable<SurgeryRoomDto>>> GetSurgeryRooms() {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequiredBackofficeRole")]
        public async Task<ActionResult<SurgeryRoomDto>> GetById(string id) {
            var surgeryRoom = await _service.GetByIdAsync(new RoomNumber(id));

            if (surgeryRoom == null)
                return NotFound();

                return surgeryRoom;
        }
    }
}