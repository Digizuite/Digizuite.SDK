namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record FloatMetadataUpdate : MetadataUpdate
    {
        public double? Value { get; set; } = null;
    }
}