using System.Collections.Generic;

namespace DDDSample1.Domain.Auth{
    public class LoginDto{

        public string LoginToken { get; set; }
        public List<string> Roles { get; set; }
    }
}