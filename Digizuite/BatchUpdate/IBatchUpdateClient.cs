using System.Threading.Tasks;
using Digizuite.BatchUpdate.Models;

namespace Digizuite.BatchUpdate
{
    public interface IBatchUpdateClient
    {
        Task ApplyBatch(Batch batch,
            bool useVersionedMetadata = false);
    }
}