using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record NoteMetadataResponse(string Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Note;
    }
}