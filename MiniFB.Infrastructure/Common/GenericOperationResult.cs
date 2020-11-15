using MiniFB.Infrastructure.Enums;
using System.Collections.Generic;


namespace MiniFB.Infrastructure.Common
{
    public class GenericOperationResult<T>
    {
        public T Data { get; set; }
        public List<T> ListOfData { get; set; }
        public List<string> Messages { get; set; }
        public OperationResultStatusEnum Status { get; set; }
        public GenericOperationResult()
        {
            Messages = new List<string>();
        }
        
    }
}
