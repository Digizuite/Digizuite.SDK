namespace Digizuite.Models.Metadata
{
    public class MetaFieldGroup
    {
        public int GroupId { get; set; } = default!;
        public int ParentFolderId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Path { get; set; } = default!;
    }
}