namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record StringMetadataUpdate : MetadataUpdate
    {
        public string? Value { get; set; } = "";
    }
}