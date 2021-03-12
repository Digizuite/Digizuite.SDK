namespace Digizuite.Models.Metadata.Fields
{
    public record NoteMetafield : Field<string>
    {
        public bool IsHtml { get; set; }

        public override string ToSingleString(string separator)
        {
            return Value;
        }
    }
}