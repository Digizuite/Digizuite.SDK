using System;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Digizuite.Helpers
{
    internal class SystemTextJsonSerializer : IRestSerializer
    {
        private JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
        };
        
        public string Serialize(object obj) =>
            JsonSerializer.Serialize(obj);

#pragma warning disable 618
        public string Serialize(Parameter parameter) =>
            JsonSerializer.Serialize(parameter?.Value);
#pragma warning restore 618

        public T? Deserialize<T>(IRestResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return JsonSerializer.Deserialize<T>(response.Content, _options);
        }

        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;
    }
}