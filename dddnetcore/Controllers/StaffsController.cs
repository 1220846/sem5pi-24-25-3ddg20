using System;
using System.Threading.Tasks;
using dddnetcore.Domain.Staffs;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
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
            var staff = await _service.GetByIdAsync(new StaffId(id));

            if (staff == null)
            {
                return NotFound();
            }

            return staff;
        }

        [HttpPost]
        public async Task<ActionResult<StaffDto>> Create(CreatingStaffDto dto) {
            //try {
                StaffDto staff = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new {id = staff.Id}, staff);
            /*
            } catch (BusinessRuleValidationException e) {
                return BadRequest(new {e.Message});
            } catch (NullReferenceException e) {
                return NotFound(new {e.Message});
            } catch (ArgumentNullException e) {
                return BadRequest(new {e.Message});
            } catch (ArgumentException) {
                return Forbid();
            }*/
        }
    }
}