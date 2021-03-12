using System.Globalization;

namespace Digizuite.Models.Metadata.Fields
{
    public record FloatMetafield : Field<double?>
    {
        public MetaFieldDataType Type => MetaFieldDataType.Float;

        public override string ToSingleString(string separator)
        {
            return Value?.ToString(CultureInfo.InvariantCulture) ?? "";
        }
    }
}