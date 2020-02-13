
using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Models;
using Digizuite.Models.Search;
using Moq;
using NUnit.Framework;
using RestSharp;
// ReSharper disable UnusedVariable
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable RedundantCast
// ReSharper disable UseObjectOrCollectionInitializer

namespace Digizuite.Test.UnitTests
{
    [TestFixture]
    public class SearchServiceTest
    {
        [Test(Description = "Verify search without accesskey")]
        public async Task TestSearchWithoutAccessKey()
        {
            var client = new Mock<IDamRestClient>(MockBehavior.Strict);
            var auth = new Mock<IDamAuthenticationService>(MockBehavior.Strict);
            var logger = new Mock<ILogger<SearchService>>(MockBehavior.Loose);
            auth.Setup(x => x.GetAccessKey())
                .ReturnsAsync(() => "someAccessKey");

            client.Setup(x => x.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()))
                .ReturnsAsync((Method method,RestRequest request, string accessKey) =>
                {
                    Assert.That(accessKey, Is.EqualTo("someAccessKey"), "AccessKey should be parsed on to DamRestClient");
                    Assert.That(method, Is.EqualTo(Method.POST), "Execute should be called with Method.POST");
                    Assert.That(request.Resource, Is.EqualTo("SearchService.js"));
                    
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.PageKey}" && p.Value.Equals("5")), Is.True, "Expected Page is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.PageSizeKey}" && p.Value.Equals("3")), Is.True, "Expected PageSize is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.SearchNameKey}" && p.Value.Equals("GetAssets")), Is.True, "Expected SearchName is not in Parameters");
                    var response = new RestResponse<DigiResponse<Asset>>()
                    {
                        Data = new DigiResponse<Asset>()
                        {
                            Success = true,
                            Total = 22,
                            Items = new List<Asset>()
                            {
                                new Asset() {AssetId = 1, ItemId = 9001, AssetType = AssetType.META },
                                new Asset() {AssetId = 2, ItemId = 9002, AssetType = AssetType.META },
                                new Asset() {AssetId = 3, ItemId = 9003, AssetType = AssetType.META }
                            }
                        }
                    };
                    return response;
                });

            var src = new SearchService(client.Object, auth.Object, logger.Object);
            var parameters = new SearchParameters("GetAssets", 5, 3);
            var result = await src.Search<Asset>(parameters, (string) null).ConfigureAwait(true);
            
            auth.Verify(x => x.GetAccessKey(), Times.Once,"if Search does not have n accessKey argument, Search should call GetAccessKey on the AuthService");
            auth.VerifyNoOtherCalls();

            client.Verify(cli => cli.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "Verify search without accesskey")]
        public async Task TestSearchWithAccessKey()
        {
            var client = new Mock<IDamRestClient>(MockBehavior.Strict);
            var auth = new Mock<IDamAuthenticationService>(MockBehavior.Strict);
            var logger = new Mock<ILogger<SearchService>>(MockBehavior.Loose);
            auth.Setup(x => x.GetAccessKey())
                .ReturnsAsync(() => "someAccessKey");

            client.Setup(x => x.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()))
                .ReturnsAsync((Method method, RestRequest request, string accessKey) =>
                {
                    Assert.That(accessKey, Is.EqualTo("specifiedAccessKey"), "AccessKey should be parsed on to DamRestClient");
                    Assert.That(method, Is.EqualTo(Method.POST), "Execute should be called with Method.POST");
                    Assert.That(request.Resource, Is.EqualTo("SearchService.js"));

                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.PageKey}" && p.Value.Equals("5")), Is.True, "Expected Page is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.PageSizeKey}" && p.Value.Equals("3")), Is.True, "Expected PageSize is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.SearchNameKey}" && p.Value.Equals("GetAssets")), Is.True, "Expected SearchName is not in Parameters");
                    var response = new RestResponse<DigiResponse<Asset>>()
                    {
                        Data = new DigiResponse<Asset>()
                        {
                            Success = true,
                            Total = 22,
                            Items = new List<Asset>()
                            {
                                new Asset() {AssetId = 1, ItemId = 9001, AssetType = AssetType.META },
                                new Asset() {AssetId = 2, ItemId = 9002, AssetType = AssetType.META },
                                new Asset() {AssetId = 3, ItemId = 9003, AssetType = AssetType.META }
                            }
                        }
                    };
                    return response;
                });

            var src = new SearchService(client.Object, auth.Object, logger.Object);
            var parameters = new SearchParameters("GetAssets", 5, 3);
            var result = await src.Search<Asset>(parameters, "specifiedAccessKey").ConfigureAwait(true);

            auth.Verify(x => x.GetAccessKey(), Times.Never, "Should not call GetAccessKey, if accessKey is supplied");
            auth.VerifyNoOtherCalls();

            client.Verify(cli => cli.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "Verify search parameters with null value is removed")]
        public async Task TestParametersWithNullvalueRemoved()
        {
            var client = new Mock<IDamRestClient>(MockBehavior.Strict);
            var auth = new Mock<IDamAuthenticationService>(MockBehavior.Strict);
            var logger = new Mock<ILogger<SearchService>>(MockBehavior.Loose);
            auth.Setup(x => x.GetAccessKey())
                .ReturnsAsync(() => "someAccessKey");

            client.Setup(x => x.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()))
                .ReturnsAsync((Method method, RestRequest request, string accessKey) =>
                {
                    Assert.That(accessKey, Is.EqualTo("someAccessKey"), "AccessKey should be parsed on to DamRestClient");
                    Assert.That(method, Is.EqualTo(Method.POST), "Execute should be called with Method.POST");
                    Assert.That(request.Resource, Is.EqualTo("SearchService.js"));

                    Assert.That(request.Parameters.Exists(p => p.Value == null), Is.False, "Parameters with NULL value is not removed");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.PageKey}" && p.Value.Equals("5")), Is.True, "Expected Page is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.PageSizeKey}" && p.Value.Equals("3")), Is.True, "Expected PageSize is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.SearchNameKey}" && p.Value.Equals("GetAssets")), Is.True, "Expected SearchName is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == "Title" && p.Value.Equals("some title")), Is.True, "Expected Title is not in Parameters");
                    var response = new RestResponse<DigiResponse<Asset>>()
                    {
                        Data = new DigiResponse<Asset>()
                        {
                            Success = true,
                            Total = 22,
                            Items = new List<Asset>()
                            {
                                new Asset() {AssetId = 1, ItemId = 9001, AssetType = AssetType.META },
                                new Asset() {AssetId = 2, ItemId = 9002, AssetType = AssetType.META },
                                new Asset() {AssetId = 3, ItemId = 9003, AssetType = AssetType.META }
                            }
                        }
                    };
                    return response;
                });

            var src = new SearchService(client.Object, auth.Object, logger.Object);
            var parameters = new SearchParameters("GetAssets", 5, 3);
            parameters.Add("NullParam", (string)null);
            parameters.Add("Title", "some title");
            var result = await src.Search<Asset>(parameters, null).ConfigureAwait(true);

            auth.Verify(x => x.GetAccessKey(), Times.Once, "if Search does not have n accessKey argument, Search should call GetAccessKey on the AuthService");
            auth.VerifyNoOtherCalls();

            client.Verify(cli => cli.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "Verify search throws SearchFailedException if search fails")]
        public void TestSearchFailedException()
        {
            var client = new Mock<IDamRestClient>(MockBehavior.Strict);
            var auth = new Mock<IDamAuthenticationService>(MockBehavior.Strict);
            var logger = new Mock<ILogger<SearchService>>(MockBehavior.Loose);
            auth.Setup(x => x.GetAccessKey())
                .ReturnsAsync(() => "someAccessKey");

            client.Setup(x => x.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()))
                .ReturnsAsync((Method method, RestRequest request, string accessKey) =>
                {
                    Assert.That(accessKey, Is.EqualTo("someAccessKey"), "AccessKey should be parsed on to DamRestClient");
                    Assert.That(method, Is.EqualTo(Method.POST), "Execute should be called with Method.POST");
                    Assert.That(request.Resource, Is.EqualTo("SearchService.js"));
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.PageKey}" && p.Value.Equals("1")), Is.True, "Expected Page is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.PageSizeKey}" && p.Value.Equals("1")), Is.True, "Expected PageSize is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.SearchNameKey}" && p.Value.Equals("GetAssets")), Is.True, "Expected SearchName is not in Parameters");
                    var response = new RestResponse<DigiResponse<Asset>>()
                    {
                        Data = new DigiResponse<Asset>()
                        {
                            Success = false,
                            Total = 0,
                            Items = null
                        }
                    };
                    return response;
                });

            var src = new SearchService(client.Object, auth.Object, logger.Object);
            var parameters = new SearchParameters("GetAssets", 1, 1);
            
            var ex = Assert.Throws<SearchFailedException>(() =>
                {
                    src.Search<Asset>(parameters).GetAwaiter().GetResult();
                }
            );
            auth.Verify(x => x.GetAccessKey(), Times.Once, "if Search does not have n accessKey argument, Search should call GetAccessKey on the AuthService");
            auth.VerifyNoOtherCalls();
            client.Verify(cli => cli.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "Verify search with templated search")]
        public async Task CanSearchMultiplePages()
        {
            var firstClient = new Mock<IDamRestClient>(MockBehavior.Loose);
            var secondClient = new Mock<IDamRestClient>(MockBehavior.Strict);
            var auth = new Mock<IDamAuthenticationService>(MockBehavior.Strict);
            var logger = new Mock<ILogger<SearchService>>(MockBehavior.Loose);

            var firstSearch = new SearchService(firstClient.Object, auth.Object, logger.Object);

            firstClient.Setup(x => x.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()))
                .ReturnsAsync((Method method, RestRequest request, string accessKey) =>
                {
                    var response = new RestResponse<DigiResponse<Asset>>()
                    {
                        Data = new DigiResponse<Asset>()
                        {
                            Success = true,
                            Total = 22,
                            Items = new List<Asset>()
                            {
                                new Asset() {AssetId = 1, ItemId = 9001, AssetType = AssetType.META, Name = "asset_1", PrevRef = 0, Thumb = null, ImagePreview = null, UploadMemberId = 23, VideoPreview = null },
                                new Asset() {AssetId = 2, ItemId = 9002, AssetType = AssetType.META, Name = "asset_2", PrevRef = 0, Thumb = null, ImagePreview = null, UploadMemberId = 23, VideoPreview = null  },
                            }
                        }
                    };
                    return response;
                });

            var secondSearch = new SearchService(secondClient.Object, auth.Object, logger.Object);

            secondClient.Setup(x => x.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()))
                .ReturnsAsync((Method method, RestRequest request, string accessKey) =>
                {

                    Assert.That(accessKey, Is.EqualTo("excpected_assetkey"), "AccessKey should be parsed on to DamRestClient");
                    Assert.That(method, Is.EqualTo(Method.POST), "Execute should be called with Method.POST");
                    Assert.That(request.Resource, Is.EqualTo("SearchService.js"));

                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.PageKey}" && p.Value.Equals("2")), Is.True, "Expected Page is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.PageSizeKey}" && p.Value.Equals("2")), Is.True, "Expected PageSize is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == $"{SearchParameters.SearchNameKey}" && p.Value.Equals("GetAssets")), Is.True, "Expected SearchName is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == "sCatalogFolderId" && p.Value.Equals("40")), Is.True, "Expected sCatalogFolderId is not in Parameters");

                    var response = new RestResponse<DigiResponse<Asset>>()
                    {
                        Data = new DigiResponse<Asset>()
                        {
                            Success = true,
                            Total = 22,
                            Items = new List<Asset>()
                            {
                                new Asset() {AssetId = 3, ItemId = 9003, AssetType = AssetType.META },
                                new Asset() {AssetId = 4, ItemId = 9004, AssetType = AssetType.META },
                            }
                        }
                    };
                    return response;
                });

            var parameters = new SearchParameters("GetAssets", 1, 2)
            {
                {"sCatalogFolderId", "40"}
            };

            var firstResult = await firstSearch.Search<Asset>(parameters,"expected_assetkey").ConfigureAwait(false);
            var secondResult = await secondSearch.Search(firstResult.Next,"excpected_assetkey").ConfigureAwait(false);

            firstClient.Verify(cli => cli.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()), Times.Once);
            secondClient.Verify(cli => cli.Execute<DigiResponse<Asset>>(It.IsAny<Method>(), It.IsAny<RestRequest>(), It.IsAny<string>()), Times.Once);
            secondClient.VerifyNoOtherCalls();

            var expectedResult = new List<Asset>()
            {
                new Asset() {AssetId = 3, ItemId = 9003, AssetType = AssetType.META},
                new Asset() {AssetId = 4, ItemId = 9004, AssetType = AssetType.META},
            };
            Assert.That(secondResult.Items, Is.EquivalentTo(expectedResult));
        }
    }
}
