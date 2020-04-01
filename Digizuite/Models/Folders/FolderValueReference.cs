using System.Diagnostics.CodeAnalysis;
using Digizuite.BatchUpdate.Models;

namespace Digizuite.Models.Folders
{
    public class FolderValueReference
    {
        public int FolderId { get; set; }
        public RepositoryType RepositoryType { get; set; }

        public override string ToString()
        {
            return $"{nameof(FolderId)}: {FolderId}, {nameof(RepositoryType)}: {RepositoryType}";
        }

        protected bool Equals(FolderValueReference other)
        {
            return FolderId == other?.FolderId && RepositoryType == other.RepositoryType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FolderValueReference) obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                return (FolderId * 397) ^ (int) RepositoryType;
            }
        }
    }
}