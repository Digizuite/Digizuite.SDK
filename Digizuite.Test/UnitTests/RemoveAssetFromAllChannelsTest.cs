using System.Collections.Generic;
using System.Linq;
using Digizuite.BatchUpdate.Models;
using Digizuite.Folders;
using Digizuite.Models.Folders;
using Digizuite.Test.IntegrationTests;
using Digizuite.Test.Mocks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.UnitTests
{
    public class RemoveAssetFromAllChannelsTest : IntegrationTestBase
    {
        private static List<FolderValue> MockFolderValues =>
            new List<FolderValue>()
            {
                new FolderValue()
                {
                    Label = "Root",
                    FolderId = 1,
                    ItemId = 100,
                    GroupId = 0,
                    RepositoryType = RepositoryType.Portal
                },
                new FolderValue()
                {
                    Label = "Root 2",
                    FolderId = 2,
                    ItemId = 101,
                    GroupId = 0,
                    RepositoryType = RepositoryType.Portal
                },
                new FolderValue()
                {
                    Label = "Left 1 1",
                    FolderId = 3,
                    ItemId = 102,
                    GroupId = 1,
                    RepositoryType = RepositoryType.Portal
                },
                new FolderValue()
                {
                    Label = "Right 1 1",
                    FolderId = 4,
                    ItemId = 103,
                    GroupId = 1,
                    RepositoryType = RepositoryType.Portal
                },
                new FolderValue()
                {
                    Label = "Left 2 1",
                    FolderId = 5,
                    ItemId = 104,
                    GroupId = 2,
                    RepositoryType = RepositoryType.Portal
                },
                new FolderValue()
                {
                    Label = "Left 2 2",
                    FolderId = 6,
                    ItemId = 105,
                    GroupId = 5,
                    RepositoryType = RepositoryType.Portal
                },
                new FolderValue()
                {
                    Label = "Left 2 3",
                    FolderId = 7,
                    ItemId = 106,
                    GroupId = 6,
                    RepositoryType = RepositoryType.Portal
                },
                new FolderValue()
                {
                    Label = "Left 2 4",
                    FolderId = 8,
                    ItemId = 107,
                    GroupId = 7,
                    RepositoryType = RepositoryType.Portal
                }
            };

        protected override void SetupDependencies(IServiceCollection services)
        {
            base.SetupDependencies(services);
            ReplaceWith<IFolderService, FolderServiceMock>(services);
        }

        [Test]
        public void TestRemovalOfExcludedFolders()
        {
            var folderServiceMock = Get<IFolderService, FolderServiceMock>();
            folderServiceMock.MockFolderValues = MockFolderValues;

            var assetFolderService = Get<IAssetFolderService, AssetFolderService>();

            var excluded = new List<FolderValueReference>()
            {
                new FolderValueReference()
                {
                    FolderId = 1,
                    RepositoryType = RepositoryType.Portal
                }
            };

            var folders = assetFolderService.GetAndFilterExcludedFolders(MockFolderValues, excluded);


            Assert.That(folders.Count, Is.EqualTo(5));

            var folderIds = folders.Select(folder => folder.FolderId);

            Assert.That(folderIds, Is.EqualTo(new List<int> {2, 5, 6, 7, 8}));
        }

        [Test]
        public void TestRemovalOfFoldersWithNestedExclusions()
        {
            var folderServiceMock = Get<IFolderService, FolderServiceMock>();
            folderServiceMock.MockFolderValues = MockFolderValues;

            var assetFolderService = Get<IAssetFolderService, AssetFolderService>();

            var excluded = new List<FolderValueReference>()
            {
                new FolderValueReference()
                {
                    FolderId = 1,
                    RepositoryType = RepositoryType.Portal
                },
                new FolderValueReference()
                {
                    FolderId = 3,
                    RepositoryType = RepositoryType.Portal
                }
            };

            var folders = assetFolderService.GetAndFilterExcludedFolders(MockFolderValues, excluded);


            Assert.That(folders.Count, Is.EqualTo(5));

            var folderIds = folders.Select(folder => folder.FolderId);

            Assert.That(folderIds, Is.EqualTo(new List<int> {2, 5, 6, 7, 8}));
        }

        [Test]
        public void TestRemovalOfFoldersAllExcluded()
        {
            var folderServiceMock = Get<IFolderService, FolderServiceMock>();
            folderServiceMock.MockFolderValues = MockFolderValues;

            var assetFolderService = Get<IAssetFolderService, AssetFolderService>();

            var excluded = new List<FolderValueReference>()
            {
                new FolderValueReference()
                {
                    FolderId = 1,
                    RepositoryType = RepositoryType.Portal
                },
                new FolderValueReference()
                {
                    FolderId = 2,
                    RepositoryType = RepositoryType.Portal
                }
            };

            var folders = assetFolderService.GetAndFilterExcludedFolders(MockFolderValues, excluded);


            Assert.That(folders.Count, Is.EqualTo(0));

            var folderIds = folders.Select(folder => folder.FolderId);

            Assert.That(folderIds, Is.EqualTo(new List<int>()));
        }

        [Test]
        public void TestRemovalOfFoldersOnMiddleOfChain()
        {
            var folderServiceMock = Get<IFolderService, FolderServiceMock>();
            folderServiceMock.MockFolderValues = MockFolderValues;

            var assetFolderService = Get<IAssetFolderService, AssetFolderService>();

            var excluded = new List<FolderValueReference>()
            {
                new FolderValueReference()
                {
                    FolderId = 7,
                    RepositoryType = RepositoryType.Portal
                }
            };

            var folders = assetFolderService.GetAndFilterExcludedFolders(MockFolderValues, excluded);


            Assert.That(folders.Count, Is.EqualTo(6));

            var folderIds = folders.Select(folder => folder.FolderId);

            Assert.That(folderIds, Is.EqualTo(new List<int> {1, 2, 3, 4, 5, 6}));
        }
    }
}