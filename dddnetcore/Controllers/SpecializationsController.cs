using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Specializations;

namespace DDDSample1.Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class SpecializationsController : ControllerBase{

        private readonly SpecializationService _service;

        public  SpecializationsController(SpecializationService service){
            _service = service;
        }

        // GET: api/specializations/
        [HttpGet("{id}")]
        public async Task<ActionResult<SpecializationDto>> GetById(Guid id)
        {
            var operationType = await _service.GetByIdAsync(new SpecializationId(id));

            if (operationType == null)
            {
                return NotFound();
            }

            return operationType;
        }

        // POST: api/specializations
        [HttpPost]
        public async Task<ActionResult<SpecializationDto>> Create(CreatingSpecializationDto dto)
        {
            try{
                var specialization = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = specialization.Id }, specialization);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }
        }
    }
}