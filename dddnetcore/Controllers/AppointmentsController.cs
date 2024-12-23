using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDDSample1.Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class AppointmentsController : ControllerBase{

        private readonly AppointmentService _service;

        public  AppointmentsController(AppointmentService service){
            _service = service;
        }

        // GET: api/appointments
        [HttpGet]
        [Authorize(Policy = "RequiredBackofficeRole")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        // GET: api/appointments/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "RequiredBackofficeRole")]
        public async Task<ActionResult<AppointmentDto>> GetById(Guid id)
        {
            var appointment = await _service.GetByIdAsync(new AppointmentId(id));

            if (appointment == null)
            {
                return NotFound($"Not Found appointment with id: {id}");
            }

            return appointment;
        }

        // POST: api/appointments
        [HttpPost]
        [Authorize(Policy = "RequiredBackofficeRole")]
        public async Task<ActionResult<AppointmentDto>> Create(CreatingAppointmentDto dto)
        {
           try{
                var appointment = await _service.AddAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }catch(NullReferenceException exception){
                
                return NotFound(new {exception.Message});
            }catch(Exception){
                
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        // GET: api/appointments/patient/{medicalRecordNumber}
        [HttpGet("patient/{medicalRecordNumber}")]
        [Authorize(Policy = "RequiredPatientRole")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByPatientId(string medicalRecordNumber)
        {
            var appointments = await _service.GetByPatientIdAsync(medicalRecordNumber);

            if (appointments == null)
            {
                return NotFound($"No appointments found for patient with id: {medicalRecordNumber}");
            }

            return Ok(appointments);
        }

        [HttpGet("doctor/{doctorId}")]
        [Authorize(Policy = "RequiredDoctorRole")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByDoctorId(string doctorId)
        {
            var appointments = await _service.GetByDoctorIdAsync(doctorId);

            if (appointments == null)
            {
                return NotFound($"No appointments found for doctor with id: {doctorId}");
            }

            return Ok(appointments);
        }

        // PATCH: api/appointments/{id}
        [HttpPatch("{id}")]
        [Authorize(Policy = "RequiredDoctorRole")]
        public async Task<ActionResult<AppointmentDto>> UpdateAppointment(Guid id,UpdateAppointmentDto dto)
        {
            try{
                var userDto = await _service.UpdateAsync(id,dto);

                return Ok(userDto);

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