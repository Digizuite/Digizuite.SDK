using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Models;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata
{
    public class MetaGroupLoaderService : IMetaGroupLoaderService
    {
        private readonly IDamAuthenticationService _authenticationService;
        private readonly ServiceHttpWrapper _serviceHttpWrapper;
        private readonly ILogger<MetaGroupLoaderService> _logger;

        public MetaGroupLoaderService(IDamAuthenticationService authenticationService,
            ILogger<MetaGroupLoaderService> logger, ServiceHttpWrapper serviceHttpWrapper)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _serviceHttpWrapper = serviceHttpWrapper;
        }

        public async Task<List<SystemToolsNodeItem>> GetMetaGroupInGroup(string hierarchyId = "/",
            CancellationToken cancellationToken = default)
        {
            var (client, request) = _serviceHttpWrapper.GetSearchServiceClient();
            
            var ak = await _authenticationService.GetAccessKey().ConfigureAwait(false);
            var memberId = await _authenticationService.GetMemberId().ConfigureAwait(false);
            request
                .AddQueryParameter("hid", hierarchyId)
                .AddQueryParameter("subRepositoryType", 65)
                .AddQueryParameter("method", "GetSystemToolsTree")
                .AddQueryParameter("memberId", memberId)
                .AddQueryParameter("page", 1)
                .AddQueryParameter("limit", 9999)
                .AddQueryParameter("prevRef", "")
                .AddAccessKey(ak);

            var res = await client.GetAsync<DigiResponse<SystemToolsNodeItem>>(request, cancellationToken).ConfigureAwait(false);

            return res.Data!.Items;
        }

        public async Task<RootSystemToolsResponse> GetRootMetaGroups(CancellationToken cancellationToken = default)
        {
            var (client, request) = _serviceHttpWrapper.GetSearchServiceClient();

            var ak = await _authenticationService.GetAccessKey().ConfigureAwait(false);
            var memberId = await _authenticationService.GetMemberId().ConfigureAwait(false);
            request
                .AddQueryParameter("method", "GetSystemToolsTree")
                .AddQueryParameter("memberId", memberId)
                .AddQueryParameter("page", 1)
                .AddQueryParameter("limit", 9999)
                .AddQueryParameter("node", "root")
                .AddAccessKey(ak);

            _logger.LogDebug("Requesting root groups");
            var res = await client.GetAsync<DigiResponse<RootSystemToolsResponse>>( request,  cancellationToken).ConfigureAwait(false);

            if (!res.Data!.Success)
            {
                _logger.LogError("Requesting root groups failed", "response", res);
                throw new Exception("Requesting root groups failed");
            }

            _logger.LogDebug("Requested root groups");
            return res.Data!.Items.Single(item => item.SubRepositoryType == CodedFolder.Admin_MetaGroup);
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
                _logger.LogWarning($"Don't know how to handle group type: {group.SubRepositoryType}", group);
            }
        }


        private class GroupSearch
        {
            public int FolderId = default!;
            public string Hid = default!;
            public string ParentPath = default!;
        }
    }
}
