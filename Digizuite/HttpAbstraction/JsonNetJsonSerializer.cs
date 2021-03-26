using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Helpers;
using Newtonsoft.Json;

namespace Digizuite.HttpAbstraction
{
    public class JsonNetJsonSerializer : IHttpSerializer
    {
        private readonly JsonSerializer _serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                new DigizuiteIntConverter(), 
                new DigizuiteBoolConverter()
            }
        });
        
        public Task<T?> Deserialize<T>(Stream content, CancellationToken cancellationToken = default)
        {

            using var sr = new StreamReader(content);
            using var jsonTextReader = new JsonTextReader(sr);
            return Task.FromResult(_serializer.Deserialize<T>(jsonTextReader));
        }

        public Task Serialize<T>(T item, Stream stream, CancellationToken cancellationToken = default)
        {
            using var sr = new StreamWriter(stream);
            using var jsonTextWriter = new JsonTextWriter(sr);
            _serializer.Serialize(jsonTextWriter, item);
            
            return Task.CompletedTask;
        }
    }
}