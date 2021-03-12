using System;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.Cache;
using Digizuite.Exceptions;
using Digizuite.Logging;
using Digizuite.Models;
using Digizuite.Models.Search;

namespace Digizuite
{
    public class ItemService : IItemService
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<ItemService> _logger;
        private readonly ICache<ItemInfo> _cache;

        public ItemService(ILogger<ItemService> logger, ISearchService searchService, ICache<ItemInfo> cache)
        {
            _logger = logger;
            _searchService = searchService;
            _cache = cache;
        }

        public async Task<ItemInfo> GetItemInfo(Guid itemGuid)
        {
            _logger.LogDebug("Getting item info", nameof(itemGuid), itemGuid);
            return await InnerGetItemInfo(itemGuid.ToString(), new SearchParameters("DigiZuite_System_Item", 1, 25) {{"sItemGuid", $"{itemGuid}"}}).ConfigureAwait(false);
        }
        public async Task<ItemInfo> GetItemInfo(int itemId)
        {
            _logger.LogDebug("Getting item info", nameof(itemId), itemId);
            return await InnerGetItemInfo($"{itemId}", new SearchParameters("DigiZuite_System_Item", 1, 25) {{"sItemId", itemId}}).ConfigureAwait(false);
        }
        public async Task<ItemInfo> GetItemInfoFromBaseId(ItemType itemType, int baseId)
        {
            _logger.LogDebug("Getting item info", nameof(ItemType), itemType, nameof(baseId), baseId);
            return await InnerGetItemInfo($"{itemType}_{baseId}", new SearchParameters("DigiZuite_System_Item", 1, 25) {{"sItemType", (int)itemType}, {"sBaseId",baseId}}).ConfigureAwait(false);
        }
        private async Task<ItemInfo> InnerGetItemInfo(string cacheKey, SearchParameters parameters)
        {
            return await _cache.Get(cacheKey, TimeSpan.FromMinutes(60), async () =>
            {
                _logger.LogDebug("Getting item info", "cacheKey", cacheKey);
                var response = await _searchService.Search<ItemInfo>(parameters).ConfigureAwait(false);
                if (response.Items.Any())
                {
                    _logger.LogDebug("Got item info", "cacheKey", cacheKey, "info", response.Items[0]);
                    return response.Items[0];
                }
                _logger.LogError("GetItemInfo request failed", "cacheKey", cacheKey);
                throw new ItemNotFoundException("GetItemInfo request failed");
            }).ConfigureAwait(false);
        }
    }
}
