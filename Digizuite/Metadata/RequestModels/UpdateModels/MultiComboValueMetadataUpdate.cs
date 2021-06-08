using System.Collections.Generic;
using Digizuite.Metadata.RequestModels.UpdateModels.Values;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record MultiComboValueMetadataUpdate : MultiMetadataUpdate
    {
        public List<BaseInputCombo> ComboValues { get; set; } = new();
    }
}
