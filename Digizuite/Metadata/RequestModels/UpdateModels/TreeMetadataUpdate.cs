using Digizuite.Collections;
using Digizuite.Metadata.RequestModels.UpdateModels.Values;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public record TreeMetadataUpdate : MultiMetadataUpdate
    {
        public ValueList<BaseTreeNodeUpdate> TreeValues { get; set; } = new();
    }
}
