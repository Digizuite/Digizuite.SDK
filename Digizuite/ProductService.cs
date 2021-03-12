using System.Threading;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Logging;
using RestSharp;

namespace Digizuite
{
    /// <inheritdoc />
    public class ProductService : IProductService
    {
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly ILogger<ProductService> _logger;
        private readonly IDamRestClient _restClient;

        public ProductService(IDamRestClient restClient, IDamAuthenticationService damAuthenticationService,
            ILogger<ProductService> logger)
        {
            _restClient = restClient;
            _damAuthenticationService = damAuthenticationService;
            _logger = logger;
        }

        public async Task<string> GetProductItemGuidFromVersionId(string versionId, string? accessKey = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("GetProductItemGuidFromVersionId", nameof(versionId), versionId);
            if (string.IsNullOrWhiteSpace(accessKey))
                accessKey = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);

            var request = new RestRequest("MetadataService.js");
            request.AddParameter("method", "GetProductItemGuidFromVersionId");
            // ReSharper disable once StringLiteralTypo
            request.AddParameter("versionId", versionId);
            var response = await _restClient
                .Execute<DigiSingleResult<string>>(Method.POST, request, accessKey, cancellationToken)
                .ConfigureAwait(false);
            _logger.LogTrace("Got api response", response);

            if (!response.Data.Success)
            {
                _logger.LogError("request failed", nameof(response), response.Data, nameof(response.Content),
                    response.Content);
                throw new ProductVersionNotFoundException(
                    $"{nameof(GetProductItemGuidFromVersionId)} did not find a Product ItemGuid for Version {versionId}");
            }

            return response.Data.Result;
        }

        private class DigiSingleResult<T>
        {
            public bool Success { get; } = default!;
            public T Result { get; } = default!;
            public object Errors { get; set; } = default!;
        }
    }
}