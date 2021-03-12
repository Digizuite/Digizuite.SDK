using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record BitMetadataResponse(bool Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Bit;
    }
}