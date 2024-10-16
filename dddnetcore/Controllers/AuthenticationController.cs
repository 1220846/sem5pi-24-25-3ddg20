using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDDSample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;
        private readonly string  _domain;
        private readonly string  _audience;
        private readonly string  _clientId;
        private readonly string  _clientSecret;

        public AuthenticationController()
        {
            _authenticationService = new AuthenticationService(); 
            _domain = Environment.GetEnvironmentVariable("Auth0_Domain");
            _audience = Environment.GetEnvironmentVariable("Auth0_Audience");
            _clientId = Environment.GetEnvironmentVariable("Auth0_ClientId");
            _clientSecret = Environment.GetEnvironmentVariable("Auth0_ClientSecret");
        }

        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new
            {
                Message = "Hello from a public endpoint!"
            });
        }

        [HttpGet("private")]
        [Authorize]
        public IActionResult Private()
        {
            return Ok(new
            {
                Message = "Hello from a private endpoint!"
            });
        }

        [HttpGet("private-scoped")]
        [Authorize("read:messages")]
        public IActionResult Scoped()
        {
            return Ok(new
            {
                Message = "Hello from a private-scoped endpoint!"
            });
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetAcessToken()
        {

            var token = await _authenticationService.GetAccessToken(_domain, _audience,_clientId, _clientSecret);

            return Ok(new { acess_token = token });
        }
    }
}