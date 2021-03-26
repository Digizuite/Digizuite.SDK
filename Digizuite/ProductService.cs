using System.Threading;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;

namespace Digizuite
{
    /// <inheritdoc />
    public class ProductService : IProductService
    {
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly ILogger<ProductService> _logger;
        private readonly ServiceHttpWrapper _serviceHttpWrapper;

        public ProductService( IDamAuthenticationService damAuthenticationService,
            ILogger<ProductService> logger, ServiceHttpWrapper serviceHttpWrapper)
        {
            _damAuthenticationService = damAuthenticationService;
            _logger = logger;
            _serviceHttpWrapper = serviceHttpWrapper;
        }

        public async Task<string> GetProductItemGuidFromVersionId(string versionId, string? accessKey = null,
            CancellationToken cancellationToken = default)
        {
            var (client, request) =
                _serviceHttpWrapper.GetClientAndRequest(ServiceType.Dmm3bwsv3, "MetadataService.js");
            
            _logger.LogDebug("GetProductItemGuidFromVersionId", nameof(versionId), versionId);
            accessKey ??= await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);

            request
                .AddQueryParameter("method", "GetProductItemGuidFromVersionId")
                .AddQueryParameter("versionId", versionId)
                .AddAccessKey(accessKey);
            
            var response = await client.PostAsync<DigiSingleResult<string>>(request, cancellationToken)
                .ConfigureAwait(false);
            _logger.LogTrace("Got api response", response);

            if (!response.Data!.Success)
            {
                _logger.LogError("request failed", nameof(response), response);
                throw new ProductVersionNotFoundException(
                    $"Did not find a Product ItemGuid for Version {versionId}");
            }

            return response.Data!.Result;
        }

        private class DigiSingleResult<T>
        {
            public bool Success { get; } = default!;
            public T Result { get; } = default!;
            public object Errors { get; set; } = default!;
        }
    }
}