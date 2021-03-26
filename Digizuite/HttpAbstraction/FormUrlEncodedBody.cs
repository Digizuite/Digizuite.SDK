using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digizuite.HttpAbstraction
{
    public class FormUrlEncodedBody : IBodyParameters
    {
        public readonly Dictionary<string, string> Parameters;

        public FormUrlEncodedBody(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
        }


        public (HttpContent, Task) GetBody(HttpSerializationSettings serializationSettings,
            CancellationToken cancellationToken = default)
        {
            return (new FormUrlEncodedContent(Parameters), Task.CompletedTask);
        }
    }
}