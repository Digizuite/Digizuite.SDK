using Digizuite.BatchUpdate.Models;

namespace Digizuite.Models.Folders
{
    public record FolderValueReference
    {
        public int FolderId { get; set; }
        public RepositoryType RepositoryType { get; set; }
    }
}