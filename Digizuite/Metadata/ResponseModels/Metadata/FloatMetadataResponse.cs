using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record FloatMetadataResponse(double? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Float;
    }
}