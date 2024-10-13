using System;

namespace DDDSample1.Domain.Users
{
    public class CreatingUserDto
    {
        public string Email { get; set; }
        public string Role { get; set; }

        public string Password { get; set; }
    }
}
