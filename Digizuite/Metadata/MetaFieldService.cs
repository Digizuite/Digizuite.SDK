using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Metadata.ResponseModels;

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

            var languageId = await _damAuthenticationService.GetLanguageId();

            foreach (var field in response.Data!)
            {
                var label = field.Labels.SingleOrDefault(l => l.Value.LanguageId == languageId);
#pragma warning disable 612
                field.LabelId = label.Value?.LabelId ?? field.LabelId;
                field.LanguageId = label.Value?.LanguageId ?? field.LanguageId;
                field.Label = label.Value?.Label ?? field.Label;
#pragma warning restore 612
            }

            _logger.LogDebug("Loaded metafields without issues");

            return response.Data!;
        }
    }
}