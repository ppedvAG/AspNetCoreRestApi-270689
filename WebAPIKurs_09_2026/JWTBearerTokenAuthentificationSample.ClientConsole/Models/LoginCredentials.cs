using System;
using System.Collections.Generic;
using System.Text;

namespace JWTBearerTokenAuthentificationSample.ClientConsole.Models
{
    public class LoginCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCredentialsResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
