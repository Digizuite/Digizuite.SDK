using System;
using Digizuite.Models.Search;
using Digizuite.Test.TestModels;
using NUnit.Framework;
using System.Collections.Generic;

// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable PossibleNullReferenceException
// ReSharper disable UnusedVariable
namespace Digizuite.Test.UnitTests
{
    [TestFixture]
    public class SearchResponseTest
    {
        [Test(Description = "Test Default Constructor")]
        public void TestDefaultConstructor()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 1, 6);
            var response = new SearchResponse<GetAssetsResponse>(items,0, parameters);
            Assert.AreSame(items, response.Items, "Expected item list ws not returned");
            Assert.AreEqual(1, response.Page, "Page did not return expected value");
            Assert.AreEqual(0, response.Total, "Total did not return expected value");
            Assert.AreEqual(0, response.TotalPages, "TotalPages calculation failed");
        }
        [Test(Description = "Test Default Constructor")]
        public void TestPageCalculation1()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 1, 3);
            var response = new SearchResponse<GetAssetsResponse>(items, 8, parameters);
            
            Assert.AreSame(items, response.Items, "Expected item list ws not returned");
            Assert.AreEqual(1, response.Page, "Page did not return expected value");
            Assert.AreEqual(8, response.Total, "Total did not return expected value");
            Assert.AreEqual(3, response.TotalPages, "TotalPages calculation failed");
        }
        [Test(Description = "Test Next property")]

        public void TestNext1()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 2, 3);
            // also test that search arguments are transfered to Next SearchParameter
            parameters.Add("fromAssetId", 0);
            parameters.Add("toAssetId", 10);
            var response = new SearchResponse<GetAssetsResponse>(items, 8, parameters);
            var next = response.Next;
            Assert.IsNotNull(next, "next returned null");
            Assert.That(next.GetValues("limit")[0], Is.EqualTo("3"), "not expected PageSize value");
            Assert.That(next.GetValues("page")[0], Is.EqualTo("3"), "not expected Page value");
            Assert.That(next.GetValues("SearchName")[0], Is.EqualTo("GetAssets"), "not expected SearchName value");
            Assert.That(next.GetValues("fromAssetId")[0], Is.EqualTo("0"), "not expected search parameter fromAssetId value");
            Assert.That(next.GetValues("toAssetId")[0], Is.EqualTo("10"), "not expected search parameter toAssetId value");
        }
        
        [Test(Description = "Test Next property - after last page")]

        public void TestNext2()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 3, 3);
            var response = new SearchResponse<GetAssetsResponse>(items, 8, parameters);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => { var nxt = response.Next; });
            Assert.That(ex.Message, 
                    Is.EqualTo($"Page cannot be more than TotalPages (3) (Parameter 'page'){Environment.NewLine}Actual value was 4."), 
                    "Expected exception message not returned when accessing page# after last page"
            );
        }

        [Test(Description = "Test Previous property")]

        public void TestPrev1()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 2, 3);
            // also test that search arguments are transfered to Next SearchParameter
            parameters.Add("fromAssetId", 0);
            parameters.Add("toAssetId", 10);
            var response = new SearchResponse<GetAssetsResponse>(items, 8, parameters);
            var prev = response.Previous;
            Assert.That(prev.GetValues("limit")[0], Is.EqualTo("3"), "not expected PageSize value");
            Assert.That(prev.GetValues("page")[0], Is.EqualTo("1"), "not expected Page value");
            Assert.That(prev.GetValues("SearchName")[0], Is.EqualTo("GetAssets"), "not expected SearchName value");
            Assert.That(prev.GetValues("fromAssetId")[0], Is.EqualTo("0"), "not expected search parameter fromAssetId value");
            Assert.That(prev.GetValues("toAssetId")[0], Is.EqualTo("10"), "not expected search parameter toAssetId value");
        }

        [Test(Description = "Test Previous property - before first page")]
        public void TestPrev2()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 1, 3);
            var response = new SearchResponse<GetAssetsResponse>(items, 8, parameters);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => { var prev= response.Previous; });
            var msg = ex.Message;
            Assert.That(ex.Message,
                Is.EqualTo($"Page cannot be less than 1 (Parameter 'page'){Environment.NewLine}Actual value was 0."),
                "Expected exception message not returned when accessing page before first page"
            );

        }

        [Test(Description = "Test GoTo Page")]
        public void TestGoToPage()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 1, 3);
            // also test that search arguments are transfered to Next SearchParameter
            parameters.Add("fromAssetId", 0);
            parameters.Add("toAssetId", 10);
            var response = new SearchResponse<GetAssetsResponse>(items, 8, parameters);
            var gto = response.GoToPage(3);
            Assert.IsNotNull(gto, "GoToPage returned null");
            Assert.That(gto.GetValues("limit")[0], Is.EqualTo("3"), "not expected PageSize value");
            Assert.That(gto.GetValues("page")[0], Is.EqualTo("3"), "not expected Page value");
            Assert.That(gto.GetValues("SearchName")[0], Is.EqualTo("GetAssets"), "not expected SearchName value");
            Assert.That(gto.GetValues("fromAssetId")[0], Is.EqualTo("0"), "not expected search parameter fromAssetId value");
            Assert.That(gto.GetValues("toAssetId")[0], Is.EqualTo("10"), "not expected search parameter toAssetId value");
        }

        [Test(Description = "Test IsLast")]
        public void TestIsLast1()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 1, 3);
            var response = new SearchResponse<GetAssetsResponse>(items, 8, parameters);
            Assert.AreEqual(false, response.IsLast, "IsLast does not return correct value");
        }
        [Test(Description = "Test IsLast")]
        public void TestIsLast2()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 2, 3);
            var response = new SearchResponse<GetAssetsResponse>(items, 8, parameters);
            Assert.AreEqual(false, response.IsLast, "IsLast does not return correct value");
        }
        [Test(Description = "Test IsLast")]
        public void TestIsLast3()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 3, 3);
            var response = new SearchResponse<GetAssetsResponse>(items, 8, parameters);
            Assert.AreEqual(true, response.IsLast, "IsLast does not return correct value");
        }
        [Test(Description = "Test IsLast")]
        public void TestIsLast4()
        {
            var items = new List<GetAssetsResponse>();
            var parameters = new SearchParameters("GetAssets", 3, 3);
            var response = new SearchResponse<GetAssetsResponse>(items, 0, parameters);
            Assert.AreEqual(true, response.IsLast, "IsLast does not return correct value");
        }
    }
}
