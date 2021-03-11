using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record BitMetadataUpdate : MetadataUpdate
    {
        public bool Value { get; set; } = false;
    }
}