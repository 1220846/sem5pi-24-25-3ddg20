using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationRequests;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

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
            //try{
                var operationRequest = await _service.AddOperationRequestAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = operationRequest.Id }, operationRequest);

            /*}catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }catch(NullReferenceException exception){
                
                return BadRequest(new {exception.Message});
            }*/
        }

        // GET: api/operationRequests/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<OperationRequestDto>>> GetOperationRequestsAsync(string patientId = null, Guid? operationTypeId = null, string priority=null ,string status = null)
        {
            return await _service.GetOperationRequestsAsync(patientId,operationTypeId,priority,status);
        }

        // DELETE api/operationRequests/filter
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequiredDoctorRole")]
        public async Task<ActionResult<OperationRequestDto>> RemoveOperationRequisition(Guid id) {
            try {
                var dto = await _service.RemoveAsync(id);
                return Ok(dto);
            } catch(NullReferenceException exception){
                return NotFound(new {exception.Message});
            } catch (BusinessRuleValidationException exception) {
                var message = exception.Message;
                if (message.Equals("Cannot remove scheduled operation requests!"))
                    return Forbid();
                return BadRequest();
            } catch(Exception){
                return Forbid();
            }
        } 
    }
}