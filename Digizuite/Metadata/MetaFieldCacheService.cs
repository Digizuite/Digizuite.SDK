using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.Cache;
using Digizuite.Logging;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata
{
    /// <summary>
    ///     Creates a local cache of all the metafields and groups, so they are easy to lookup in the future
    /// </summary>
    public class MetaFieldCacheService : IMetaFieldCacheService
    {
        public static readonly string MetaFieldGroupsCacheKey =
            typeof(MetaFieldCacheService).FullName + "_groups+folders";

        public static readonly string MetaFieldCacheKey = typeof(MetaFieldCacheService).FullName + "_metafields";
        private readonly ICache<Tuple<List<MetaFieldGroup>, List<MetaFieldGroupFolder>>> _groupAndFolderCache;
        private readonly ILogger<MetaFieldCacheService> _logger;
        private readonly ICache<List<MetaField>> _metaFieldCache;
        private IMetaFieldsLoaderService _fieldsLoaderService;
        private IMetaGroupLoaderService _groupLoaderService;

        public MetaFieldCacheService(ILogger<MetaFieldCacheService> logger,
            IMetaGroupLoaderService groupLoaderService, IMetaFieldsLoaderService fieldsLoaderService,
            ICache<Tuple<List<MetaFieldGroup>, List<MetaFieldGroupFolder>>> groupAndFolderCache,
            ICache<List<MetaField>> metaFieldCache)
        {
            _logger = logger;
            _groupLoaderService = groupLoaderService;
            _fieldsLoaderService = fieldsLoaderService;
            _groupAndFolderCache = groupAndFolderCache;
            _metaFieldCache = metaFieldCache;
        }

        public async Task<List<MetaFieldGroup>> GetMetaFieldGroups()
        {
            var (groups, _) = await GetMetaFieldGroupsAndFolders().ConfigureAwait(false);
            return groups;
        }

        public async Task<List<MetaFieldGroupFolder>> GetMetaFieldFolders()
        {
            var (_, folders) = await GetMetaFieldGroupsAndFolders().ConfigureAwait(false);
            return folders;
        }

        public Task<List<MetaField>> GetMetaFields()
        {
            return _metaFieldCache.Get(MetaFieldCacheKey, TimeSpan.FromMinutes(15d), GetFreshMetaFields);
        }

        private async Task<List<MetaField>> GetFreshMetaFields()
        {
            var groups = await GetMetaFieldGroups().ConfigureAwait(false);
            var result = new List<MetaField>();

            foreach (var group in groups)
            {
                _logger.LogDebug("Loading MetaField group", "path", group.Path, "name", group.Name);
                var fields = await _fieldsLoaderService.GetMetaFieldsInGroup(@group.GroupId).ConfigureAwait(false);
                result.AddRange(fields);
            }

            return result;
        }

        private Task<Tuple<List<MetaFieldGroup>, List<MetaFieldGroupFolder>>> GetMetaFieldGroupsAndFolders()
        {
            return _groupAndFolderCache.Get(MetaFieldGroupsCacheKey, TimeSpan.FromMinutes(15d),
                _groupLoaderService.GetAllMetaDataGroups);
        }

        public void Clear()
        {
            _logger.LogInformation("Clearing metafield and metagroup cache");
            _metaFieldCache.Clear(MetaFieldCacheKey);
            _groupAndFolderCache.Clear(MetaFieldGroupsCacheKey);
        }
    }
}