using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Auth;

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
            Console.WriteLine(id);
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
                var user = await _service.addBackofficeUserAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = user.Username }, user);

            }catch(BusinessRuleValidationException exception){
                
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