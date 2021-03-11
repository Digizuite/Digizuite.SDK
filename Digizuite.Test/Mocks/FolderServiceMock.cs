using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Folders;
using Digizuite.Models.Folders;

namespace Digizuite.Test.Mocks
{
    public class FolderServiceMock : IFolderService
    {
        [SuppressMessage("ReSharper", "CA2227")] 
        public List<FolderValue> MockFolderValues { get; set; }

        public Task<List<FolderValue>> GetFolders()
        {
            return Task.FromResult(MockFolderValues);
        }

        public Task<IEnumerable<FolderValue>> GetMemberFolders(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<FolderValue>> GetCatalogFolders()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<FolderValue>> GetPortalFolders()
        {
            return await GetFolders();
        }
    }
}