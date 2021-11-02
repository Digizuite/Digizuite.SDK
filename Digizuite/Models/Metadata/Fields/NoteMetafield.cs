namespace Digizuite.Models.Metadata.Fields
{
    public record NoteMetafield : Field<string>
    {
        public int MaxLength { get; set; }

        public override string ToSingleString(string separator)
        {
            return Value;
        }
    }
}