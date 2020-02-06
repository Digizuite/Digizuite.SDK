namespace Digizuite.Models.Metadata.Fields
{
    public class NoteMetafield : Field<string>
    {
        public bool IsHtml { get; set; }

        public override string ToSingleString(string separator)
        {
            return Value;
        }
    }
}