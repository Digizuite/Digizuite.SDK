using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Digizuite.HttpAbstraction
{
    public interface IHttpSerializer
    {
        public Task<T?> Deserialize<T>(Stream content, CancellationToken cancellationToken = default);

        public Task Serialize<T>(T item, Stream stream, CancellationToken cancellationToken = default);
    }
}