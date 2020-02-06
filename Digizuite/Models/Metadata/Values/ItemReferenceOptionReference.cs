namespace Digizuite.Models.Metadata.Values
{
    public class ItemReferenceOptionReference
    {
        public int ItemId { get; set; }

        public override string ToString()
        {
            return $"{nameof(ItemId)}: {ItemId}";
        }

        protected bool Equals(ItemReferenceOptionReference other)
        {
            return ItemId == other.ItemId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ItemReferenceOptionReference)obj);
        }

        public override int GetHashCode()
        {
            return ItemId;
        }
    }
}
