using System;

namespace DDDSample1.Domain.Users
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public UserDto() {}

        public UserDto(User user) {
            this.Username = user.Id.Name;
            this.Email = user.Email.Address;
            this.Role = EnumDescription.GetEnumDescription(user.Role);
        } 

    }
}
