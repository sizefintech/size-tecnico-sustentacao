using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.DomainObjects
{
    public class DomainException : AggregateException
    {
        public DomainException()
        { }

        public DomainException(string message) : base(message)
        { }

        public DomainException(Exception exception) : base(exception)
        { }

        public DomainException(IEnumerable<Exception> exceptions) : base(exceptions)
        { }

        public DomainException(string message, Exception innerException) : base(message, innerException)
        { }

        public DomainException(string message, List<Exception> innerExceptions) : base(message, innerExceptions)
        { }
    }
}
