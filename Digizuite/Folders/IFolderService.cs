using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Models.Folders;

namespace Digizuite.Folders
{
    public interface IFolderService
    {
        Task<List<FolderValue>> GetFolders();
        Task<IEnumerable<FolderValue>> GetMemberFolders(CancellationToken cancellationToken = default );
        Task<IEnumerable<FolderValue>> GetCatalogFolders();
        Task<IEnumerable<FolderValue>> GetPortalFolders();
    }
}