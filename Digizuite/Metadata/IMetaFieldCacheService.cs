using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata
{
    public interface IMetaFieldCacheService
    {
        Task<List<MetaFieldGroup>> GetMetaFieldGroups();
        Task<List<MetaFieldGroupFolder>> GetMetaFieldFolders();
        Task<List<MetaField>> GetMetaFields();
        void Clear();
    }
}