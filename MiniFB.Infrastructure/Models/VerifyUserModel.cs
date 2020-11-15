using MiniFB.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.Infrastructure.Models
{
    public class VerifyUserModel
    {
        public  string Password { get; set; }
        public User User { get; set; }
    }
}
