using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.BatchUpdate.Models;

namespace Digizuite.BatchUpdate
{
    public interface IBatchUpdateClient
    {
        Task<List<BatchUpdateResponse>> ApplyBatch(Batch batch,
            bool useVersionedMetadata = false);
    }
}