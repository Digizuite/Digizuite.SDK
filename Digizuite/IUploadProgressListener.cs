using System.Threading.Tasks;

namespace Digizuite
{
    public interface IUploadProgressListener
    {
        Task UploadInitiated(int itemId);
        Task ChunkUploaded(int itemId, long totalUploaded);
        Task FinishedUpload(int itemId);
    }
}