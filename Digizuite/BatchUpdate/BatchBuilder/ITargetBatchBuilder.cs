namespace Digizuite.BatchUpdate.BatchBuilder
{
    public interface ITargetBatchBuilder
    {
        IBatchBuilderWithoutUpdateTarget Target(string target);
    }
}