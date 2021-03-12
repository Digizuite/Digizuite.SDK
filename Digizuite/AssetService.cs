using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.BatchUpdate;
using Digizuite.BatchUpdate.Models;
using Digizuite.Exceptions;
using Digizuite.Logging;
using Digizuite.Models;
using Digizuite.Models.Search;

namespace Digizuite
{
    /// <inheritdoc cref="IAssetService" />
    public class AssetService : IAssetService
    {
        private readonly IBatchUpdateClient _batchUpdateClient;
        private readonly ILogger<AssetService> _logger;
        private readonly ISearchService _searchService;

        public AssetService(ISearchService searchService, IBatchUpdateClient batchUpdateClient,
            ILogger<AssetService> logger)
        {
            _searchService = searchService;
            _batchUpdateClient = batchUpdateClient;
            _logger = logger;
        }

        /// <inheritdoc cref="IAssetService.GetAssetByItemId" />
        public Task<Asset> GetAssetByItemId(int itemId)
        {
            _logger.LogDebug("Loading asset by item id", nameof(itemId), itemId);
            return GetAssetUsingParameter("sItemId", itemId);
        }

        /// <inheritdoc cref="IAssetService.GetAssetByAssetId" />
        public Task<Asset> GetAssetByAssetId(int assetId)
        {
            _logger.LogDebug("Loading asset by asset id", nameof(assetId), assetId);
            return GetAssetUsingParameter("sAssetId", assetId);
        }

        /// <inheritdoc cref="IAssetService.GetAssetVersionsByItemId" />
        public Task<SearchResponse<Asset>> GetAssetVersionsByItemId(int itemId)
        {
            _logger.LogDebug("Getting asset version by item id", nameof(itemId), itemId);
            return GetAssetVersionsUsingParameter("assetItemId", itemId);
        }

        /// <inheritdoc cref="IAssetService.GetAssetVersionsByAssetId" />
        public Task<SearchResponse<Asset>> GetAssetVersionsByAssetId(int assetId)
        {
            _logger.LogDebug("Getting asset version by asset id", nameof(assetId), assetId);
            return GetAssetVersionsUsingParameter("assetId", assetId);
        }

        /// <inheritdoc cref="IAssetService.GetAssets" />
        public Task<SearchResponse<Asset>> GetAssets(SearchParameters? parameters = null)
        {
            if (parameters == null)
            {
                _logger.LogDebug("Search parameter was null, defaulting to 'GetAssets'");
                parameters = new SearchParameters("GetAssets");
            }

            _logger.LogDebug("Getting assets", nameof(parameters.Page), parameters.Page, nameof(parameters.PageSize),
                parameters.PageSize);
            return _searchService.Search<Asset>(parameters);
        }

        /// <inheritdoc cref="IAssetService.DeleteAsset" />
        public Task DeleteAsset(int itemId, AssetDeleteType deleteType,
            AcceptsConsequence accepts = AcceptsConsequence.No)
        {
            _logger.LogTrace("Validating delete is allowed", nameof(deleteType), deleteType, nameof(accepts), accepts,
                nameof(itemId), itemId);
            if (deleteType == AssetDeleteType.Hard && accepts == AcceptsConsequence.No)
                throw new ResponsibilitiesNotAcceptedException(
                    "A hard delete was attempted, but the responsibilities of doing that was not accepted");

            _logger.LogDebug("Deleting asset", nameof(itemId), itemId);
            var batch = new Batch(new BatchPart
            {
                Target = FieldType.Asset,
                ItemIds = new List<int> {itemId},
                ForceDelete = deleteType == AssetDeleteType.Hard,
                BatchType = BatchType.ItemIdsDelete,
                Values =
                {
                    new IntBatchValue(FieldType.Empty, 0, null)
                }
            });

            return _batchUpdateClient.ApplyBatch(batch);
        }

        /// <inheritdoc cref="IAssetService.CreateMetaAsset"/>
        public async Task<Asset> CreateMetaAsset(string title, int catalogFolderId)
        {
            _logger.LogDebug("Creating meta asset", nameof(title), title, nameof(catalogFolderId), catalogFolderId);
            var batch = new Batch(new BatchPart
            {
                Target = FieldType.Asset,
                BatchType = BatchType.Values,
                ItemIds = new List<int>{0},
                Values =
                {
                    new StringBatchValue(FieldType.Name, title, null),
                    new IntBatchValue(FieldType.AssetType, (int)AssetType.META, null),
                    new IntBatchValue(FieldType.MetafieldGroup, 10025, null),
                    new FolderBatchValue(FieldType.Folder, catalogFolderId, null, RepositoryType.Catalog)
                }
            });

            var response = await _batchUpdateClient.ApplyBatch(batch).ConfigureAwait(false);

            if (response.Any())
            {
                return await GetAssetByItemId(response[0].ItemId).ConfigureAwait(false);
            }
            
            throw new Exception("Batch update succeeded, but we didn't get any information about the created asset");
        }

        private async Task<Asset> GetAssetUsingParameter(string key, int value)
        {
            _logger.LogTrace("Getting asset using parameter", nameof(key), key, nameof(value), value);
            var parameters = new SearchParameters("GetAssets")
            {
                {key, value}
            };

            var response = await _searchService.Search<Asset>(parameters).ConfigureAwait(false);
            _logger.LogDebug("Got asset response");

            if (response.Items.Any()) return response.Items[0];

            _logger.LogDebug("No assets was in the response");
            throw new AssetNotFoundException("Asset not found");
        }

        private Task<SearchResponse<Asset>> GetAssetVersionsUsingParameter(string key, int value)
        {
            _logger.LogDebug("Getting asset versions using parameter", nameof(key), key, nameof(value), value);
            var parameters = new SearchParameters("GetAssets", 1, 9999) {Method = "GetAssetVersions"};
            parameters.Add(key, value);

            return GetAssets(parameters);
        }
    }
}