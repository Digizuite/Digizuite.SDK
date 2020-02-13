using System;
using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models
{
    public class Asset
    {
        public AssetType AssetType { get; set; }
        public int AssetId { get; set; }
        public int ItemId { get; set; }
        public int PrevRef { get; set; }
        public int UploadMemberId { get; set; }
        public long FileSize { get; set; }
        public string Name { get; set; }
        public bool WriteAccess { get; set; }
        public int AssetVersionId { get; set; }
        public string Thumb { get; set; }
        public string ImagePreview { get; set; }
        public string VideoPreview { get; set; }
        public DateTime ImportDate { get; set; }
        public bool IsPublic { get; set; }
        public int Published { get; set; }
        protected bool Equals(Asset other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return AssetType == other.AssetType && AssetId == other.AssetId && ItemId == other.ItemId && PrevRef == other.PrevRef && UploadMemberId == other.UploadMemberId && Name == other.Name && WriteAccess == other.WriteAccess && AssetVersionId == other.AssetVersionId && Thumb == other.Thumb && ImagePreview == other.ImagePreview && VideoPreview == other.VideoPreview;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Asset) obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) AssetType;
                hashCode = (hashCode * 397) ^ AssetId;
                hashCode = (hashCode * 397) ^ ItemId;
                hashCode = (hashCode * 397) ^ PrevRef;
                hashCode = (hashCode * 397) ^ UploadMemberId;
                hashCode = (hashCode * 397) ^ AssetVersionId;
                hashCode = (hashCode * 397) ^ Published;
                hashCode = (hashCode * 397) ^ FileSize.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ WriteAccess.GetHashCode();
                hashCode = (hashCode * 397) ^ IsPublic.GetHashCode();
                hashCode = (hashCode * 397) ^ ImportDate.GetHashCode();
                hashCode = (hashCode * 397) ^ (Thumb != null ? Thumb.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ImagePreview != null ? ImagePreview.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (VideoPreview != null ? VideoPreview.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Asset left, Asset right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Asset left, Asset right)
        {
            return !Equals(left, right);
        }
    }
}