namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record EditComboValueMetadataUpdate : MetadataUpdate
    {
        public string? ComboValue { get; set; } = "";
    }
}