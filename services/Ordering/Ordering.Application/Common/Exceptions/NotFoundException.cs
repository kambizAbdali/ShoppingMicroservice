using System;

namespace Ordering.Application.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource is not found
    /// </summary>
    public class NotFoundException : ApplicationException
    {
        public string EntityName { get; }
        public string EntityId { get; }

        public NotFoundException(string entityName, string entityId)
            : base($"'{entityName}' with Id '{entityId}' was not found.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public NotFoundException(string entityName, string entityId, Exception innerException)
            : base($"'{entityName}' with Id '{entityId}' was not found.", innerException)
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}