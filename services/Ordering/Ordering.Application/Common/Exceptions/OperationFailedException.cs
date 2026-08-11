using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when an operation fails to execute properly
    /// </summary>
    public class OperationFailedException : ApplicationException
    {
        public string OperationName { get; }
        public string EntityName { get; }
        public string EntityId { get; }

        public OperationFailedException(string operationName, string entityName, string entityId)
            : base($"Operation '{operationName}' failed for '{entityName}' with Id '{entityId}'.")
        {
            OperationName = operationName;
            EntityName = entityName;
            EntityId = entityId;
        }

        public OperationFailedException(string operationName, string entityName, string entityId, Exception innerException)
            : base($"Operation '{operationName}' failed for '{entityName}' with Id '{entityId}'.", innerException)
        {
            OperationName = operationName;
            EntityName = entityName;
            EntityId = entityId;
        }

        public OperationFailedException(string message) : base(message) { }
        public OperationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
