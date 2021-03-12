using Digizuite.Metadata.ResponseModels.Properties;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record EditComboValueMetadataResponse(ComboValueResponse? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.EditComboValue;
    }
}