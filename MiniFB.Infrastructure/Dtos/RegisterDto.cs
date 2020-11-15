using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.Infrastructure.Dtos
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
