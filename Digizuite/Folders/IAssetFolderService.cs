using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.BatchUpdate.Models;
using Digizuite.Models.Folders;

namespace Digizuite.Folders
{
    public interface IAssetFolderService
    {
        Task MoveAssetToFolder(int folderId, RepositoryType folderRepositoryType, int assetItemId);

        Task RemoveAssetFromFolder(int folderId, RepositoryType folderRepositoryType, int assetItemId);

        Task RemoveAssetFromAllChannels(List<FolderValueReference> excludedFolders, int assetItemId);

        Task RemoveAssetFromFolderRecursive(FolderValueReference folder,
            List<FolderValueReference> excluded, int assetItemId);
    }
}