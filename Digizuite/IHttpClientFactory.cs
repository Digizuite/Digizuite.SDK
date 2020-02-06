using RestSharp;

namespace Digizuite
{
    public interface IHttpClientFactory
    {
        IRestClient GetRestClient();
    }
}