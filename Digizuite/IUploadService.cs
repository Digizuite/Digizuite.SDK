using System.IO;
using System.Threading.Tasks;

namespace Digizuite
{
    public interface IUploadService
    {
        Task<int> Upload(Stream stream, string filename, string computerName, IUploadProgressListener listener);
        Task<int> Replace(Stream stream, string filename, string computerName, int targetAssetId,
            bool keepMetadata, bool overwrite, IUploadProgressListener listener);
    }
}