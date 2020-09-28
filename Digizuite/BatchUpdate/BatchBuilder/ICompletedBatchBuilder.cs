using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.BatchUpdate.Models;

namespace Digizuite.BatchUpdate.BatchBuilder
{
    public interface ICompletedBatchBuilder
    {
        Task<List<BatchUpdateResponse>> Apply(bool useVersionedMetadata = false);
        INewBatchBuilder NewPart();
    }
}