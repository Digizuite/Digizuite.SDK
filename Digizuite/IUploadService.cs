using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Digizuite
{
    public interface IUploadService
    {
        /// <summary>
        /// Uploads a new asset
        /// </summary>
        /// <param name="stream">A stream to read the asset content from</param>
        /// <param name="filename">The name of the file to upload</param>
        /// <param name="computerName">Should be the name of the service uploading this file, e.g., "Digizuite Media Manager"</param>
        /// <param name="listener">An optional listener for hooking in as the upload moves along. Do any custom operations you might have to do here</param>
        /// <param name="cancellationToken">A cancellation token for cancelling the requests if they are no longer relevant</param>
        /// <returns>The itemId of the newly uploaded asset</returns>
        Task<int> Upload(Stream stream, string filename, string computerName, IUploadProgressListener? listener = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces an existing asset
        /// </summary>
        /// <param name="stream">A stream to read the asset content from</param>
        /// <param name="filename">The name of the file to upload</param>
        /// <param name="computerName">Should be the name of the service uploading this file, e.g., "Digizuite Media Manager"</param>
        /// <param name="targetAssetId">The asset id of the asset to replace</param>
        /// <param name="keepMetadata">If metadata should be kept or replace</param>
        /// <param name="overwrite">How the asset history should be considered</param>
        /// <param name="listener">An optional listener for hooking in as the upload moves along. Do any custom operations you might have to do here</param>
        /// <param name="cancellationToken">A cancellation token for canceling the requests if they are no longer relevant</param>
        /// <returns>The itemId of the newly uploaded asset</returns>
        Task<int> Replace(Stream stream, string filename, string computerName, int targetAssetId,
            KeepMetadata keepMetadata, Overwrite overwrite, IUploadProgressListener? listener = null, CancellationToken cancellationToken = default);
    }

    #pragma warning disable CA1717
    public enum KeepMetadata
    {
        /// <summary>
        /// Indicates that existing metadata on the asset should be kept
        /// </summary>
        Keep,
        /// <summary>
        /// Indicates that the metadata on the asset should be reset as if the upload was a completely new asset
        /// </summary>
        Reset
    }
    #pragma warning restore CA1717

    public enum Overwrite
    {
        /// <summary>
        /// The new file will appear as a new entry in the asset history
        /// </summary>
        AddHistoryEntry,
        /// <summary>
        /// The last entry in the asset history will be overwritten with this entry
        /// </summary>
        ReplaceHistoryEntry
    }
}