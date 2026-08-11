using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Common.Exceptions
{

    /// <summary>
    /// Exception thrown when a duplicate or conflict occurs
    /// </summary>
    public class ConflictException : ApplicationException
    {
        public string EntityName { get; }
        public string ConflictReason { get; }

        public ConflictException(string entityName, string conflictReason)
            : base($"Conflict occurred for '{entityName}'. Reason: {conflictReason}")
        {
            EntityName = entityName;
            ConflictReason = conflictReason;
        }

        public ConflictException(string message) : base(message) { }
        public ConflictException(string message, Exception innerException) : base(message, innerException) { }
    }
}
