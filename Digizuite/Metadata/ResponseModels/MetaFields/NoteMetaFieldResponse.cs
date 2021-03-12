using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.MetaFields
{
    public record NoteMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Note;
        public bool ShowRichTextEditor { get; set; }
    }
}