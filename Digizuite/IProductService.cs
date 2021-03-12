using System.Threading;
using System.Threading.Tasks;

namespace Digizuite
{
    public interface IProductService
    {
        Task<string> GetProductItemGuidFromVersionId(string versionId, string? accessKey = null,
            CancellationToken cancellationToken = default );
    }
}
