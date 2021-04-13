using Digizuite.Metadata.RequestModels.UpdateModels.Values;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record ComboValueMetadataUpdate : MetadataUpdate
    {
        public BaseInputCombo? ComboValue { get; set; }
    }
}