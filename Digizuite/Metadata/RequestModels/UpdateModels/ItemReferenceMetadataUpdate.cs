using System.Collections.Generic;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record ItemReferenceMetadataUpdate : MultiMetadataUpdate
    {
        public HashSet<int> ItemIds { get; set; } = new();
    }
}