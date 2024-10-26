using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dddnetcore.Domain.Staffs;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Patients;
using Microsoft.AspNetCore.Mvc;
using dddnetcore.Domain.Patients;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Policy = "RequiredAdminRole")]
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

        [HttpPut("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<PatientDto>> EditPatient(string id, EditingPatientDto dto) {
            try {
                var patientDto = await _service.EditPatientAsync(id, dto);
                return Ok(patientDto);
            } catch(NullReferenceException exception){
                
                return NotFound(new {exception.Message});
                
            } catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});

            }
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<PatientDto>> DeletePatient(string id)
        {
            try{
                var patientDto = await _service.DeletePatientAsync(id);

                return Ok(patientDto);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});

            }catch(NullReferenceException exception){
                
                return NotFound(new {exception.Message});
            }
            
        }

        [HttpGet("filter")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients(string firstName = null, string lastName = null, string fullName = null, string email = null, string birthDate = null,
        string phoneNumber = null, string id = null, string gender = null, int pageNumber = 1, int pageSize = 10) {
            return await _service.GetPatientsAsync(firstName, lastName, fullName, email, birthDate, phoneNumber, id, gender, pageNumber, pageSize);
        }
    }
}