using System;
using System.Collections.Generic;
using System.Text;
using Twilio.Types;

namespace MiniFB.Infrastructure.Models
{
    public class MessageModel
    {
        public PhoneNumber To { get; set; }
        public PhoneNumber From { get; set; }
        public string  Body { get; set; }
    }
}
