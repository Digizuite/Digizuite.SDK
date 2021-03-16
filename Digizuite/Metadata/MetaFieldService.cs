﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Extensions;
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

            var response = await client.ExecuteGetAsync<List<MetaFieldResponse>>(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError(response.ErrorException, "Failed to load metafields", nameof(response.Content), response.Content, nameof(response.StatusCode), response.StatusCode);
                throw new Exception("Failed to load metafields: " + response.Content);
            }
            
            _logger.LogDebug("Loaded metafields without issues");

            return response.Data;
        }
    }
}