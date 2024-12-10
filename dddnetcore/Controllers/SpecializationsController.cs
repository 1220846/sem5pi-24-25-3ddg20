using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using Microsoft.AspNetCore.Authorization;
using dddnetcore.Domain.Specializations;

namespace DDDSample1.Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class SpecializationsController : ControllerBase{

        private readonly SpecializationService _service;

        public  SpecializationsController(SpecializationService service){
            _service = service;
        }

        // GET: api/specializations
        [HttpGet]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<IEnumerable<SpecializationDto>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("filter")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<IEnumerable<SpecializationDto>>> GetSpecializations(string namePartial = null, string codeExact = null, string descriptionPartial = null) {
            return await _service.GetSpecializationsAsync(namePartial, codeExact, descriptionPartial);
        }

        // GET: api/specializations/
        [HttpGet("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<SpecializationDto>> GetById(Guid id)
        {
            var specialization = await _service.GetByIdAsync(new SpecializationId(id));

            if (specialization == null)
            {
                return NotFound();
            }

            return specialization;
        }



        // POST: api/specializations
        [HttpPost]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<SpecializationDto>> Create(CreatingSpecializationDto dto)
        {
            Console.WriteLine(dto.Code);
            try{
                var specialization = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = specialization.Id }, specialization);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }catch(Exception){
                
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        //PATCH api/specializations
        [HttpPatch("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<SpecializationDto>> EditSpecialization(Guid id, EditingSpecializationDto dto) {
            try {
                return await _service.EditSpecializationAsync(id, dto);
            } catch (NullReferenceException e) {
                return NotFound(new {e.Message});
            } catch (BusinessRuleValidationException e) {
                return BadRequest(new {e.Message});
            } catch (Exception) {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<SpecializationDto>> RemoveSpecialization(Guid id) {
            try {
                return await _service.RemoveAsync(id);
            } catch (NullReferenceException e) {
                return NotFound(new {e.Message});
            } catch (BusinessRuleValidationException e) {
                return BadRequest(new {e.Message});
            } catch (Exception) {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}