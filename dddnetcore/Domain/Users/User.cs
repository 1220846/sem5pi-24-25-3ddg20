using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Users
{
    public class User : Entity<Username>, IAggregateRoot
    {
        public Email Email { get; private set; }
        public Role Role { get; private set; }

        public User(Username name, Email email, Role userRole)
        {
            this.Id = name;
            this.Email = email;
            this.Role = userRole;
        }

        public User()
        {
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otherUser = (User)obj;

            return Id.Equals(otherUser.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Email, Role);
        }
    }
}
