using System.Collections.Generic;
using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;
using Digizuite.Metadata.RequestModels.UpdateModels.Values;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record MultiComboValueMetadataUpdate : MetadataUpdate
    {
        public List<BaseInputCombo> ComboValues { get; set; } = new();
    }
}
