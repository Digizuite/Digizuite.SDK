﻿using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;

namespace Digizuite
{
    internal class JsonNetSerializer : IRestSerializer
    {
        public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj);

        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value);

        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content);

        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;
    }

    internal static class JsonNetSerializerExtensions
    {
        public static IRestClient UseJsonNetSerializer(this IRestClient client)
        {
            client.UseSerializer(() => new JsonNetSerializer());
            return client;
        }
    }
}
