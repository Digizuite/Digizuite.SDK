using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.Models;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata
{
    public interface IMetaGroupLoaderService
    {
        Task<List<SystemToolsNodeItem>> GetMetaGroupInGroup(string hierarchyId = "/");
        Task<RootSystemToolsResponse> GetRootMetaGroups();
        Task<Tuple<List<MetaFieldGroup>, List<MetaFieldGroupFolder>>> GetAllMetaDataGroups();
    }
}