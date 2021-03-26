using System.Threading;
using System.Threading.Tasks;

namespace Digizuite
{
    public interface IUploadProgressListener
    {
        Task UploadInitiated(int itemId, CancellationToken cancellationToken = default);
        Task ChunkUploaded(int itemId, long totalUploaded, CancellationToken cancellationToken = default);
        Task FinishedUpload(int itemId, CancellationToken cancellationToken = default);
    }
}