using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dddnetcore.Domain.Staffs;
using DDDSample1.Domain.Shared;
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
            } catch (ArgumentException) {
                return Forbid();
            }
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetStaffs(string firstName = null, string lastName = null, string fullName = null, string email = null, Guid? specializationId = null,
        string phoneNumber = null, string id = null, string licenseNumber = null, int pageNumber = 1, int pageSize = 10) {
            return await _service.GetStaffsAsync(firstName, lastName, fullName, email, specializationId, phoneNumber, id, licenseNumber, pageNumber, pageSize);
        }

        /*
        [HttpPut("/{id}")]
        public async Task<ActionResult<StaffDto>> EditStaff(string id, EditingStaffDto dto) {
            try {
                var staffDto = await _service.EditStaffAsync(id, dto);
                return Ok(staffDto);
            } catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});

            } catch(NullReferenceException exception){
                
                return NotFound(new {exception.Message});
            }
        }*/
    }
}