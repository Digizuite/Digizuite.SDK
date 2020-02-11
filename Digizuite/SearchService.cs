using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Helpers;
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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly ILogger<SearchService> _logger;

        public SearchService(IHttpClientFactory httpClientFactory, IDamAuthenticationService damAuthenticationService,
            ILogger<SearchService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _damAuthenticationService = damAuthenticationService;
            _logger = logger;
        }


        /// <summary>
        /// Executes the specific search
        /// </summary>
        /// <param name="parameters">The parameters to search with</param>
        /// <typeparam name="T">The type the response items should be converted into</typeparam>
        public async Task<SearchResponse<T>> Search<T>(SearchParameters parameters)
        {
            // Copy the parameters immediately, so the user cannot change them under us
            parameters = new SearchParameters(parameters);

            var client = _httpClientFactory.GetRestClient();
            client.UseJsonNetSerializer();

            var ak = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);
            
            var request = new RestRequest("SearchService.js");
            request.MakeRequestDamSafe()
                .AddParameter(DigizuiteConstants.AccessKeyParameter, ak);
            
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
            var response = await client.ExecutePostAsync<DigiResponse<T>>(request).ConfigureAwait(false);
            _logger.LogTrace("Got api response");

            if (!response.Data.Success)
            {
                _logger.LogError("Search request failed", nameof(response), response.Data, nameof(response.Content), response.Content);
                throw new SearchFailedException("Search request failed");
            }
            
            
            return new SearchResponse<T>(response.Data.Items, response.Data.Total, parameters);
        }

        /// <summary>
        /// Executes the specific search
        /// </summary>
        /// <param name="parameters">The parameters to search with</param>
        /// <typeparam name="T">The type of the response items should be converted into</typeparam>
        public Task<SearchResponse<T>> Search<T>(SearchParameters<T> parameters)
        {
            return Search<T>((SearchParameters) parameters);
        }
    }
}