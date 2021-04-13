using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Digizuite.HttpAbstraction
{
    public class SystemTextJsonSerializer : IHttpSerializer
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
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
