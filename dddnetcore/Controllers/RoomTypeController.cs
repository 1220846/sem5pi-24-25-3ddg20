using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.RoomTypes;
using Microsoft.AspNetCore.Authorization;

namespace DDDSample1.Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class RoomTypesController : ControllerBase{

        private readonly RoomTypeService _service;

        public  RoomTypesController(RoomTypeService service){
            _service = service;
        }

        // GET: api/roomtypes
        [HttpGet]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<IEnumerable<RoomTypeDto>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        // GET: api/operationtypes/
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomTypeDto>> GetById(Guid id)
        {
            var roomType = await _service.GetByIdAsync(new RoomTypeId(id));

            if (roomType == null)
            {
                return NotFound($"Not Found Room Type with Id: {id}");
            }

            return roomType;
        }

        // POST: api/operationtypes
        [HttpPost]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<RoomTypeDto>> Create(CreatingRoomTypeDto dto)
        {
            try{
                var roomType = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = roomType.Id }, roomType);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }catch(Exception){
                
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}