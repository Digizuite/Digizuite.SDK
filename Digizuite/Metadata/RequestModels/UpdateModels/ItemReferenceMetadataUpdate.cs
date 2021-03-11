using System.Collections.Generic;
using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record ItemReferenceMetadataUpdate : MetadataUpdate
    {
        public HashSet<int> ItemIds { get; set; } = new();
    }
}