using System.Linq;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Models;
using RestSharp;

namespace Digizuite
{
    /// <inheritdoc />
    public class ProductService : IProductService
    {
        private readonly IDamRestClient _restClient;
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IDamRestClient restClient, IDamAuthenticationService damAuthenticationService, ILogger<ProductService> logger)
        {
            _restClient = restClient;
            _damAuthenticationService = damAuthenticationService;
            _logger = logger;
        }

        public async Task<string> GetProductItemGuidFromVersionId(string versionId, string accessKey = null)
        {
            _logger.LogDebug("GetProductItemGuidFromVersionId", nameof(versionId), versionId);
            if (string.IsNullOrWhiteSpace(accessKey))
                accessKey = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);
            
            var request = new RestRequest("MetadataService.js");
            request.AddParameter("method", "GetProductItemGuidFromVersionId");
            // ReSharper disable once StringLiteralTypo
            request.AddParameter("accesskey", accessKey);
            request.AddParameter("versionId", versionId);
            var response = await _restClient.Execute<DigiResponse<string>>(Method.POST, request, accessKey).ConfigureAwait(false);
            _logger.LogTrace("Got api response", response);

            if (!response.Data.Success)
            {
                _logger.LogError("request failed", nameof(response), response.Data, nameof(response.Content), response.Content);
                throw new ProductVersionNotFoundException($"{nameof(GetProductItemGuidFromVersionId)} did not find a Product ItemGuid for Version {versionId}");
            }
            if (!response.Data.Items.Any())
            {
                _logger.LogError("request failed", nameof(response), response.Data, nameof(response.Content), response.Content);
                throw new ProductVersionNotFoundException($"{nameof(GetProductItemGuidFromVersionId)} succeeded, but did not return a Product ItemGuid for VersionId {versionId}");
            }
            return response.Data.Items[0];
        }
    }
}
