using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Staffs{

    public class StaffContactInformation : IValueObject {
        public StaffEmail Email {get; private set;}
        public StaffPhone PhoneNumber {get; private set;}

        private StaffContactInformation() {}

        public StaffContactInformation(StaffEmail staffEmail, StaffPhone staffPhone) {
            this.Email = staffEmail;
            this.PhoneNumber = staffPhone;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;
            
            return Email.Equals(((StaffContactInformation)obj).Email) && PhoneNumber.Equals(((StaffContactInformation)obj).PhoneNumber);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Email, PhoneNumber);
        }
    }
}