using System.Globalization;

namespace Digizuite.Models.Metadata.Fields
{
    public class BitMetafield : Field<bool>
    {
        public MetaFieldDataType Type => MetaFieldDataType.Bit;

        public override string ToSingleString(string separator)
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
