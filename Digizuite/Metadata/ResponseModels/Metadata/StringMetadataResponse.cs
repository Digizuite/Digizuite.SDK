using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record StringMetadataResponse(string Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.String;
    }
}