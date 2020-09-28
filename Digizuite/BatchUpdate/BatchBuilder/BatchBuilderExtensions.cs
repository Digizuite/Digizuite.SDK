namespace Digizuite.BatchUpdate.BatchBuilder
{
    public static class BatchBuilderExtensions
    {
        public static IBatchBuilderWithoutValue BaseIds(this IBatchBuilderWithoutUpdateTarget builder, params int[] baseIds)
        {
            return builder.BaseIds(baseIds);
        }

        public static IBatchBuilderWithoutValue ItemIds(this IBatchBuilderWithoutUpdateTarget builder, params int[] itemIds)
        {
            return builder.ItemIds(itemIds);
        }
    }
}