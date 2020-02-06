namespace Digizuite.Models.Metadata.Fields
{
    public class IntMetafield : Field<int?>
    {
        public MetaFieldDataType Type => MetaFieldDataType.Int;

        public override string ToSingleString(string separator)
        {
            return Value?.ToString() ?? "";
        }
    }
}