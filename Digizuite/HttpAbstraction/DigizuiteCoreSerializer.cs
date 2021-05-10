using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Digizuite.HttpAbstraction
{
    public class DigizuiteCoreSerializer : IHttpSerializer
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(),
                new TimeSpanConverter()
            }
        };

        public async Task<T?> Deserialize<T>(Stream content, CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync<T>(content, JsonSerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task Serialize<T>(T item, Stream stream, CancellationToken cancellationToken = default)
        {
            await JsonSerializer.SerializeAsync(stream, item, JsonSerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
