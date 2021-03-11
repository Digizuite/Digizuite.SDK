using Digizuite.Collections;

namespace Digizuite.Metadata.RequestModels.UpdateModels.Values
{
    public record DynamicTopDownTreeNode : DynamicTreeNode
    {
        public ValueList<DynamicTopDownTreeNode> Children { get; set; } = new();
    }
}
