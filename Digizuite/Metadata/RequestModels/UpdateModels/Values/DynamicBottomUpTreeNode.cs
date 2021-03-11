namespace Digizuite.Metadata.RequestModels.UpdateModels.Values
{
    public record DynamicBottomUpTreeNode : DynamicTreeNode
    {
        public BaseTreeNodeUpdate? Parent { get; set; }
    }
}