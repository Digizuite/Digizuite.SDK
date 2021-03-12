using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record LinkMetadataResponse(string? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Link;
    }
}