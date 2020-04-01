using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.BatchUpdate;
using Digizuite.BatchUpdate.Models;
using Digizuite.Models.Folders;

namespace Digizuite.Folders
{
    public class AssetFolderService : IAssetFolderService, IDisposable
    {
        private readonly BatchUpdateClient _batchUpdateClient;
        private readonly IFolderService _folderService;
        private readonly ILogger<AssetFolderService> _logger;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public AssetFolderService(BatchUpdateClient batchUpdateClient, IFolderService folderService,
            ILogger<AssetFolderService> logger)
        {
            _batchUpdateClient = batchUpdateClient;
            _folderService = folderService;
            _logger = logger;
        }

        public async Task MoveAssetToFolder(int folderId, RepositoryType folderRepositoryType, int assetItemId)
        {
            var batch = new Batch();
            batch.AddValues(new BatchPart
            {
                RowId = 1,
                Target = FieldType.Asset,
                BatchType = BatchType.ItemIdsValuesRowId,
                ItemIds = new List<int> {assetItemId},
                Values = new List<BatchValue>
                {
                    new FolderBatchValue(FieldType.Folder, folderId, null, folderRepositoryType)
                }
            });

            await ApplyBatch(batch).ConfigureAwait(false);
        }

        private async Task ApplyBatch(Batch batch)
        {
            await _lock.WaitAsync().ConfigureAwait(false);
            try
            {
                await _batchUpdateClient.ApplyBatch(batch).ConfigureAwait(false);
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task RemoveAssetFromFolder(int folderId, RepositoryType folderRepositoryType, int assetItemId)
        {
            var batch = new Batch();
            batch.AddValues(new BatchPart
            {
                RowId = 1,
                Target = FieldType.Asset,
                BatchType = BatchType.ItemIdsValues,
                ItemIds = new List<int> {assetItemId},
                Values = new List<BatchValue>
                {
                    new FolderBatchValue(FieldType.Folder, folderId, null, folderRepositoryType, true)
                }
            });
            await ApplyBatch(batch).ConfigureAwait(false);
        }

        public async Task RemoveAssetFromAllChannels(List<FolderValueReference> excluded, int assetItemId)
        {
            var allFolders = (await _folderService.GetPortalFolders().ConfigureAwait(false)).ToList();
            var folders = GetAndFilterExcludedFolders(allFolders, excluded);
            foreach (var folder in folders)
            {
                _logger.LogDebug("Removing asset from portal folder", nameof(folder), folder);
                await RemoveAssetFromFolder(folder.FolderId, folder.RepositoryType, assetItemId).ConfigureAwait(false);
            }
        }

        public async Task RemoveAssetFromFolderRecursive(FolderValueReference folder,
            List<FolderValueReference> excluded, int assetItemId)
        {
            var allFolders = (await _folderService.GetPortalFolders().ConfigureAwait(false)).ToList();
            var mainFolder = allFolders.Single(f => f.FolderId == folder.FolderId);
            var children = new HashSet<FolderValue>();
            FindAllChildren(mainFolder, allFolders, children);
            var toRemoveFrom = GetAndFilterExcludedFolders(children.ToList(), excluded);
            toRemoveFrom.Add(mainFolder);
            foreach (var folderValue in toRemoveFrom)
            {
                _logger.LogDebug("Removing asset from folder", nameof(folderValue), folderValue);
                await RemoveAssetFromFolder(folderValue.FolderId, folderValue.RepositoryType, assetItemId).ConfigureAwait(false);
            }
        }

        #region Excluded folders

        private List<FolderValue> FindChildren(FolderValueReference item, List<FolderValue> folders)
        {
            return folders.FindAll(folder =>
                folder.GroupId == item.FolderId && folder.RepositoryType == item.RepositoryType);
        }

        private FolderValue ToFolderValue(FolderValueReference item, List<FolderValue> folders)
        {
            var folderValue = folders.First(folder =>
                folder.FolderId == item.FolderId && folder.RepositoryType == item.RepositoryType);
            return folderValue;
        }

        public void FindAllChildren(FolderValue folder, List<FolderValue> folders, ISet<FolderValue> resultingChildren)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            if (folders == null)
            {
                throw new ArgumentNullException(nameof(folders));
            }

            if (resultingChildren == null)
            {
                throw new ArgumentNullException(nameof(resultingChildren));
            }
            
            var children = FindChildren(folder, folders);

            // Errors indicate that no children was found and we reached the leafs
            if (!children.Any())
            {
                return;
            }

            foreach (var child in children)
            {
                // Add the child node to exclude
                resultingChildren.Add(child);
                FindAllChildren(child, folders, resultingChildren);
            }
        }

        public List<FolderValue> GetAndFilterExcludedFolders(List<FolderValue> folders,
            List<FolderValueReference> excluded)
        {
            if (folders == null)
            {
                throw new ArgumentNullException(nameof(folders));
            }
            if (excluded == null)
            {
                throw new ArgumentNullException(nameof(excluded));
            }

            // Excluded only represents the folders that a user typed in
            var excludedFolders = excluded.Select(folder => ToFolderValue(folder, folders)).ToList();

            // All excluded folders includes child items to those folders a user typed
            // Is a set to prevent duplicate items in case a user was an idiot and typed nested folders to be excluded
            var allExcludedFolders = new HashSet<FolderValue>();

            foreach (var folderValue in excludedFolders)
            {
                // Add initial node
                allExcludedFolders.Add(folderValue);

                // Trace excluded items down to the leafs
                //    Do a depth first search starting from each of the excluded folders
                //    Add each of the folders to a list in an outer scope
                //    Intersect that list with all the folders
                // Remove all of these items from the folders return variable
                // Return the folders that are now only the set of non excluded folders
                FindAllChildren(folderValue, folders, allExcludedFolders);
            }

            foreach (var excludedFolder in allExcludedFolders)
            {
                folders.Remove(excludedFolder);
            }

            return folders;
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _lock.Dispose();
                }

                _disposed = true;
            }    
        }
        
        
    }
}