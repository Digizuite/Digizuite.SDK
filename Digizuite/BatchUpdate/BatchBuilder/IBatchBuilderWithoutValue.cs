namespace Digizuite.BatchUpdate.BatchBuilder
{
    public interface IBatchBuilderWithoutValue : IBatchBuilderValues
    {
        ICompletedBatchBuilder EmptyValues();
    }
}