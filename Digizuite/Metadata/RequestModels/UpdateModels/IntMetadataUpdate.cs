namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record IntMetadataUpdate : MetadataUpdate
    {
        public int? Value { get; set; } = null;
    }
}