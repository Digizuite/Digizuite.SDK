using System.Linq;
using RestSharp;

namespace Digizuite.Extensions
{
    public static class RestRequestExtensions
    {
        public static IRestRequest AddAccessKey(this IRestRequest request, string accessKey)
        {
            var value = $"AccessKey {accessKey}";
            
            var existingAuthHeader =
                request.Parameters.FirstOrDefault(p => p.Type == ParameterType.HttpHeader && p.Name == "Authorization");

            if (existingAuthHeader != null)
            {
                existingAuthHeader.Value = value;
                return request;
            }
            else
            {
                return request.AddHeader("Authorization", value);
            }
        }
    }
}