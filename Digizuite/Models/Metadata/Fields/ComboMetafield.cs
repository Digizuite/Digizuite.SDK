using Digizuite.Metadata.ResponseModels;
using ComboValue = Digizuite.Models.Metadata.Values.ComboValue;

namespace Digizuite.Models.Metadata.Fields
{
    public record ComboMetafield : Field<ComboValue?>
    {
        public ComboValueViewType ViewType;
        public MetaFieldDataType Type => MetaFieldDataType.ComboValue;

        public override string ToSingleString(string separator)
        {
            return Value?.Label ?? "";
        }
    }
}