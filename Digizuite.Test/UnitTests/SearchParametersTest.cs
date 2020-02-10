using System;
using System.Collections.Generic;
using System.Linq;
using Digizuite.Models.Search;
using Digizuite.Test.TestModels;
using NUnit.Framework;

namespace Digizuite.Test.UnitTests
{
    [TestFixture]
    public class SearchParametersTest
    {
        [Test(Description = "Test Default Constructor")]
        public void TestDefaultConstructor()
        {
            var obj = new SearchParameters<GetAssetsResponse>("GetAssets");
            Assert.AreEqual(1, obj.Page, "Page did not contain expected value");
            Assert.AreEqual(12, obj.PageSize, "PageSize did not contain expected value");
            Assert.AreEqual("GetAssets", obj.SearchName, "SearchName did not contain expected value");
        }

        [Test(Description = "Test Constructor")]
        public void TestConstructor()
        {
            var obj = new SearchParameters<GetAssetsResponse>("GetAssets", 2, 10);
            Assert.AreEqual(2, obj.Page, "Page did not contain expected value");
            Assert.AreEqual(10, obj.PageSize, "PageSize did not contain expected value");
            Assert.AreEqual("GetAssets", obj.SearchName, "SearchName did not contain expected value");
        }

        [Test(Description = "Test Copy Constructor")]
        public void TestCopyConstructor1()
        {
            var srcParams = new SearchParameters("GetAssets", 3, 100) {{"assetId", 1234}};
            var cpyParams = new SearchParameters<GetAssetsResponse>(srcParams);

            Assert.AreEqual("GetAssets", cpyParams.SearchName, "SearchName did not contain expected value");
            Assert.AreEqual("1234", cpyParams["assetId"], "Expected SearchParameter assetId did not contain the expected value");
            Assert.AreEqual(3, cpyParams.Page, "Page did not contain expected value");
            Assert.AreEqual(100, cpyParams.PageSize, "PageSize did not contain expected value");
        }

        [Test(Description = "Test Copy Constructor")]
        public void TestCopyConstructor2()
        {
            var srcParams = new SearchParameters<GetAssetsResponse>("GetAssets", 3, 100) {{"assetId", 1234}};
            var cpyParams = new SearchParameters<GetAssetsResponse>(srcParams);

            Assert.AreEqual("GetAssets", cpyParams.SearchName, "SearchName did not contain expected value");
            Assert.AreEqual("1234", cpyParams["assetId"], "Expected SearchParameter assetId did not contain the expected value");
            Assert.AreEqual(3, cpyParams.Page, "Page did not contain expected value");
            Assert.AreEqual(100, cpyParams.PageSize, "PageSize did not contain expected value");
        }

        [Test(Description = "Test PageSize property")]
        public void TestPageSizeProperty1()
        {
            var par = new SearchParameters("GetAssets");
            par["limit"] = "100";
            Assert.AreEqual(100, par.PageSize);
        }

        [Test(Description = "Test PageSize property")]
        public void TestPageSizeProperty2()
        {
            var par = new SearchParameters("GetAssets");
            par["limit"] = "nolimit";
            var ex = Assert.Throws<InvalidCastException>(() =>
            {
                var res = par.PageSize;
            });
            Assert.AreEqual("The stored page size was not a valid integer. Value was: 'nolimit'", ex.Message);
        }

        [Test(Description = "Test Page property")]
        public void TestPageProperty1()
        {
            var par = new SearchParameters("GetAssets");
            par["page"] = "5";
            Assert.AreEqual(5, par.Page);
        }

        [Test(Description = "Test Page property")]
        public void TestPageProperty2()
        {
            var par = new SearchParameters("GetAssets");
            par["page"] = "first";
            var ex = Assert.Throws<InvalidCastException>(() =>
            {
                var res = par.Page;
            });
            Assert.AreEqual("The stored page was not a valid integer. Value was: 'first'", ex.Message);
        }

        [Test(Description ="Test Method property")]
        public void TestMethodProperty()
        {
            var par = new SearchParameters("GetAssets");
            par.Method = "GET";
            Assert.AreEqual("GET", par.Method, "Method does not contain expected value");
            par.Method = "POST";
            Assert.AreEqual("POST", par.Method, "Method does not contain expected value");
        }

        [Test(Description = "Test Set method for MultiStrings")]
        public void TestSetMultiString1()
        {
            var par = new  SearchParameters("someSearch");
            var values = new List<string>() {"value1", "value2", "value1"};
            par.Set("titles", values);
            Assert.That(par.AllKeys.Contains("titles"));
            Assert.That(par.AllKeys.Contains("titles_type_multistrings"));
            Assert.That(par.GetValues("titles") ,Is.EquivalentTo(values));
            Assert.AreEqual("1", par["titles_type_multistrings"], "titles_type_multistring=1 expected");
            Assert.IsNull(par["titles_type_multiids"], "titles_type_multiids was not expected");
        }

        [Test(Description = "Test Set method for MultiStrings, with empty list")]
        public void TestSetMultiString2()
        {
            var par = new SearchParameters("someSearch");
            var values = new List<string>() { "value1", "value2", "value1"};
            par.Set("titles", values);
            values.Clear();
            par.Set("titles", values);
            Assert.That(!par.AllKeys.Contains("titles"), "titles was not expected");
            Assert.That(!par.AllKeys.Contains("titles_type_multistrings"),"titles_type_multistrings was not expected");
            Assert.IsNull(par["titles"]);
            Assert.IsNull(par["titles_type_multistrings"]);
            Assert.IsNull(par["titles_type_multiids"], "titles_type_multiids was not expected");
        }

        [Test(Description = "Test Set method for MultiIds")]
        public void TestSetMultiIds1()
        {
            var par = new SearchParameters("someSearch");
            var values = new List<int>() { 2, 97, 45,-4122 };
            par.Set("ids", values);
            Assert.That(par.AllKeys.Contains("ids"));
            Assert.That(par.AllKeys.Contains("ids_type_multiids"));
            Assert.That(par.GetValues("ids"), Is.EquivalentTo(values.ConvertAll(x => x.ToString())));
            Assert.AreEqual("1", par["ids_type_multiids"], "ids_type_multiids=1 expected");
            Assert.IsNull(par["ids_type_multistrings"], "ids_type_multistrings was not expected");
        }

        [Test(Description = "Test Set method for MultiIds, with empty list")]
        public void TestSetMultiIds2()
        {
            var par = new SearchParameters("someSearch");
            var values = new List<int>() { 2, 97, 45, -4122 };
            par.Set("ids", values);
            values.Clear();
            par.Set("ids", values);
            Assert.That(!par.AllKeys.Contains("ids"), "ids was not expected");
            Assert.That(!par.AllKeys.Contains("ids_type_multiids"), "ids_type_multiids was not expected");
            Assert.IsNull(par["ids"]);
            Assert.IsNull(par["ids_type_multiids"]);
        }

        [Test]
        public void TestAddStrings()
        {
            var par = new SearchParameters("someSearch");
            var values = new List<string>() { "value1", "value2", "value1" };
            par.Add("titles", values);
            Assert.That(par.AllKeys.Contains("titles"));
            Assert.That(par.AllKeys.Contains("titles_type_multistrings"));
            Assert.That(par.GetValues("titles"), Is.EquivalentTo(values));
            Assert.AreEqual("1", par["titles_type_multistrings"], "titles_type_multistring=1 expected");
            Assert.IsNull(par["titles_type_multiids"], "titles_type_multiids was not expected");
        }
        
        [Test]
        public void TestAddMultiIds()
        {
            var par = new SearchParameters("someSearch");
            var values = new List<int>() { 2, 97, 45, -4122 };
            par.Add("ids", values);
            Assert.That(par.AllKeys.Contains("ids"));
            Assert.That(par.AllKeys.Contains("ids_type_multiids"));
            Assert.That(par.GetValues("ids"), Is.EquivalentTo(values.ConvertAll(x => x.ToString())));
            Assert.AreEqual("1", par["ids_type_multiids"], "ids_type_multiids=1 expected");
            Assert.IsNull(par["ids_type_multistrings"], "ids_type_multistrings was not expected");
        }

        [Test(Description = "Test SetDateBetween method with default dates")]
        public void TestSetDateBetween1()
        {
            var par = new SearchParameters("");
            par.SetDateBetween("dateRange", new DateTime(0), new DateTime(0));
            Assert.That(par.AllKeys.Contains("dateRange"), "Excpected parameter dateRange not found");
            Assert.That(par.AllKeys.Contains("dateRange_end"), "Excpected parameter dateRange_end not found");
            Assert.That(par.AllKeys.Contains("dateRange_type_date"), "Expected _type_date not found");
            Assert.That(par.GetValues("dateRange").Contains("01/01/0001 00:00:00"), "Expected FromDate value not found in parameter dateRange");
            Assert.That(par.GetValues("dateRange_end").Contains("12/31/9999 23:59:59"), "Expected ToDate value not found in parameter dateRange_end");
        }
        [Test(Description = "Test SetDateBetween method with specific start date and default end date")]
        public void TestSetDateBetween2()
        {
            var par = new SearchParameters("");
            par.SetDateBetween("dateRange", new DateTime(2020,2,29), new DateTime(0));
            Assert.That(par.AllKeys.Contains("dateRange"), "Excpected parameter dateRange not found");
            Assert.That(par.AllKeys.Contains("dateRange_end"), "Excpected parameter dateRange_end not found");
            Assert.That(par.AllKeys.Contains("dateRange_type_date"), "Expected _type_date not found");
            Assert.That(par.GetValues("dateRange").Contains("02/29/2020 00:00:00"), "Expected FromDate value not found in parameter dateRange");
            Assert.That(par.GetValues("dateRange_end").Contains("12/31/9999 23:59:59"), "Expected ToDate value not found in parameter dateRange_end");
        }
        [Test(Description = "Test SetDateBetween method with default start date and specific end date")]
        public void TestSetDateBetween3()
        {
            var par = new SearchParameters("");
            par.SetDateBetween("dateRange", new DateTime(), new DateTime(2020, 2, 29));
            Assert.That(par.AllKeys.Contains("dateRange"), "Excpected parameter dateRange not found");
            Assert.That(par.AllKeys.Contains("dateRange_end"), "Excpected parameter dateRange_end not found");
            Assert.That(par.AllKeys.Contains("dateRange_type_date"), "Expected _type_date not found");
            Assert.That(par.GetValues("dateRange").Contains("01/01/0001 00:00:00"), "Expected FromDate value not found in parameter dateRange");
            Assert.That(par.GetValues("dateRange_end").Contains("02/29/2020 00:00:00"), "Expected ToDate value not found in parameter dateRange_end");
        }
        [Test(Description = "Test SetDateBetween method thows an exception if fromdate is after todate")]
        public void TestSetDateBetween4()
        {
            var par = new SearchParameters("");
            var ex = Assert.Throws<ArgumentException>(() => { par.SetDateBetween("dateRange", new DateTime(2020, 2, 2), new DateTime(2019, 2, 28)); });
            Assert.AreEqual("from date is after to date", ex.Message);
        }
    }
}
