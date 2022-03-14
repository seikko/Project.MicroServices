using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Course.Web.Exceptions
{
    public class UnAuthroizeException : Exception
    {
        public UnAuthroizeException()
        {
        }

        public UnAuthroizeException(string message) : base(message)
        {
        }

        public UnAuthroizeException(string message, Exception innerException) : base(message, innerException)
        {
        }

         
    }
}
