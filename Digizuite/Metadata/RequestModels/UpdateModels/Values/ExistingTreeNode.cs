namespace Digizuite.Metadata.RequestModels.UpdateModels.Values
{
    public record ExistingTreeNode : BaseTreeNodeUpdate
    {
        public int TreeValueId { get; set; }

        public ExistingTreeNode(int treeValueId)
        {
            TreeValueId = treeValueId;
        }
    }
}