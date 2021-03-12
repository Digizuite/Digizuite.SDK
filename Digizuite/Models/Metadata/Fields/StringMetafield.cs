namespace Digizuite.Models.Metadata.Fields
{
    public record StringMetafield : Field<string>
    {
        public MetaFieldDataType Type => MetaFieldDataType.String;
        public int MaxLength { get; set; }

        public override string ToSingleString(string separator)
        {
            return Value ?? "";
        }
    }
}