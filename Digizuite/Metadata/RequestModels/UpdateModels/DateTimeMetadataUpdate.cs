using System;
using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record DateTimeMetadataUpdate : MetadataUpdate
    {
        public DateTime? Value { get; set; } = null;
    }
}