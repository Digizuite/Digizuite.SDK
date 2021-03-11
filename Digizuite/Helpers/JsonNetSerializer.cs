using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;

namespace Digizuite.Helpers
{
    internal class JsonNetSerializer : IRestSerializer
    {
        private readonly List<JsonConverter> _jsonConverters;
        public JsonNetSerializer()
        {
            _jsonConverters = new List<JsonConverter>
            {
                new DigizuiteIntConverter(), 
                new DigizuiteBoolConverter()
            };
        }
        public JsonNetSerializer(List<JsonConverter> converters)
        {
            _jsonConverters = converters;
        }
        public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj);

        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter?.Value);

        public T Deserialize<T>(IRestResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                Converters = _jsonConverters ?? new List<JsonConverter>()
            });
        }

        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;
    }
}
