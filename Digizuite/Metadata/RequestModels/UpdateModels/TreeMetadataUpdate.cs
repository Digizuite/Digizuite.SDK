using Digizuite.Collections;
using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;
using Digizuite.Metadata.RequestModels.UpdateModels.Values;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record TreeMetadataUpdate : MetadataUpdate
    {
        public ValueList<BaseTreeNodeUpdate> TreeValues { get; set; } = new();
    }
}
