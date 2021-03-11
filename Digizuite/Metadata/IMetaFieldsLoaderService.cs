using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata
{
    public interface IMetaFieldsLoaderService
    {
        Task<List<MetaField>> GetMetaFieldsInGroup(int groupId, CancellationToken cancellationToken = default );
    }
}