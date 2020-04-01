using System;
using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models.Folders
{
    public class FolderValue : FolderValueReference
    {
        public string Label { get; set; }
        public int ItemId { get; set; }
        public int GroupId { get; set; }


        public override string ToString()
        {
            return
                $"{base.ToString()}, {nameof(Label)}: {Label}, {nameof(ItemId)}: {ItemId}, {nameof(GroupId)}: {GroupId}";
        }

        protected bool Equals(FolderValue other)
        {
            return base.Equals(other) && string.Equals(Label, other?.Label, StringComparison.InvariantCulture) && ItemId == other?.ItemId &&
                   GroupId == other.GroupId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FolderValue) obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Label != null ? Label.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ItemId;
                hashCode = (hashCode * 397) ^ GroupId;
                return hashCode;
            }
        }
    }
}