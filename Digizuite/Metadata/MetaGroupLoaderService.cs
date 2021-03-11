using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Models;
using Digizuite.Models.Metadata;
using RestSharp;

namespace Digizuite.Metadata
{
    public class MetaGroupLoaderService : IMetaGroupLoaderService
    {
        private readonly IDamAuthenticationService _authenticationService;
        private readonly IDamRestClient _restClient;
        private readonly ILogger<MetaGroupLoaderService> _logger;

        public MetaGroupLoaderService(IDamAuthenticationService authenticationService,
            ILogger<MetaGroupLoaderService> logger,  IDamRestClient restClient)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _restClient = restClient;
        }

        public async Task<List<SystemToolsNodeItem>> GetMetaGroupInGroup(string hierarchyId = "/",
            CancellationToken cancellationToken = default)
        {
            var request = new RestRequest("dmm3bwsv3/SearchService.js", Method.GET, DataFormat.Json);
            var ak = await _authenticationService.GetAccessKey().ConfigureAwait(false);
            var memberId = await _authenticationService.GetMemberId().ConfigureAwait(false);
            request
                .AddParameter("hid", hierarchyId)
                .AddParameter("subRepositoryType", 65)
                .AddParameter("method", "GetSystemToolsTree")
                .AddParameter("memberId", memberId)
                .AddParameter("page", 1)
                .AddParameter("limit", 9999)
                .AddParameter("prevRef", "");

            var res = await _restClient.Execute<DigiResponse<SystemToolsNodeItem>>(Method.GET, request, ak, cancellationToken).ConfigureAwait(false);

            return res.Data.Items;
        }

        public async Task<RootSystemToolsResponse> GetRootMetaGroups(CancellationToken cancellationToken = default)
        {
            var request = new RestRequest("dmm3bwsv3/SearchService.js");

            var ak = await _authenticationService.GetAccessKey().ConfigureAwait(false);
            var memberId = await _authenticationService.GetMemberId().ConfigureAwait(false);
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
                _logger.LogError("Requesting root groups failed", "response", res);
                throw new Exception("Requesting root groups failed");
            }

            _logger.LogDebug("Requested root groups");
            return res.Data.Items.Single(item => item.SubRepositoryType == CodedFolder.Admin_MetaGroup);
        }

        public async Task<Tuple<List<MetaFieldGroup>, List<MetaFieldGroupFolder>>> GetAllMetaDataGroups()
        {
            var groups = new List<MetaFieldGroup>();
            var folders = new List<MetaFieldGroupFolder>();

            _logger.LogInformation("Loading meta groups");

            var root = await GetRootMetaGroups().ConfigureAwait(false);

            var remainingSearchIds = new Stack<GroupSearch>();
            foreach (var item in root.Items)
                AddResponseItem(item, remainingSearchIds, new GroupSearch
                {
                    Hid = "/",
                    ParentPath = "",
                    FolderId = 0
                }, folders, groups);

            while (remainingSearchIds.Count != 0)
            {
                var next = remainingSearchIds.Pop();

                _logger.LogDebug("Loading next group", nameof(next), next);

                var res = await GetMetaGroupInGroup(next.Hid).ConfigureAwait(false);

                foreach (var group in res) AddResponseItem(group, remainingSearchIds, next, folders, groups);
            }

            _logger.LogInformation("Finished loading meta groups");

            return new Tuple<List<MetaFieldGroup>, List<MetaFieldGroupFolder>>(groups, folders);
        }

        private void AddResponseItem(SystemToolsNodeItem group, Stack<GroupSearch> remainingSearchIds,
            GroupSearch next, List<MetaFieldGroupFolder> folders,
            List<MetaFieldGroup> groups)
        {
            if (group.SubRepositoryType == CodedFolder.Admin_MetaGroup)
            {
                _logger.LogDebug("Found a MetaGroup group", nameof(group), group);
                remainingSearchIds.Push(new GroupSearch
                {
                    Hid = group.Hid,
                    ParentPath = string.IsNullOrWhiteSpace(next.ParentPath)
                        ? group.Name
                        : $"{next.ParentPath} -> {group.Name}",
                    FolderId = group.FolderId
                });
                folders.Add(new MetaFieldGroupFolder
                {
                    Name = group.Name,
                    FolderId = group.FolderId,
                    ParentFolderId = next.FolderId
                });
            }
            else if (group.SubRepositoryType == CodedFolder.Admin_MetaField)
            {
                _logger.LogDebug("Found a MetaField group", nameof(group), group);
                groups.Add(new MetaFieldGroup
                {
                    Name = group.Name,
                    Path = next.ParentPath,
                    GroupId = group.MetafieldGroupId,
                    ParentFolderId = next.FolderId
                });
            }
            else
            {
                Console.WriteLine($"Unknown group type: {group.SubRepositoryType}");
                _logger.LogWarning($"Don't know how to handle group type: {group.SubRepositoryType}", group);
            }
        }


        private class GroupSearch
        {
            public int FolderId;
            public string Hid;
            public string ParentPath;
        }
    }
}