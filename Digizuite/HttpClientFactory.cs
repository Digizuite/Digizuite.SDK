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
            return new RestClient(_configuration.BaseUrl);
        }
    }
}