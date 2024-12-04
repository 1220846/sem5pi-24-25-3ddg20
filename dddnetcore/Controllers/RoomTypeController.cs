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

        // GET: api/roomtypes/{code}
        [HttpGet("{code}")]
        public async Task<ActionResult<RoomTypeDto>> GetById(String code)
        {
            var roomType = await _service.GetByIdAsync(new RoomTypeCode(code));

            if (roomType == null)
            {
                return NotFound($"Not Found Room Type with code: {code}");
            }

            return roomType;
        }

        // POST: api/roomtypes
        [HttpPost]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<RoomTypeDto>> Create(CreatingRoomTypeDto dto)
        {
            try{
                var roomType = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { code = roomType.Code }, roomType);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }catch(Exception){
                
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}