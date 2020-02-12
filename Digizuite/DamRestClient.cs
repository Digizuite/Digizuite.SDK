using System;
using System.Threading.Tasks;
using Digizuite.Models;
using RestSharp;
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable NotAccessedField.Local

namespace Digizuite
{
    /// <inheritdoc />
    public class DamRestClient : IDamRestClient
    {
        private readonly RestClient _client;
        private readonly ILogger<DamRestClient> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger">optional logger</param>
        public DamRestClient(IConfiguration configuration, ILogger<DamRestClient> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var baseUrl = _configuration.GetDmm3Bwsv3Url();
            _client = new RestClient(baseUrl);
            _client.UseSerializer(() => new JsonNetSerializer());
        }
        /// <inheritdoc />
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
                request.AddParameter(DigizuiteConstants.AccessKeyParameter, accessKey);
            }
            return _client.ExecuteAsync<T>(request);
        }
    }
}
