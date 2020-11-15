using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.Infrastructure.Dtos
{
    public class LoginReturnDto
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
