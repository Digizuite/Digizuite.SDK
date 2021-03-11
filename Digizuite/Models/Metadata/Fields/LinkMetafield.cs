namespace Digizuite.Models.Metadata.Fields
{
    public class LinkMetafield : Field<string?>
    {
        public MetaFieldDataType Type => MetaFieldDataType.Link;

        public override string? ToSingleString(string separator)
        {
            return Value;
        }
    }
}