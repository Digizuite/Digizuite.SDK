using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record IntMetadataResponse(int? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Int;

        public IntMetadataResponse() : this(0)
        {
        }
    }
}