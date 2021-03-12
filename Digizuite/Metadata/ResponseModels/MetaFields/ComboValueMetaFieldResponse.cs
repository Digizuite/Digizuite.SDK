using Digizuite.Metadata.ResponseModels.Properties;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.MetaFields
{
    public record ComboValueMetaFieldResponse : MetaFieldResponse
    {
        public ComboValueViewType ViewType { get; set; }
        public override MetaFieldDataType Type => MetaFieldDataType.ComboValue;
    }
}