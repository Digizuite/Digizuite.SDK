using System.IO.Pipelines;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Digizuite.HttpAbstraction
{
    public class JsonBody {}
    
    public class JsonBody<T> : JsonBody, IBodyParameters
    {
        private readonly T _data;

        public JsonBody(T data)
        {
            _data = data;
        }

        public (HttpContent, Task) GetBody(HttpSerializationSettings serializationSettings, CancellationToken cancellationToken = default)
        {
            var pipe = new Pipe();

            var writerStream = pipe.Writer.AsStream();
            var streamTask =
                serializationSettings.Serializer.Serialize(_data, writerStream, cancellationToken)
                    // The http client won't "finish" until the stream is marked as completed, so do that once 
                    // we have written everything to the pipe.
                    .ContinueWith(_ => writerStream.Dispose(), cancellationToken);

            var content = new StreamContent(pipe.Reader.AsStream());
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            return (content, streamTask);
        }
    }
}
