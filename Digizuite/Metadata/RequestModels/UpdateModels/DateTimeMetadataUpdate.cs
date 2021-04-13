using System;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record DateTimeMetadataUpdate : MetadataUpdate
    {
        public DateTime? Value { get; set; } = null;
    }
}