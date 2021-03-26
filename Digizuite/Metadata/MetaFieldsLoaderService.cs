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
    public class MetaFieldsLoaderService : IMetaFieldsLoaderService
    {
        private readonly IDamAuthenticationService _authenticationService;
        private readonly ILogger<MetaFieldsLoaderService> _logger;
        private readonly ServiceHttpWrapper _serviceHttpWrapper;

        public MetaFieldsLoaderService(IDamAuthenticationService authenticationService,
            ILogger<MetaFieldsLoaderService> logger, ServiceHttpWrapper serviceHttpWrapper)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _serviceHttpWrapper = serviceHttpWrapper;
        }

        public async Task<List<MetaField>> GetMetaFieldsInGroup(int groupId,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Loading metafields for group", nameof(groupId), groupId);

            var (client, request) = _serviceHttpWrapper.GetClientAndRequest(ServiceType.Dmm3bwsv3, "SearchService.js");

            var ak = await _authenticationService.GetAccessKey().ConfigureAwait(false);
            var memberId = await _authenticationService.GetMemberId().ConfigureAwait(false);
            request.AddQueryParameter("SearchName", "GetMetafields")
                .AddQueryParameter("memberId", memberId)
                .AddQueryParameter("page", 1)
                .AddQueryParameter("limit", 999999)
                .AddQueryParameter("metafieldGroupId", groupId)
                .AddAccessKey(ak);

            var res = await client.GetAsync<DigiResponse<MetaFieldListResponse>>(request, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug("Metafield list response", nameof(res), res);

            if (!res.IsSuccessful || !res.Data!.Success)
            {
                _logger.LogError("failed to load metafields in group", nameof(groupId), groupId);
                return new List<MetaField>();
            }

            return res.Data!.Items.Select(item => new MetaField
            {
                Name = item.Name,
                Type = item.DatatypeId,
                MetaFieldId = item.MetafieldId,
                MetaFieldGuid = item.Guid,
                MetaFieldItemId = item.ItemId,
                MetaFieldLabelId = item.MetafieldLabel.MetafieldLabelId,
                GroupId = groupId,
            }).ToList();
        }

        private record MetaFieldListResponseMetaFieldLabel
        {
            public int MetafieldLabelId { get; set; } = default!;
            public string MetafieldLabelLabel { get; set; } = default!;
        }


        private record MetaFieldListResponse
        {
            public MetaFieldDataType DatatypeId { get; set; } = default!;
            public string Guid { get; set; } = default!;
            public int ItemId { get; set; } = default!;
            public int MetafieldId { get; set; } = default!;
            public MetaFieldListResponseMetaFieldLabel MetafieldLabel { get; set; } = default!;
            public string Name { get; set; } = default!;
        }
    }
}