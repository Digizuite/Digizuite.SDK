using System.Threading;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Models;
using Digizuite.Models.Search;
using RestSharp;

namespace Digizuite
{
    /// <summary>
    /// A concrete implementation of the search service
    /// </summary>
    public class SearchService : ISearchService
    {
        private readonly IDamRestClient _restClient;
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly ILogger<SearchService> _logger;

        public SearchService(IDamRestClient restClient, IDamAuthenticationService damAuthenticationService,
            ILogger<SearchService> logger)
        {
            _restClient = restClient;
            _damAuthenticationService = damAuthenticationService;
            _logger = logger;
        }


        /// <summary>
        /// Executes the specific search
        /// </summary>
        /// <param name="parameters">The parameters to search with</param>
        /// <param name="accessKey">Optional accessKey, if not specified use DamAuthentication</param>
        /// <typeparam name="T">The type the response items should be converted into</typeparam>
        public async Task<SearchResponse<T>> Search<T>(SearchParameters parameters, string? accessKey = null)
        {
            return await Search(new SearchParameters<T>(parameters), accessKey).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the specific search
        /// </summary>
        /// <param name="parameters">The parameters to search with</param>
        /// <param name="accessKey">Optional accessKey, if not specified use DamAuthentication</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T">The type of the response items should be converted into</typeparam>
        public async Task<SearchResponse<T>> Search<T>(SearchParameters<T> parameters, string? accessKey = null,
            CancellationToken cancellationToken = default)
        {
            // Copy the parameters immediately, so the user cannot change them under us
            parameters = new SearchParameters<T>(parameters);
            if (string.IsNullOrWhiteSpace(accessKey))
                accessKey = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);
            
            var request = new RestRequest("SearchService.js");
            
            foreach (var key in parameters.AllKeys)
            {
                var values = parameters.GetValues(key);
                if (values == null)
                {
                    continue;
                }
                foreach (var value in values)
                {
                    request.AddParameter(key, value);
                }
            }

            _logger.LogTrace("Sending search request");
            var response = await _restClient.Execute<DigiResponse<T>>(Method.POST, request, accessKey, cancellationToken).ConfigureAwait(false);
            _logger.LogTrace("Got api response");

            if (!response.Data.Success)
            {
                _logger.LogError("Search request failed", nameof(response), response.Data, nameof(response.Content), response.Content);
                throw new SearchFailedException("Search request failed");
            }
            
            
            return new SearchResponse<T>(response.Data.Items, response.Data.Total, parameters);
        }
    }
}