using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a validation fails
    /// </summary>
    public class ValidationException : ApplicationException
    {
        public string PropertyName { get; }
        public string InvalidValue { get; }

        public ValidationException(string propertyName, string invalidValue, string message)
            : base($"Validation failed for '{propertyName}' with value '{invalidValue}'. {message}")
        {
            PropertyName = propertyName;
            InvalidValue = invalidValue;
        }

        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
