using System;

namespace Digizuite.Models
{
    public record Asset
    {
        public AssetType AssetType { get; set; } = default!;
        public int AssetId { get; set; } = default!;
        public int ItemId { get; set; } = default!;
        public int PrevRef { get; set; } = default!;
        public int UploadMemberId { get; set; } = default!;
        public long FileSize { get; set; } = default!;
        public string Name { get; set; } = default!;
        public bool WriteAccess { get; set; } = default!;
        public int AssetVersionId { get; set; } = default!;
        public string Thumb { get; set; } = default!;
        public string ImagePreview { get; set; } = default!;
        public string VideoPreview { get; set; } = default!;
        public DateTime ImportDate { get; set; } = default!;
        public bool IsPublic { get; set; } = default!;
        public int Published { get; set; } = default!;
    }
}