using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dddnetcore.Domain.Staffs;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDDSample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly StaffService _service;

        public StaffsController(StaffService service) {
            this._service = service;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<StaffDto>> GetById(string id)
        {
            var staff = await _service.GetByIdAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return staff;
        }

        [HttpPost]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<StaffDto>> Create(CreatingStaffDto dto) {
            try {
                StaffDto staff = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new {id = staff.Id.ToString()}, staff);
            
            } catch (BusinessRuleValidationException e) {
                return BadRequest(new {e.Message});
            } catch (NullReferenceException e) {
                return NotFound(new {e.Message});
            } catch (ArgumentNullException e) {
                return BadRequest(new {e.Message});
            } catch (Exception) {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("filter")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetStaffs(string firstName = null, string lastName = null, string fullName = null, string email = null, Guid? specializationId = null,
        string phoneNumber = null, string id = null, string licenseNumber = null,
        string status = null, int pageNumber = 1, int pageSize = 10) {
            return await _service.GetStaffsAsync(firstName, lastName, fullName, email, specializationId, phoneNumber, id, licenseNumber, status, pageNumber, pageSize);
        }

        [HttpGet("count")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<int>> GetStaffCount() {
            try {
                var count = await _service.GetStaffsCountAsync();
                return Ok(count);
            } catch (Exception e) {
                return BadRequest(new { e.Message});
            }
        }
        
        [HttpPatch("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<StaffDto>> EditStaff(string id, EditingStaffDto dto) {
            try {
                var staffDto = await _service.EditStaffAsync(id, dto);
                return Ok(staffDto);
            } catch(NullReferenceException exception){
                return NotFound(new {exception.Message});
            } catch(BusinessRuleValidationException exception){
                return BadRequest(new {exception.Message});
            } catch (Exception) {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<StaffDto>> DeactivateStaff(string id) {
            try {
                return await _service.RemoveAsync(id);
            } catch(NullReferenceException exception){
                return NotFound(new {exception.Message});
            } catch(BusinessRuleValidationException exception){
                return BadRequest(new {exception.Message});
            } catch (Exception) {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}