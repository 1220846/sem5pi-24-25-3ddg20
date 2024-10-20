using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dddnetcore.Domain.Staffs;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Patients;
using Microsoft.AspNetCore.Mvc;
using dddnetcore.Domain.Patients;

namespace DDDSample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _service;

        public PatientsController(PatientService service) {
            this._service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetById(string id)
        {
            var patient = await _service.GetByIdAsync(new MedicalRecordNumber(id));

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> Create(CreatingPatientDto dto) {
            try {
                PatientDto patient = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new {id = patient.Id.ToString()}, patient);
            
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
    }
}