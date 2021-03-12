namespace Digizuite.Test.TestModels
{
    public record GetAssetsResponse
    {
        public int ItemId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int AssetId { get; set; } = default!;
    }
}