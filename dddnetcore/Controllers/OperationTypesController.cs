using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationTypes;
using Microsoft.AspNetCore.Authorization;

namespace DDDSample1.Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class OperationTypesController : ControllerBase{

        private readonly OperationTypeService _service;

        public  OperationTypesController(OperationTypeService service){
            _service = service;
        }

        // GET: api/operationtypes/
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationTypeDto>> GetGetById(Guid id)
        {
            var operationType = await _service.GetByIdAsync(new OperationTypeId(id));

            if (operationType == null)
            {
                return NotFound($"Not Found Operation Type with Id: {id}");
            }

            return operationType;
        }

        // POST: api/operationtypes
        [HttpPost]
        //[Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<OperationTypeDto>> Create(CreatingOperationTypeDto dto)
        {
            try{
                var operationType = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetGetById), new { id = operationType.Id }, operationType);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }catch(NullReferenceException exception){
                
                return NotFound(new {exception.Message});
            }catch(Exception){
                
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        // GET: api/operationtypes/filter
        [HttpGet("filter")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<IEnumerable<OperationTypeDto>>> GetOperationTypes(string name = null, Guid? specializationId = null, string status = null)
        {   
            return await _service.GetOperationTypesAsync(name,specializationId,status);
        }

        // GET: api/operationtypes/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<OperationTypeDto>> RemoveOperationTypes(Guid id)
        {
            try{
                return await _service.RemoveAsync(id);

            }catch(NullReferenceException exception){
                return NotFound(new {exception.Message});

            }catch(Exception){
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        // PUT: api/operationtypes/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<OperationTypeDto>> EditOperationType(Guid id, EditingOperationTypeDto dto)
        {
            try{
                return await _service.EditOperationType(id, dto);

            }catch(NullReferenceException exception){
                return NotFound(new {exception.Message});

            }catch(Exception){
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}