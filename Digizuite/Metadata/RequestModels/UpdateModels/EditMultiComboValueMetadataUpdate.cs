using System.Collections.Generic;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record EditMultiComboValueMetadataUpdate : MultiMetadataUpdate
    {
        public List<string> ComboValues { get; set; } = new();
    }
}