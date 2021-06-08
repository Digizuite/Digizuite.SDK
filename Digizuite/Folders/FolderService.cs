using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.BatchUpdate.Models;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Models;
using Digizuite.Models.Folders;
using Digizuite.Models.Metadata;

namespace Digizuite.Folders
{
    public class FolderService : IFolderService
    {
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly ILogger<FolderService> _logger;
        private readonly ISearchService _searchService;
        private readonly ServiceHttpWrapper _serviceHttpWrapper;

        public FolderService(ILogger<FolderService> logger, ISearchService searchService,IDamAuthenticationService damAuthenticationService, ServiceHttpWrapper serviceHttpWrapper)
        {
            _logger = logger;
            _searchService = searchService;
            _damAuthenticationService = damAuthenticationService;
            _serviceHttpWrapper = serviceHttpWrapper;
        }

        public async Task<List<FolderValue>> GetFolders()
        {
            var catalogFolderTask = GetCatalogFolders();
            var portalFolderTask = GetPortalFolders();
            var memberFolderTask = GetMemberFolders();

            var responses = await Task.WhenAll(catalogFolderTask, portalFolderTask, memberFolderTask).ConfigureAwait(false);
            return responses.SelectMany(r => r).ToList();
        }

        public async Task<IEnumerable<FolderValue>> GetMemberFolders(CancellationToken cancellationToken = default)
        {
            var (client, request) = _serviceHttpWrapper.GetSearchServiceClient();

            var ak = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);
            var memberId = await _damAuthenticationService.GetMemberId().ConfigureAwait(false);
            request
                .AddQueryParameter("method", "GetSystemToolsTree")
                .AddQueryParameter("memberId", memberId)
                .AddQueryParameter("page", 1)
                .AddQueryParameter("limit", 9999)
                .AddQueryParameter("node", "root")
                .AddAccessKey(ak);

            _logger.LogDebug("Requesting root groups");
            var res = await client.GetAsync<DigiResponse<RootSystemToolsResponse>>(request, cancellationToken).ConfigureAwait(false);

            if (!res.Data!.Success)
            {
                _logger.LogError("GetSystemToolsTree failed", "response", res);
                throw new Exception("GetSystemToolsTree failed");
            }

            return res.Data!.Items
                .Where(item => item.SubRepositoryType == CodedFolder.Admin_Users_And_Groups)
                .SelectMany(item => item.Items)
                .Where(item => item.SubRepositoryType == CodedFolder.Admin_Users)
                .SelectMany(item => item.Items)
                .Where(item => item.RepositoryType == RepositoryType.BackendUsers)
                .Select(item => new FolderValue
                {
                    Label = item.Name,
                    FolderId = item.FolderId,
                    GroupId = 0,
                    ItemId = item.ItemId,
                    RepositoryType = item.RepositoryType
                });
        }

        public async Task<IEnumerable<FolderValue>> GetCatalogFolders()
        {
            var response = await _searchService.ExecuteSearch<FolderResponseItem>("GetCatalogFolders").ConfigureAwait(false);

            return HandleResponse(response, RepositoryType.Catalog);
        }

        public async Task<IEnumerable<FolderValue>> GetPortalFolders()
        {
            var response = await _searchService.ExecuteSearch<FolderResponseItem>("GetChannelFolders").ConfigureAwait(false);

            return HandleResponse(response, RepositoryType.Portal);
        }

        private IEnumerable<FolderValue> HandleResponse(IEnumerable<FolderResponseItem> response,
            RepositoryType repositoryType)
        {
            return response.Select(item => new FolderValue
            {
                Label = item.Name,
                FolderId = item.FolderId,
                ItemId = item.ItemId,
                // Id of the prev reference
                GroupId = item.PrevRef,
                RepositoryType = repositoryType
            });
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class FolderResponseItem
        {
            public int ItemId { get; set; } = default!;
            public int FolderId { get; set; } = default!;
            public int PrevRef { get; set; } = default!;
            public string Name { get; set; } = default!;
            public int ChildCount { get; set; } = default!;
            public int AssetsInFolder { get; set; } = default!;
            public int AssetsInFolderRecursive { get; set; } = default!;
            public int WriteAccess { get; set; } = default!;
        }
    }
}