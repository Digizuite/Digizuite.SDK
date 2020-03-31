using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.Models;
using Digizuite.Samples;
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
    public class DamAuthenticationServiceTest
    {
        [Test]
        public async Task TestLoginWithPlaintextPassword()
        {
            var logger = new Mock<ILogger<DamAuthenticationService>>(MockBehavior.Loose);
            var config = new Mock<IConfiguration>(MockBehavior.Strict);
            var client = new Mock<IDamRestClient>(MockBehavior.Strict);
            config.SetupAllProperties();
            config.Object.AccessKeyDuration = TimeSpan.FromDays(14);
            config.Object.SystemUsername = "someUser";
            config.Object.SystemPassword = "somepwd";
            config.Object.BaseUrl = new Uri("https://unittest-dam.dev.digizuite.com");

            client.Setup(x =>
                    x.Execute<DigiResponse<AuthenticateResponse>>(It.IsAny<Method>(), It.IsAny<RestRequest>(),
                        It.IsAny<string>()))
                .ReturnsAsync((Method method, RestRequest request, string akey) =>
                {
                    Assert.IsNull(akey);
                    Assert.That(method, Is.EqualTo(Method.POST), "Execute should be called with Method.POST");
                    Assert.That(request.Resource, Is.EqualTo("ConnectService.js"));
                    Assert.That(request.Parameters.Exists(p => p.Name == "method" && p.Value.Equals("CreateAccesskey")),
                        Is.True, "Expected method is not in Parameters");

                    Assert.That(request.Parameters.Exists(p => p.Name == "username" && p.Value.Equals("someUser")),
                        Is.True, "Expected username is not in Parameters");
                    Assert.That(
                        request.Parameters.Exists(p =>
                            p.Name == "password" && p.Value.Equals("a3b52f8cc26266e390a7fcc0cf3f9a69")), Is.True,
                        "Expected password is not in Parameters");

                    Assert.That(request.Parameters.Exists(p => p.Name == "usertype" && p.Value.Equals(2)), Is.True,
                        "Expected usertype is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == "useversionedmetadata" && p.Value.Equals(0)),
                        Is.True, "Expected useversionedmetadata is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == "page" && ((int) p.Value) >= 1), Is.True,
                        "Expected parameter page is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == "limit" && ((int) p.Value) >= 1), Is.True,
                        "Expected parameter limit is not in Parameters");

                    var response = new RestResponse<DigiResponse<AuthenticateResponse>>()
                    {
                        Data = new DigiResponse<AuthenticateResponse>()
                        {
                            Success = true,
                            Total = 1,
                            Items = new List<AuthenticateResponse>()
                            {
                                new AuthenticateResponse()
                                {
                                    AccessKey = "e656f5fb-6c02-4f50-be00-df6c703a5387",
                                    CsrfToken =
                                        "8581689986:45efc66ade01fa84d1d158b8da948b046b345ce6a95ecada85cb4c5749659bab",
                                    MemberId = "30024",
                                    Itemid = "10174",
                                    LanguageId = "3",
                                    Expiration = DateTime.Now.AddDays(1),
                                }
                            }
                        }
                    };
                    return response;
                });


            using var auth = new DamAuthenticationService(config.Object, client.Object, logger.Object);

            var accessKey = await auth.GetAccessKey();
            client.Verify(
                cli => cli.Execute<DigiResponse<AuthenticateResponse>>(It.IsAny<Method>(), It.IsAny<RestRequest>(),
                    It.IsAny<string>()), Times.Once);
            client.VerifyNoOtherCalls();

            Assert.AreEqual("e656f5fb-6c02-4f50-be00-df6c703a5387", accessKey, "Did not return expected accesskey");
        }

        [Test]
        public async Task TestLoginWithMd5Password()
        {
            var logger = new Mock<ILogger<DamAuthenticationService>>(MockBehavior.Loose);
            var config = new Mock<IConfiguration>(MockBehavior.Strict);
            var client = new Mock<IDamRestClient>(MockBehavior.Strict);
            config.SetupAllProperties();
            config.Object.AccessKeyDuration = TimeSpan.FromDays(14);
            config.Object.SystemUsername = "someUser";
            config.Object.SystemPassword = "a3b52f8cc26266e390a7fcc0cf3f9a69";
            config.Object.BaseUrl = new Uri("https://unittest-dam.dev.digizuite.com");

            client.Setup(x =>
                    x.Execute<DigiResponse<AuthenticateResponse>>(It.IsAny<Method>(), It.IsAny<RestRequest>(),
                        It.IsAny<string>()))
                .ReturnsAsync((Method method, RestRequest request, string akey) =>
                {
                    Assert.IsNull(akey);
                    Assert.That(method, Is.EqualTo(Method.POST), "Execute should be called with Method.POST");
                    Assert.That(request.Resource, Is.EqualTo("ConnectService.js"));
                    Assert.That(request.Parameters.Exists(p => p.Name == "method" && p.Value.Equals("CreateAccesskey")),
                        Is.True, "Expected method is not in Parameters");

                    Assert.That(request.Parameters.Exists(p => p.Name == "username" && p.Value.Equals("someUser")),
                        Is.True, "Expected username is not in Parameters");
                    Assert.That(
                        request.Parameters.Exists(p =>
                            p.Name == "password" && p.Value.Equals("a3b52f8cc26266e390a7fcc0cf3f9a69")), Is.True,
                        "Expected password is not in Parameters");

                    Assert.That(request.Parameters.Exists(p => p.Name == "usertype" && p.Value.Equals(2)), Is.True,
                        "Expected usertype is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == "useversionedmetadata" && p.Value.Equals(0)),
                        Is.True, "Expected useversionedmetadata is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == "page" && ((int) p.Value) >= 1), Is.True,
                        "Expected parameter page is not in Parameters");
                    Assert.That(request.Parameters.Exists(p => p.Name == "limit" && ((int) p.Value) >= 1), Is.True,
                        "Expected parameter limit is not in Parameters");


                    var response = new RestResponse<DigiResponse<AuthenticateResponse>>()
                    {
                        Data = new DigiResponse<AuthenticateResponse>()
                        {
                            Success = true,
                            Total = 1,
                            Items = new List<AuthenticateResponse>()
                            {
                                new AuthenticateResponse()
                                {
                                    AccessKey = "e656f5fb-6c02-4f50-be00-df6c703a5387",
                                    CsrfToken =
                                        "8581689986:45efc66ade01fa84d1d158b8da948b046b345ce6a95ecada85cb4c5749659bab",
                                    MemberId = "30024",
                                    Itemid = "10174",
                                    LanguageId = "3",
                                    Expiration = DateTime.Now.AddDays(1),
                                }
                            }
                        }
                    };
                    return response;
                });


            using var auth = new DamAuthenticationService(config.Object, client.Object, logger.Object);

            var accessKey = await auth.GetAccessKey();

            client.Verify(
                cli => cli.Execute<DigiResponse<AuthenticateResponse>>(It.IsAny<Method>(), It.IsAny<RestRequest>(),
                    It.IsAny<string>()), Times.Once);
            client.VerifyNoOtherCalls();

            Assert.AreEqual("e656f5fb-6c02-4f50-be00-df6c703a5387", accessKey, "Did not return expected accesskey");
        }

        [Test]
        public void TestFailedLogin()
        {
            var logger = new Mock<ILogger<DamAuthenticationService>>(MockBehavior.Loose);
            var config = new Mock<IConfiguration>(MockBehavior.Strict);
            var client = new Mock<IDamRestClient>(MockBehavior.Strict);
            config.SetupAllProperties();
            config.Object.AccessKeyDuration = TimeSpan.FromDays(14);
            config.Object.SystemUsername = "someUser";
            config.Object.SystemPassword = "f3b52f8cc26266e390a7fcc0cf3f9a60";
            config.Object.BaseUrl = new Uri("https://unittest-dam.dev.digizuite.com");

            client.Setup(x =>
                    x.Execute<DigiResponse<AuthenticateResponse>>(It.IsAny<Method>(), It.IsAny<RestRequest>(),
                        It.IsAny<string>()))
                .ReturnsAsync((Method method, RestRequest request, string akey) =>
                {
                    var response = new RestResponse<DigiResponse<AuthenticateResponse>>()
                    {
                        Data = new DigiResponse<AuthenticateResponse>()
                        {
                            Success = false,
                            Total = 0,
                            Items = null,
                        }
                    };
                    return response;
                });


            using var auth = new DamAuthenticationService(config.Object, client.Object, logger.Object);
            var ex = Assert.ThrowsAsync<Exceptions.AuthenticationException>(async () =>
            {
                // ReSharper disable once AccessToDisposedClosure
                var accessKey = await auth.GetAccessKey();
                Assert.IsNull(accessKey, "Expected GetAccessKey to return null when authentication fails");
            });
            Assert.AreEqual("Authentication failed", ex.Message);

            client.Verify(
                cli => cli.Execute<DigiResponse<AuthenticateResponse>>(It.IsAny<Method>(), It.IsAny<RestRequest>(),
                    It.IsAny<string>()), Times.Once);
            client.VerifyNoOtherCalls();
        }

        [Test]
        public async Task TestReUseLogin()
        {
            var logger = new Mock<ILogger<DamAuthenticationService>>(MockBehavior.Loose);
            var config = new Mock<IConfiguration>(MockBehavior.Strict);
            var client = new Mock<IDamRestClient>(MockBehavior.Strict);
            config.SetupAllProperties();
            config.Object.AccessKeyDuration = TimeSpan.FromDays(14);
            config.Object.SystemUsername = "someUser";
            config.Object.SystemPassword = "a3b52f8cc26266e390a7fcc0cf3f9a69";
            config.Object.BaseUrl = new Uri("https://unittest-dam.dev.digizuite.com");

            client.Setup(x =>
                    x.Execute<DigiResponse<AuthenticateResponse>>(It.IsAny<Method>(), It.IsAny<RestRequest>(),
                        It.IsAny<string>()))
                .ReturnsAsync((Method method, RestRequest request, string akey) =>
                {
                    var response = new RestResponse<DigiResponse<AuthenticateResponse>>()
                    {
                        Data = new DigiResponse<AuthenticateResponse>()
                        {
                            Success = true,
                            Total = 1,
                            Items = new List<AuthenticateResponse>()
                            {
                                new AuthenticateResponse()
                                {
                                    AccessKey = "e656f5fb-6c02-4f50-be00-df6c703a5387",
                                    CsrfToken =
                                        "8581689986:45efc66ade01fa84d1d158b8da948b046b345ce6a95ecada85cb4c5749659bab",
                                    MemberId = "30024",
                                    Itemid = "10174",
                                    LanguageId = "3",
                                    Expiration = DateTime.Now.AddDays(1),
                                }
                            }
                        }
                    };
                    return response;
                });


            using var auth = new DamAuthenticationService(config.Object, client.Object, logger.Object);

            var accessKey1 = await auth.GetAccessKey();
            var accessKey2 = await auth.GetAccessKey();

            client.Verify(
                cli => cli.Execute<DigiResponse<AuthenticateResponse>>(It.IsAny<Method>(), It.IsAny<RestRequest>(),
                    It.IsAny<string>()), Times.Once);
            client.VerifyNoOtherCalls();

            Assert.AreEqual(accessKey1, accessKey2, "did not reuse login");
            Assert.AreEqual("e656f5fb-6c02-4f50-be00-df6c703a5387", accessKey2, "Did not return expected accesskey");
        }

        [Test]
        public async Task TestGetMemberId()
        {
            var logger = new Mock<ILogger<DamAuthenticationService>>(MockBehavior.Loose);
            var config = new Mock<IConfiguration>(MockBehavior.Strict);
            var client = new Mock<IDamRestClient>(MockBehavior.Strict);
            config.SetupAllProperties();
            config.Object.AccessKeyDuration = TimeSpan.FromDays(14);
            config.Object.SystemUsername = "someUser";
            config.Object.SystemPassword = "a3b52f8cc26266e390a7fcc0cf3f9a69";
            config.Object.BaseUrl = new Uri("https://unittest-dam.dev.digizuite.com");

            client.Setup(x =>
                    x.Execute<DigiResponse<AuthenticateResponse>>(It.IsAny<Method>(), It.IsAny<RestRequest>(),
                        It.IsAny<string>()))
                .ReturnsAsync((Method method, RestRequest request, string akey) =>
                {
                    var response = new RestResponse<DigiResponse<AuthenticateResponse>>()
                    {
                        Data = new DigiResponse<AuthenticateResponse>()
                        {
                            Success = true,
                            Total = 1,
                            Items = new List<AuthenticateResponse>()
                            {
                                new AuthenticateResponse()
                                {
                                    AccessKey = "e656f5fb-6c02-4f50-be00-df6c703a5387",
                                    CsrfToken =
                                        "8581689986:45efc66ade01fa84d1d158b8da948b046b345ce6a95ecada85cb4c5749659bab",
                                    MemberId = "30024",
                                    Itemid = "10174",
                                    LanguageId = "3",
                                    Expiration = DateTime.Now.AddDays(1),
                                }
                            }
                        }
                    };
                    return response;
                });


            using var auth = new DamAuthenticationService(config.Object, client.Object, logger.Object);

            var memberId = await auth.GetMemberId();

            client.Verify(
                cli => cli.Execute<DigiResponse<AuthenticateResponse>>(It.IsAny<Method>(), It.IsAny<RestRequest>(),
                    It.IsAny<string>()), Times.Once);
            client.VerifyNoOtherCalls();

            Assert.AreEqual(30024, memberId, "Did not return expected accesskey");
        }


        [Test]
        public async Task PassesInConfigVersionIdIfSpecified()
        {
            var restClient = await GetRestClientFromLogin("foo", "bar");
            var cfgId = restClient.Request.Parameters.Single(p => p.Name == "configversionid");
            Assert.That(cfgId.Value, Is.EqualTo("foo"));

            var dataId = restClient.Request.Parameters.Single(p => p.Name == "dataversionid");
            Assert.That(dataId.Value, Is.EqualTo("bar"));
        }

        [Test]
        public async Task ShouldDefaultToConfigVersionIdWhenDataVersionIdIsNotSpecified()
        {
            var restClient = await GetRestClientFromLogin("foo");
            var cfgId = restClient.Request.Parameters.Single(p => p.Name == "configversionid");
            Assert.That(cfgId.Value, Is.EqualTo("foo"));

            var dataId = restClient.Request.Parameters.Single(p => p.Name == "dataversionid");
            Assert.That(dataId.Value, Is.EqualTo("foo"));
        }

        [Test]
        public async Task ShouldNotSetConfigVersionIdAndDataVersionIdWhenNotSpecified()
        {
            var restClient = await GetRestClientFromLogin();
            
            Assert.That(restClient.Request.Parameters.Any(p => p.Name == "configversionid"), Is.False);
            Assert.That(restClient.Request.Parameters.Any(p => p.Name == "dataversionid"), Is.False);
        }
        
        private async Task<FakeDamRestClient> GetRestClientFromLogin(string configVersionId = null, string dataVersionId = null)
        {
            var config = new DigizuiteConfiguration()
            {
                BaseUrl = new Uri("https://unittest-dam.dev.digizuite.com"),
                SystemPassword = "test",
                SystemUsername = "test",
                ConfigVersionId = configVersionId,
                DataVersionId = dataVersionId
            };

            var restClient = new FakeDamRestClient(new DigiResponse<AuthenticateResponse>()
            {
                Success = true,
                Items = new List<AuthenticateResponse>
                {
                    new AuthenticateResponse
                    {
                        Expiration = DateTime.Now.Add(TimeSpan.FromDays(1)),
                        Itemid = "2000",
                        AccessKey = "ff",
                        CsrfToken = "f",
                        LanguageId = "3",
                        MemberId = "5862"
                    }
                }
            });

            using var service =
                new DamAuthenticationService(config, restClient, new ConsoleLogger<DamAuthenticationService>());

            await service.GetAccessKey();

            return restClient;
        }

        private class FakeDamRestClient : IDamRestClient
        {
            public Method Method;
            public RestRequest Request;
            public string AccessKey;

            public object Response;

            public FakeDamRestClient(object response)
            {
                Response = response;
            }

            public Task<IRestResponse<T>> Execute<T>(Method method, RestRequest request, string accessKey = null)
            {
                Method = method;
                Request = request;
                AccessKey = accessKey;

                var res = new RestResponse<T>();
                res.Data = (T) Response;
                return Task.FromResult((IRestResponse<T>) res);
            }
        }
    }
}