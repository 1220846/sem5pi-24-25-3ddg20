using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationRequests;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using dddnetcore.Domain.OperationRequests.UpdateOperationRequestDto;

namespace DDDSample1.Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class OperationRequestsController : ControllerBase{

        private readonly OperationRequestService _service;

        public  OperationRequestsController(OperationRequestService service){
            _service = service;
        }

        // GET: api/operationRequests
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationRequestDto>> GetById(Guid id)
        {
            var operationRequest = await _service.GetByIdAsync(new OperationRequestId(id));

            if (operationRequest == null)
            {
                return NotFound();
            }

            return operationRequest;
        }

        // POST: api/operationRequests
        [HttpPost()]
        [Authorize(Policy = "RequiredDoctorRole")]
        public async Task<ActionResult<OperationRequestDto>> Create(CreatingOperationRequestDto dto)
        {
            try{
                var operationRequest = await _service.AddOperationRequestAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = operationRequest.Id }, operationRequest);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }catch(NullReferenceException exception){
                
                return BadRequest(new {exception.Message});
            }
        }

        // GET: api/operationRequests/filter
        [HttpGet("filter")]
        [Authorize(Policy = "RequiredDoctorRole")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetOperationRequestsAsync(string patientId = null, Guid? operationTypeId = null, string priority=null ,string status = null)
        {
            return await _service.GetOperationRequestsAsync(patientId,operationTypeId,priority,status);
        }

        // PUT: api/operationRequests/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "RequiredDoctorRole")]
        public async Task<ActionResult<OperationRequestDto>> UpdateOperationRequest(Guid id,UpdateOperationRequestDto dto)
        {
            try{
                var operationRequestDto = await _service.UpdateOperationRequestAsync(id,dto);

                return Ok(dto);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});

            }catch(NullReferenceException exception){
                
                return NotFound(new {exception.Message});
            }catch(Exception){
                
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
            
        }
    }
}