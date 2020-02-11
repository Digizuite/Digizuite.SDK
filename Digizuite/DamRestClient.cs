using System;
using System.Threading.Tasks;
using RestSharp;

namespace Digizuite
{
    /// <inheritdoc cref="IDamRestClient"/>
    public class DamRestClient : IDamRestClient
    {
        private readonly RestClient _client;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">optional logger</param>
        public DamRestClient(ILogger logger = null)
        {
            _client = new RestClient();
            _client.UseSerializer(() => new JsonNetSerializer());
            _logger = logger;
        }
        /// <inheritdoc cref="IDamRestClient"/>
        public Task<IRestResponse<T>> Execute<T>(Method method, RestRequest request, string accessKey = null)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            request.OnBeforeDeserialization = response => { response.ContentType = "application/json"; };
            request.Method = method;
            if (!string.IsNullOrWhiteSpace(accessKey))
            {
                _logger?.LogTrace("Adding AccessKeyParameter");
                request.AddParameter(DigizuiteConstants.AccessKeyParameter, accessKey);
            }
            return _client.ExecuteAsync<T>(request);
        }
    }
}
