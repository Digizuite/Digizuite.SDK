using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Metadata.RequestModels;
using Digizuite.Metadata.RequestModels.UpdateModels;
using Digizuite.Metadata.ResponseModels;
using Digizuite.Models.Metadata.Fields;

namespace Digizuite.Metadata
{
    public interface IMetadataValueService
    {
        /// <summary>
        ///     Gets all the metadata directly from the api, without any advanced parsing
        /// </summary>
        /// <param name="requestBody">What to query for. If an "empty" object is passed in, you get a response equivalent to a metadata editor</param>
        /// <param name="cancellationToken">A cancellationtoken to cancel the request</param>
        /// <param name="accessKey">If another access key than the default should be used</param>
        /// <returns></returns>
        Task<MetadataEditorResponse> GetRawMetadata(GetMetadataRequest requestBody, CancellationToken cancellationToken = default, string? accessKey = null);

        /// <summary>
        ///     Updates the value of all the specified fields for the given asset.
        ///     Does NOT change the definition of the field itself
        /// </summary>
        /// <param name="assetItemId">The item id of the asset to update</param>
        /// <param name="cancellationToken"></param>
        /// <param name="fields">All the fields with their values</param>
        Task UpdateFields(IEnumerable<int> assetItemId, CancellationToken cancellationToken = default,
            params Field[] fields);

        /// <summary>
        /// Applies the given metadata updates
        /// </summary>
        /// <param name="updates">The updates to apply</param>
        /// <param name="cancellationToken"></param>
        Task ApplyUpdate(IEnumerable<MetadataUpdate> updates, CancellationToken cancellationToken = default);
    }
}