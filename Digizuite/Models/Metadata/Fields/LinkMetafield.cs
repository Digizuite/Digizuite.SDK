namespace Digizuite.Models.Metadata.Fields
{
    public record LinkMetafield : Field<string?>
    {
        public MetaFieldDataType Type => MetaFieldDataType.Link;

        public override string? ToSingleString(string separator)
        {
            return Value;
        }
    }
}