using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record StringMetadataUpdate : MetadataUpdate
    {
        public string? Value { get; set; } = "";
    }
}