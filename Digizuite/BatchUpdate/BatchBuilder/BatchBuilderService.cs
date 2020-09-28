namespace Digizuite.BatchUpdate.BatchBuilder
{
    public class BatchBuilderService : IBatchBuilderService
    {
        private readonly IBatchUpdateClient _batchUpdateClient;

        public BatchBuilderService(IBatchUpdateClient batchUpdateClient)
        {
            _batchUpdateClient = batchUpdateClient;
        }

        public INewBatchBuilder CreateBatch()
        {
            return new BatchBuilder(_batchUpdateClient);
        }
    }
}