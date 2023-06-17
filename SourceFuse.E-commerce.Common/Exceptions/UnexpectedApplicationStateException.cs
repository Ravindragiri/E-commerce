using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Common.Exceptions
{
    public class UnexpectedApplicationStateException : Exception
    {
        public UnexpectedApplicationStateException(string message) : base(message)
        {

        }

        public UnexpectedApplicationStateException()
        {

        }
    }
}
