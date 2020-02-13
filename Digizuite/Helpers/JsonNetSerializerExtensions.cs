using RestSharp;

namespace Digizuite.Helpers
{
    internal static class JsonNetSerializerExtensions
    {
        public static IRestClient UseJsonNetSerializer(this IRestClient client)
        {
            client.UseSerializer(() => new JsonNetSerializer());
            return client;
        }
    }
}