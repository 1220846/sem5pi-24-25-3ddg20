using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationTypes;

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
                return NotFound();
            }

            return operationType;
        }

        // POST: api/operationtypes
        [HttpPost]
        public async Task<ActionResult<OperationTypeDto>> Create(CreatingOperationTypeDto dto)
        {
            try{
                var operationType = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetGetById), new { id = operationType.Id }, operationType);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }
        }
    }
}