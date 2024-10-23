using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Emails;
using System; 

namespace DDDSample1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null || emailRequest.To.Count == 0)
            {
                return BadRequest("Email request is invalid.");
            }

            try
            {
                await _emailService.SendEmailAsync(emailRequest.To, emailRequest.Subject, emailRequest.Body);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class EmailRequest
    {
        public List<string> To { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
