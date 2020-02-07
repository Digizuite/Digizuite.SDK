using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Digizuite.Models.Search;
using Digizuite.Test.TestModels;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Digizuite.Test.UnitTests
{
    [TestFixture]
    public class SearchServiceTest : UnitTestBase
    {
        private IReadOnlyList<GetAssetsResponse> GetAssetsTestData()
        {
            return new List<GetAssetsResponse>()
            {
                new GetAssetsResponse() { AssetId = 9, ItemId = 9408, Name = "Landscape_6" },
                new GetAssetsResponse() { AssetId = 26, ItemId = 9463, Name = "imagedd" },
                new GetAssetsResponse() { AssetId = 28, ItemId = 9420, Name = "test" },
                new GetAssetsResponse() { AssetId = 29, ItemId = 9431, Name = "Corgi 02" },
                new GetAssetsResponse() { AssetId = 52, ItemId = 10121, Name = "Landscape_8" },
                new GetAssetsResponse() { AssetId = 54, ItemId = 10175, Name = "2083_" },
                new GetAssetsResponse() { AssetId = 91, ItemId = 10587, Name = "Maersk deeplink embed distributaion" },
                new GetAssetsResponse() { AssetId = 92, ItemId = 10588, Name = "Velkommen til Word" },
                new GetAssetsResponse() { AssetId = 93, ItemId = 10592, Name = "Gatonovela 2" },
                new GetAssetsResponse() { AssetId = 94, ItemId = 10589, Name = "iOS Horizontal" },
                new GetAssetsResponse() { AssetId = 95, ItemId = 10591, Name = "SampleVideo_1280x720_1mb" },
                new GetAssetsResponse() { AssetId = 98, ItemId = 10590, Name = "iOS Vertical" },
                new GetAssetsResponse() { AssetId = 99, ItemId = 10595, Name = "Team Viewer" },
                new GetAssetsResponse() { AssetId = 101, ItemId = 10596, Name = "This is a test" },
                new GetAssetsResponse() { AssetId = 102, ItemId = 10599, Name = "Images" },
                new GetAssetsResponse() { AssetId = 105, ItemId = 10609, Name = "PopChartLab_Superpowers_FinalFinal-Large" },
                new GetAssetsResponse() { AssetId = 108, ItemId = 10613, Name = "tall video" },
                new GetAssetsResponse() { AssetId = 109, ItemId = 10617, Name = "0000-hero-candle" },
                new GetAssetsResponse() { AssetId = 127, ItemId = 10639, Name = "Support system agreement" },
                new GetAssetsResponse() { AssetId = 128, ItemId = 10641, Name = "Amazon - A Story of Great Sorrow and Bananas" },
                new GetAssetsResponse() { AssetId = 129, ItemId = 10640, Name = "Books Everyone Should Read" },
                new GetAssetsResponse() { AssetId = 132, ItemId = 10645, Name = "4253_" },
                new GetAssetsResponse() { AssetId = 139, ItemId = 10653, Name = "roses" },
                new GetAssetsResponse() { AssetId = 144, ItemId = 10659, Name = "IFS Corporate brochure 2017" },
                new GetAssetsResponse() { AssetId = 145, ItemId = 10658, Name = "Nowy Styl Group 2018" },
                new GetAssetsResponse() { AssetId = 158, ItemId = 10679, Name = "UploadIllustratorTest" },
                new GetAssetsResponse() { AssetId = 161, ItemId = 10682, Name = "IllustratorUploadnewfileagain" },
                new GetAssetsResponse() { AssetId = 162, ItemId = 10683, Name = "english title" },
                new GetAssetsResponse() { AssetId = 220, ItemId = 12145, Name = "Foo bar" },
                new GetAssetsResponse() { AssetId = 222, ItemId = 12149, Name = "A blue web of lies" }
            };
        }

        [Test]
        public async Task TestPaging()
        {
            var parameters = new SearchParameters("GetAssets")
            {
                {"sCatalogFolderId", "40"}
            };

            var data = GetAssetsTestData();

            var service = new Mock<ISearchService>(MockBehavior.Strict);
            service.Setup(x => x.Search<GetAssetsResponse>(It.IsAny<SearchParameters>()))
                .ReturnsAsync((SearchParameters par) => 
                    new SearchResponse<GetAssetsResponse>(data.Skip((par.Page-1)*par.PageSize).Take(par.PageSize).ToList(), data.Count, par));
            service.Setup(x => x.Search<GetAssetsResponse>(It.IsAny<SearchParameters<GetAssetsResponse>>()))
                .ReturnsAsync((SearchParameters par) =>
                    new SearchResponse<GetAssetsResponse>(data.Skip((par.Page - 1) * par.PageSize).Take(par.PageSize).ToList(), data.Count, par));



            var firstResult = await service.Object.Search<GetAssetsResponse>(parameters).ConfigureAwait(true);

            Assert.That(firstResult.Items.Count, Is.EqualTo(12));
            Assert.AreEqual(9408, firstResult.Items[0].ItemId);
            Assert.AreEqual(2, firstResult.Next.Page);
            Assert.AreEqual(3, firstResult.TotalPages);
            Assert.AreEqual(false, firstResult.IsLast);
            Assert.AreEqual(30, firstResult.Total);
            var exPrev = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var prevPage = firstResult.Previous;
            });
            Assert.AreEqual("Page cannot be less than 1 (Parameter 'page')\r\nActual value was 0.", exPrev.Message);

            var secondResult = await service.Object.Search<GetAssetsResponse>(firstResult.Next).ConfigureAwait(true);

            Assert.That(secondResult.Items.Count, Is.EqualTo(12));
            Assert.AreEqual(10595, secondResult.Items[0].ItemId);
            Assert.AreEqual(3, secondResult.Next.Page);
            Assert.AreEqual(3, secondResult.TotalPages);
            Assert.AreEqual(false, secondResult.IsLast);
            Assert.AreEqual(1, secondResult.Previous.Page);

            var thirdResult = await service.Object.Search<GetAssetsResponse>(secondResult.Next).ConfigureAwait(true);

            Assert.That(thirdResult.Items.Count, Is.EqualTo(6));
            Assert.AreEqual(10658, thirdResult.Items[0].ItemId);
            Assert.AreEqual(3, thirdResult.TotalPages);
            Assert.AreEqual(true, thirdResult.IsLast);
            Assert.AreEqual(2, thirdResult.Previous.Page);
            var exNext = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var nextPage = thirdResult.Next;
            });
            Assert.AreEqual("Page cannot be more than TotalPages (3) (Parameter 'page')\r\nActual value was 4.", exNext.Message);

            var gotoPage = await service.Object.Search<GetAssetsResponse>(thirdResult.GoToPage(2)).ConfigureAwait(true);
            Assert.That(gotoPage.Items.Count, Is.EqualTo(12));
            Assert.AreEqual(10595, gotoPage.Items[0].ItemId);
            Assert.AreEqual(3, gotoPage.Next.Page);
            Assert.AreEqual(3, gotoPage.TotalPages);
            Assert.AreEqual(false, gotoPage.IsLast);
            Assert.AreEqual(1, gotoPage.Previous.Page);
        }

    }
}
