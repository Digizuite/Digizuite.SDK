using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class AssetServiceTest : IntegrationTestBase
    {
        [Test]
        public async Task LoadsAssets()
        {
            var service = ServiceProvider.GetRequiredService<IAssetService>();
            var page1 = await service.GetAssets();

            var page2 = await service.GetAssets(page1.Next);

            Assert.That(page1.Items.Select(a => a.AssetId), Is.Not.EquivalentTo(page2.Items.Select(a => a.AssetId)));

            var asset = page1.Items[0];

            var loadedAssetByItemId = await service.GetAssetByItemId(asset.ItemId);
            Assert.That(loadedAssetByItemId, Is.EqualTo(asset));
            var loadedAssetByAssetId = await service.GetAssetByAssetId(asset.AssetId);
            Assert.That(loadedAssetByAssetId, Is.EqualTo(asset));
        }
    }
}