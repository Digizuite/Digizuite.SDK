using RestSharp;

namespace Digizuite.Test.UnitTests
{
    internal class UnitTestHttpClientFactory : IHttpClientFactory
    {
        public IRestClient GetRestClient()
        {
            throw new System.NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
        }
    }
}
