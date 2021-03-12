namespace Digizuite.Models.Folders
{
    public record FolderValue : FolderValueReference
    {
        public string Label { get; set; } = default!;
        public int ItemId { get; set; } = default!;
        public int GroupId { get; set; } = default!;
    }
}