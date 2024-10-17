using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.OperationRequests;

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
            var operationType = await _service.GetByIdAsync(new OperationRequestId(id));

            if (operationType == null)
            {
                return NotFound();
            }

            return operationType;
        }

        // POST: api/operationRequests
        [HttpPost()]
        public async Task<ActionResult<OperationRequestDto>> Create(CreatingOperationRequestDto dto)
        {
            try{
                var operationRequest = await _service.AddOperationRequestAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = operationRequest.Id }, operationRequest);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }
        }
    }
}