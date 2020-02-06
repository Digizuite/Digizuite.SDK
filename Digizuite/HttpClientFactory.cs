using Digizuite.Models;
using RestSharp;

namespace Digizuite
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly Configuration _configuration;
        private readonly ILogger<HttpClientFactory> _logger;

        public HttpClientFactory(Configuration configuration, ILogger<HttpClientFactory> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IRestClient GetRestClient()
        {
            var baseUrl = _configuration.BaseUrl;

            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            if (!baseUrl.EndsWith("/dmm3bwsv3/"))
            {
                baseUrl += "dmm3bwsv3/";
            }
            
            _logger.LogTrace("Creating request client", nameof(baseUrl), baseUrl);


            return new RestClient(baseUrl);
        }
    }
}