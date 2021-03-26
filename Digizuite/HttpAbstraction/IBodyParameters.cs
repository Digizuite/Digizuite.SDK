using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digizuite.HttpAbstraction
{
    public interface IBodyParameters
    {
        public (HttpContent Body, Task Task) GetBody(HttpSerializationSettings serializationSettings, CancellationToken cancellationToken = default);
    }
}