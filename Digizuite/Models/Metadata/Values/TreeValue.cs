using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models.Metadata.Values
{
    public class TreeValue : TreeValueReference
    {
        public int Id { get; set; }
        public string Label { get; set; }

        protected bool Equals(TreeValue other)
        {
            return base.Equals(other) && Id == other.Id && string.Equals(Label, other.Label);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TreeValue)obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ Id;
                hashCode = (hashCode * 397) ^ (Label != null ? Label.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Id)}: {Id}, {nameof(Label)}: {Label}";
        }
    }
}
