using Digizuite.Metadata.ResponseModels.Properties;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record ComboValueMetadataResponse(ComboValueResponse? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.ComboValue;
    }
}