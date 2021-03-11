using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record EditComboValueMetadataUpdate : MetadataUpdate
    {
        public string? ComboValue { get; set; } = "";
    }
}