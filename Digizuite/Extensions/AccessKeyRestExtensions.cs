using Digizuite.HttpAbstraction;

namespace Digizuite.Extensions
{
    public static class AccessKeyRestExtensions
    {
        private const string AuthorizationHeader = "Authorization";

        public static RestRequest AddAccessKey(this RestRequest request, string accessKey)
        {
            var value = $"AccessKey {accessKey}";

            request.Headers.Set(AuthorizationHeader, value);
            return request;
        }
    }
}