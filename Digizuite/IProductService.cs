using System.Threading;
using System.Threading.Tasks;
using Digizuite.Models;

namespace Digizuite
{
    public interface IProductService
    {
        Task<string> GetProductItemGuidFromVersionId(string versionId, string accessKey = null,
            CancellationToken cancellationToken = default );
    }
}
