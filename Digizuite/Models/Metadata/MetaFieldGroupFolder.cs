namespace Digizuite.Models.Metadata
{
    public class MetaFieldGroupFolder
    {
        public int FolderId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int ParentFolderId { get; set; } = default!;
    }
}