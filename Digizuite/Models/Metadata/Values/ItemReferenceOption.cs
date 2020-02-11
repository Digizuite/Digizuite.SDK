using System;
using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models.Metadata.Values
{
    public class ItemReferenceOption : ItemReferenceOptionReference
    {
        public int BaseId { get; set; }
        public string Label { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(BaseId)}: {BaseId}, {nameof(Label)}: {Label}";
        }

        protected bool Equals(ItemReferenceOption other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            return base.Equals(other) && BaseId == other.BaseId && Label == other.Label;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ItemReferenceOption)obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ BaseId;
                hashCode = (hashCode * 397) ^ (Label != null ? Label.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
