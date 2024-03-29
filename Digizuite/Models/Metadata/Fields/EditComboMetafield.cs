using Digizuite.Models.Metadata.Values;

namespace Digizuite.Models.Metadata.Fields
{
    public record EditComboMetafield : Field<ComboValue?>
    {
        public MetaFieldDataType Type => MetaFieldDataType.EditComboValue;

        public override string ToSingleString(string separator)
        {
            return Value?.Label ?? "";
        }
    }
}