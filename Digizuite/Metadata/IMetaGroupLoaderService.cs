using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Models;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata
{
    public interface IMetaGroupLoaderService
    {
        Task<List<SystemToolsNodeItem>> GetMetaGroupInGroup(string hierarchyId = "/",
            CancellationToken cancellationToken = default );
        Task<RootSystemToolsResponse> GetRootMetaGroups(CancellationToken cancellationToken = default );
        Task<Tuple<List<MetaFieldGroup>, List<MetaFieldGroupFolder>>> GetAllMetaDataGroups();
    }
}