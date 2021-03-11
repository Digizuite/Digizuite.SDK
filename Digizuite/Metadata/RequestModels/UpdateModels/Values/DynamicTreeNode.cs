namespace Digizuite.Metadata.RequestModels.UpdateModels.Values
{
    public abstract record DynamicTreeNode : BaseTreeNodeUpdate
    {
        public string Label { get; set; } = null!;
        public string OptionValue { get; set; } = null!;
    }
}
