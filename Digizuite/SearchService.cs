using System.Threading;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Models;
using Digizuite.Models.Search;

namespace Digizuite
{
    /// <summary>
    /// A concrete implementation of the search service
    /// </summary>
    public class SearchService : ISearchService
    {
        private readonly ServiceHttpWrapper _serviceHttpWrapper;
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly ILogger<SearchService> _logger;

        public SearchService(ServiceHttpWrapper serviceHttpWrapper, IDamAuthenticationService damAuthenticationService,
            ILogger<SearchService> logger)
        {
            _serviceHttpWrapper = serviceHttpWrapper;
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
            accessKey ??= await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);

            var (client, request) = _serviceHttpWrapper.GetSearchServiceClient();

            foreach (var key in parameters.AllKeys)
            {
                var values = parameters.GetValues(key);
                if (values == null)
                {
                    continue;
                }

                foreach (var value in values)
                {
                    request.AddQueryParameter(key, value);
                }
            }

            request.AddAccessKey(accessKey);

            _logger.LogTrace("Sending search request");
            var response = await client.GetAsync<DigiResponse<T>>(request, cancellationToken).ConfigureAwait(false);
            _logger.LogTrace("Got api response");



            if (!response.IsSuccessful || !response.Data!.Success)
            {
                _logger.LogError("Search request failed", nameof(response), response);
                throw new SearchFailedException("Search request failed", response.Exception);
            }


            return new SearchResponse<T>(response.Data!.Items, response.Data!.Total, parameters);
        }
    }
}