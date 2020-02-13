using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;

namespace Digizuite.Helpers
{
    internal class JsonNetSerializer : IRestSerializer
    {
        private List<JsonConverter> _jsonConverters = new List<JsonConverter>();
        public JsonNetSerializer()
        {
            _jsonConverters = new List<JsonConverter>();
            _jsonConverters.Add(new DigizuiteIntConverter());
            _jsonConverters.Add(new DigizuiteBoolConverter());
        }
        public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj);

        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value);

        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                Converters = _jsonConverters ?? new List<JsonConverter>()
            });

        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;
    }
}
