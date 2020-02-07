using Digizuite.Models;
using RestSharp;

namespace Digizuite
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HttpClientFactory> _logger;

        public HttpClientFactory(IConfiguration configuration, ILogger<HttpClientFactory> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IRestClient GetRestClient()
        {
            var baseUrl = _configuration.GetDmm3Bwsv3Url();
            
            _logger.LogTrace("Creating request client", nameof(baseUrl), baseUrl);


            return new RestClient(baseUrl);
        }
    }
}