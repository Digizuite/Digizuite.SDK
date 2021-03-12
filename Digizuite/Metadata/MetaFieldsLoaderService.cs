using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Models;
using Digizuite.Models.Metadata;
using RestSharp;

namespace Digizuite.Metadata
{
    public class MetaFieldsLoaderService : IMetaFieldsLoaderService
    {
        private IDamAuthenticationService _authenticationService;
        private readonly IDamRestClient _restClient;
        private ILogger<MetaFieldsLoaderService> _logger;

        public MetaFieldsLoaderService(IDamAuthenticationService authenticationService, IDamRestClient restClient,
            ILogger<MetaFieldsLoaderService> logger)
        {
            _authenticationService = authenticationService;
            _restClient = restClient;
            _logger = logger;
        }

        public async Task<List<MetaField>> GetMetaFieldsInGroup(int groupId,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Loading metafields for group", nameof(groupId), groupId);

            var request = new RestRequest("dmm3bwsv3/SearchService.js", Method.GET, DataFormat.Json);
            var ak = await _authenticationService.GetAccessKey().ConfigureAwait(false);
            var memberId = await _authenticationService.GetMemberId().ConfigureAwait(false);
            request.AddParameter("SearchName", "GetMetafields")
                .AddParameter("memberId", memberId)
                .AddParameter("page", 1)
                .AddParameter("limit", 999999)
                .AddParameter("metafieldGroupId", groupId)
                .AddParameter("accessKey", ak);

            var res = await _restClient.Execute<DigiResponse<MetaFieldListResponse>>(Method.GET, request, cancellationToken: cancellationToken).ConfigureAwait(false);

            _logger.LogDebug("Metafield list response", "data", res.Data, "content", res.Content);

            if (!res.IsSuccessful || !res.Data.Success)
            {
                _logger.LogError("failed to load metafields in group", nameof(groupId), groupId);
                return new List<MetaField>();
            }

            return res.Data.Items.Select(item => new MetaField
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