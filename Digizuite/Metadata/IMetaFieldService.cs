using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Metadata.ResponseModels;

namespace Digizuite.Metadata
{
    public interface IMetaFieldService
    {
        Task<List<MetaFieldResponse>> GetAllMetaFields(string? accessKey = null,
            CancellationToken cancellationToken = default);
    }
}