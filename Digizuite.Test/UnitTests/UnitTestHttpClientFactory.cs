using RestSharp;

namespace Digizuite.Test.UnitTests
{
#pragma warning disable CA1812
    internal class UnitTestHttpClientFactory : IHttpClientFactory
    {
        public IRestClient GetRestClient()
        {
            throw new System.NotImplementedException(@"Dam access needs to be mock'ed out for UnitTests");
        }
    }
#pragma warning restore CA1812
}