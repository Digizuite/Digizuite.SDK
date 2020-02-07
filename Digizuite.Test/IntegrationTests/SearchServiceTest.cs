using System.Threading.Tasks;
using Digizuite.Models.Search;
using Digizuite.Test.TestModels;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class SearchServiceTest : IntegrationTestBase
    {
        [Test]
        public async Task CanSearchMultiplePages()
        {
            var service = ServiceProvider.GetRequiredService<ISearchService>();

            var parameters = new SearchParameters("GetAssets")
            {
                {"sCatalogFolderId", "40"}
            };

            var firstResult = await service.Search<GetAssetsResponse>(parameters);

            Assert.That(firstResult.Items.Count, Is.EqualTo(12));

            var secondResult = await service.Search(firstResult.Next);
            Assert.That(secondResult.Items.Count, Is.EqualTo(12));
            Assert.That(secondResult.Items, Is.Not.EquivalentTo(firstResult.Items));
        }
    }
}