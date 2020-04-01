namespace Digizuite.Models.Metadata
{
    public class MetaFieldGroup
    {
        public int GroupId { get; set; }
        public int ParentFolderId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class MetaFieldGroupFolder
    {
        public int FolderId { get; set; }
        public string Name { get; set; }
        public int ParentFolderId { get; set; }
    }
}