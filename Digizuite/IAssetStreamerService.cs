using System;
using System.Threading.Tasks;

namespace Digizuite
{
    /// <summary>
    ///     Provides methods for getting a specific format of an asset
    /// </summary>
    public interface IAssetStreamerService
    {
        /// <summary>
        ///     Gets a url that can be used to stream/download the given asset in the given format
        ///     If mediaFormatId is not specified, then the source asset is downloaded
        /// </summary>
        /// <param name="assetId">The asset id of the asset</param>
        /// <param name="accessKey">An access key</param>
        /// <param name="mediaFormatId">The media format id</param>
        /// <param name="destinationId">The destination id</param>
        /// <returns>A full uri for that asset for that format</returns>
        Task<Uri> GetAssetDownloadUrl(int assetId,
            string accessKey = null, int mediaFormatId = -1, int destinationId = -1);
    }
}