namespace Digizuite.BatchUpdate.BatchBuilder
{
    public interface INewBatchBuilder
    {
        ITargetBatchBuilder Values();
        ITargetBatchBuilder Delete();
    }
}