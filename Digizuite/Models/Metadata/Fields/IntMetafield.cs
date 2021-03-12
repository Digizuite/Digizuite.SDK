using System.Globalization;

namespace Digizuite.Models.Metadata.Fields
{
    public record IntMetafield : Field<int?>
    {
        public MetaFieldDataType Type => MetaFieldDataType.Int;

        public override string ToSingleString(string separator)
        {
            return Value?.ToString(CultureInfo.InvariantCulture) ?? "";
        }
    }
}