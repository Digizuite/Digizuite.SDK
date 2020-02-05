using Digizuite.Models;
using RestSharp;

namespace Digizuite
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly Configuration _configuration;

        public HttpClientFactory(Configuration configuration)
        {
            _configuration = configuration;
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
            
            
            return new RestClient(baseUrl);
        }
    }
}