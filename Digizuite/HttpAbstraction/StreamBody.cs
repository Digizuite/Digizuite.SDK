using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digizuite.HttpAbstraction
{
    public class StreamBody : IBodyParameters
    {
        private readonly Stream _stream;

        public StreamBody(Stream stream)
        {
            _stream = stream;
        }

        public (HttpContent Body, Task Task) GetBody(HttpSerializationSettings serializationSettings,
            CancellationToken cancellationToken = default)
        {
            var body = new StreamContent(_stream);

            return (body, Task.CompletedTask);
        }
    }
}