using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.BatchUpdate.Models;

namespace Digizuite.BatchUpdate.BatchBuilder
{
    public class BatchBuilder : INewBatchBuilder, ITargetBatchBuilder, IBatchBuilderWithoutValue, IApplyableBatchBuilder, IBatchBuilderWithoutUpdateTarget
    {
        private readonly Batch _batch = new Batch();
        private readonly IBatchUpdateClient _batchUpdateClient;
        private BatchPart _currentBatchPart = new BatchPart();


        public BatchBuilder(IBatchUpdateClient batchUpdateClient)
        {
            _batchUpdateClient = batchUpdateClient;
            _batch.AddValues(_currentBatchPart);
        }

        public INewBatchBuilder NewPart()
        {
            _currentBatchPart = new BatchPart();
            _batch.AddValues(_currentBatchPart);
            return this;
        }

        public Task<List<BatchUpdateResponse>> Apply(bool useVersionedMetadata = false)
        {
            return _batchUpdateClient.ApplyBatch(_batch, useVersionedMetadata);
        }

        public ICompletedBatchBuilder EmptyValues()
        {
            _currentBatchPart.Values.Add(new IntBatchValue("empty", 0, null));
            return this;
        }

        public IApplyableBatchBuilder WithValue(string fieldName, string value, IBatchProperties? properties = null)
        {
            _currentBatchPart.Values.Add(new StringBatchValue(fieldName, value, properties));
            return this;
        }

        public IApplyableBatchBuilder WithValue(string fieldName, bool value, IBatchProperties? properties = null)
        {
            _currentBatchPart.Values.Add(new BoolBatchValue(fieldName, value, properties));
            return this;
        }

        public IApplyableBatchBuilder WithValue(string fieldName, int value, IBatchProperties? properties = null)
        {
            _currentBatchPart.Values.Add(new IntBatchValue(fieldName, value, properties));
            return this;
        }

        public IApplyableBatchBuilder WithValue(string fieldName, IEnumerable<string> value, IBatchProperties? properties = null)
        {
            _currentBatchPart.Values.Add(new StringListBatchValue(fieldName, value.ToList(), properties));
            return this;
        }

        public IApplyableBatchBuilder WithValue(string fieldName, IEnumerable<int> value, IBatchProperties? properties = null)
        {
            _currentBatchPart.Values.Add(new IntListBatchValue(fieldName, value.ToList(), properties));
            return this;
        }

        public ITargetBatchBuilder Values()
        {
            _currentBatchPart.BatchType = BatchType.Values;
            return this;
        }

        public ITargetBatchBuilder Delete()
        {
            _currentBatchPart.BatchType = BatchType.Delete;
            return this;
        }

        public IBatchBuilderWithoutUpdateTarget Target(string target)
        {
            _currentBatchPart.Target = target;
            return this;
        }

        public IBatchBuilderWithoutValue BaseIds(IEnumerable<int> baseIds)
        {
            _currentBatchPart.BaseIds = baseIds.ToList();
            return this;
        }

        public IBatchBuilderWithoutValue ItemIds(IEnumerable<int> itemIds)
        {
            _currentBatchPart.ItemIds = itemIds.ToList();
            return this;
        }

        public IBatchBuilderWithoutValue WithoutUpdateTarget()
        {
            return this;
        }
    }
}