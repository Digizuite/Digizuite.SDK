using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.BatchUpdate.Models;
using Digizuite.Extensions;
using Digizuite.Models;
using Digizuite.Models.Folders;
using Digizuite.Models.Metadata;
using RestSharp;

namespace Digizuite.Folders
{
    public class FolderService : IFolderService
    {
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly ILogger<FolderService> _logger;
        private readonly ISearchService _searchService;
        private readonly IDamRestClient _restClient;

        public FolderService(ILogger<FolderService> logger, ISearchService searchService,IDamAuthenticationService damAuthenticationService, IDamRestClient restClient)
        {
            _logger = logger;
            _searchService = searchService;
            _damAuthenticationService = damAuthenticationService;
            _restClient = restClient;
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
            var request = new RestRequest("dmm3bwsv3/SearchService.js");

            var ak = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);
            var memberId = await _damAuthenticationService.GetMemberId().ConfigureAwait(false);
            request
                .AddParameter("method", "GetSystemToolsTree")
                .AddParameter("memberId", memberId)
                .AddParameter("page", 1)
                .AddParameter("limit", 9999)
                .AddParameter("node", "root");

            _logger.LogDebug("Requesting root groups");
            var res = await _restClient.Execute<DigiResponse<RootSystemToolsResponse>>(Method.GET, request, ak, cancellationToken).ConfigureAwait(false);

            if (!res.Data.Success)
            {
                _logger.LogError("GetSystemToolsTree failed", "response", res);
                throw new Exception("GetSystemToolsTree failed");
            }

            return res.Data.Items
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