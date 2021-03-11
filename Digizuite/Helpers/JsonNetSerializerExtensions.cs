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

        public static IRestClient UseSystemTextJsonSerializer(this IRestClient client)
        {
            client.UseSerializer(() => new SystemTextJsonSerializer());
            return client;
        }
    }
}