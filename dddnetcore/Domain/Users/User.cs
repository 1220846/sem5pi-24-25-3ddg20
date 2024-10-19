using System;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;

namespace DDDSample1.Domain.Users
{
    public class User : Entity<Username>, IAggregateRoot
    {
        public Email Email { get; private set; }
        public Role Role { get; private set; }

        public Staff Staff {get; private set;}
        public Patient Patient {get; private set;}

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

        public void ChangeUsername(Username newUsername){

            ArgumentNullException.ThrowIfNull(newUsername);
            this.Id = newUsername;
        }

        public void ChangeEmail(Email newEmail){
            ArgumentNullException.ThrowIfNull(newEmail);
            this.Email = newEmail;
        }
    }
}
