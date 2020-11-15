using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.Infrastructure.Common
{
    public class BusinessException : Exception
    {
        public string MessageEn { get; set; }
        public string MessageAr { get; set; }
        public new Exception InnerException { get; set; }
        public BusinessException()
        {
        }
        public BusinessException(string message) : base(message)
        {
            this.MessageEn = this.MessageAr = message;
        }
        public BusinessException(string message, Exception innerException): base(message, innerException)
        {
            this.InnerException = innerException;
            this.MessageEn = this.MessageAr = message;
        }
    }
}
