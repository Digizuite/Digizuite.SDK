using System.Collections.Generic;
using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record EditMultiComboValueMetadataUpdate : MetadataUpdate
    {
        public List<string> ComboValues { get; set; } = new();
    }
}