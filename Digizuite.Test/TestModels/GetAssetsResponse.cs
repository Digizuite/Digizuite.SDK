namespace Digizuite.Test.TestModels
{
    public class GetAssetsResponse
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int AssetId { get; set; }

        protected bool Equals(GetAssetsResponse other)
        {
            return AssetId == other.AssetId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GetAssetsResponse) obj);
        }

        public override int GetHashCode()
        {
            return AssetId;
        }

        public static bool operator ==(GetAssetsResponse left, GetAssetsResponse right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GetAssetsResponse left, GetAssetsResponse right)
        {
            return !Equals(left, right);
        }
    }
}