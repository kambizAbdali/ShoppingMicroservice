using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Common.Exceptions
{
    /// <summary>
    /// Base custom exception for application-level errors
    /// </summary>
    public abstract class ApplicationException : Exception
    {
        protected ApplicationException(string message) : base(message) { }
        protected ApplicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
