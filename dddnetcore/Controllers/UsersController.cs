using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Auth;
using Microsoft.AspNetCore.Authorization;

namespace DDDSample1.Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase{

        private readonly UserService _service;

        public  UsersController(UserService service){
            _service = service;
        }

        // GET: api/users/
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(String id)
        {
            var user = await _service.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/users
        [HttpPost()]
        public async Task<ActionResult<UserDto>> Create(CreatingUserDto dto)
        {
            try{
                var user = await _service.AddBackofficeUserAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = user.Username }, user);

            }catch(Exception exception){
                
                return BadRequest(new {exception.Message});
            }
        }

        // POST: api/users/patients
        [HttpPost("patients")]
        public async Task<ActionResult<UserDto>> CreateUserPatient(CreatingUserPatientDto dto)
        {
            try{
                var user = await _service.AddUserPatientAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = user.Username }, user);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});
            }catch(NullReferenceException exception){
                
                return NotFound(new {exception.Message});
            }catch(Exception){
                
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        // PUT: api/users/patients/{username}
        [HttpPut("patients/{username}")]
        [Authorize(Policy = "RequiredPatientRole")]
        public async Task<ActionResult<UserDto>> UpdateUserPatient(string username,UpdateUserPatientDto dto)
        {
            try{
                var userDto = await _service.UpdateUserPatientAsync(username,dto);

                return Ok(userDto);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});

            }catch(NullReferenceException exception){
                
                return NotFound(new {exception.Message});
            }catch(Exception){
                
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
            
        }
        
        // POST: api/users/patients/request-delete/{username}
        [HttpPost("patients/request-delete/{username}")]
        [Authorize(Policy = "RequiredPatientRole")]
        public async Task<IActionResult> RequestDeleteUserPatient(string username)
        {
            try
            {
                var result = await _service.RequestDeleteUserPatientAsync(username);
                return Ok(result);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(new {message = ex.Message});
            
            }catch(BusinessRuleValidationException ex){
                
                return StatusCode(400, new { message = ex.Message });

            }catch(Exception){
                
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        // DELETE: api/users/patients/confirm-delete/{username}
        [HttpDelete("patients/confirm-delete/{username}")]
        public async Task<ActionResult<UserDto>> DeleteUserPatient(string username)
        {
            try{
                var userDto = await _service.DeleteUserPatientAsync(username);

                return Ok(userDto);

            }catch(BusinessRuleValidationException exception){
                
                return BadRequest(new {exception.Message});

            }catch(NullReferenceException exception){
                
                return NotFound(new {exception.Message});
            }catch(Exception){
                
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                var loginDto = await _service.LoginAsync(loginRequestDto);

                return Ok(loginDto);
            }catch (Exception ex){
                
                return Unauthorized(new { Message ="Login failed. Please check your credentials and try again.", ErrorMessage = ex.Message});
            }
        }
    }
}