using System;

namespace Ordering.Core.Common
{
    public class BaseEntity
    {
        // Unique identifier for the entity
        public long Id { get; set; }

        // Audit properties for tracking creation details
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Audit properties for tracking modification details
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int Version { get; set; } = 1;
        public bool IsActive { get; set; } = true;
    }
}
