using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Metadata.ResponseModels;
using Digizuite.Metadata.ResponseModels.MetaFields;

namespace Digizuite.Metadata
{
    public class MetaFieldService : IMetaFieldService
    {
        private ILogger<MetaFieldService> _logger;
        private ServiceHttpWrapper _serviceHttpWrapper;
        private IDamAuthenticationService _damAuthenticationService;

        public MetaFieldService(ILogger<MetaFieldService> logger, ServiceHttpWrapper serviceHttpWrapper,
            IDamAuthenticationService damAuthenticationService)
        {
            _logger = logger;
            _serviceHttpWrapper = serviceHttpWrapper;
            _damAuthenticationService = damAuthenticationService;
        }

        private MetaFieldResponse CopyMetaFieldResponse(MetaFieldResponse metaFieldResponse, MetaFieldLabelResponse label)
        {
            return metaFieldResponse with
            {
#pragma warning disable 612
                LabelId = label.LabelId,
                LanguageId = label.LanguageId,
                Label = label.Label,
#pragma warning restore 612
            };
        }

        public async Task<List<MetaFieldResponse>> GetAllMetaFields(string? accessKey = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Loading metafields");
            accessKey ??= await _damAuthenticationService.GetAccessKey();

            var (client, request) =
                _serviceHttpWrapper.GetClientAndRequest(ServiceType.LegacyService, "/api/metafield");

            request.AddAccessKey(accessKey);

            var response = await client.GetAsync<List<MetaFieldResponse>>(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to load metafields", nameof(response), response);
                throw new Exception("Failed to load metafields: " + response);
            }

            var result = new List<MetaFieldResponse>();

            foreach (var field in response.Data!)
            {
                if (field.Labels.Any())
                {
                    result.AddRange(field.Labels.Select(l => CopyMetaFieldResponse(field, l.Value)));
                }
                else
                {
                    result.Add(field);
                }
            }

            _logger.LogDebug("Loaded metafields without issues");

            return result;
        }
    }
}