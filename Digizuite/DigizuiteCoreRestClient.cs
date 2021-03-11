using System;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Helpers;
using Digizuite.Models;
using RestSharp;

namespace Digizuite
{
    public class DigizuiteCoreRestClient : ICoreRestClient
    {
        
        private readonly IRestClient _client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="client">optional IRestClient used for unit testing</param>
        public DigizuiteCoreRestClient(IConfiguration configuration, IRestClient? client = null)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var baseUrl = configuration.GetDigizuiteCoreUrl();
            _client = client ?? new RestClient(baseUrl).UseSerializer(() => new SystemTextJsonSerializer());
        }

        
        public Task<IRestResponse<T>> Execute<T>(Method method, RestRequest request, string? accessKey = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Method = method;
            if (!string.IsNullOrWhiteSpace(accessKey))
            {
                request.AddHeader("Authentication", $"AccessKey {accessKey}");
            }
            return _client.ExecuteAsync<T>(request, cancellationToken);
        }
    }
}