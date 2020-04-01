﻿using System;
using System.Threading.Tasks;
using Digizuite.Models;
using NUnit.Framework;
using Moq;
using RestSharp;

namespace Digizuite.Test.UnitTests
{
    [TestFixture]
    public class DamRestTest
    {
        [Test(Description="Test that Method is handled correctly in DamRestClient")]
        public async Task TestDamRest1()
        {
            string accessKey = null;
            var configuration = new DigizuiteConfiguration
            {
                BaseUrl = new Uri("https://unittest-dam.dev.digizuite.com"), 
                SystemUsername = "someUser", 
                SystemPassword = "somepwd",
                AccessKeyDuration = new TimeSpan(86400000L)
            };
            var logger = new Mock<ILogger<DamRestClient>>(MockBehavior.Strict);
            var client = new Mock<IRestClient>(MockBehavior.Strict);
            var restRequest = new Mock<RestRequest>(MockBehavior.Strict);
            client.Setup(x => x.UseSerializer(It.IsAny<Func<RestSharp.Serialization.IRestSerializer>>()));
            restRequest.SetupAllProperties();

            var damRest = new DamRestClient(configuration, logger.Object, client.Object);

            client.Setup(x =>
                x.ExecuteAsync<DigiResponse<object>>(
                    It.IsAny<RestRequest>(), It.IsAny<System.Threading.CancellationToken>()
                )
            ).ReturnsAsync(() => null);
            
            await damRest.Execute<DigiResponse<object>>(Method.PATCH, restRequest.Object, accessKey);

            Assert.AreEqual(Method.PATCH, restRequest.Object.Method);
            
            restRequest.VerifyNoOtherCalls();
        }
        [Test(Description = "Test that AccessKey and Method is handled correctly in DamRestClient")]
        public async Task TestDamRest2()
        {
            string accessKey = "some access key";
            var configuration = new DigizuiteConfiguration
            {
                BaseUrl = new Uri("https://unittest-dam.dev.digizuite.com"),
                SystemUsername = "someUser",
                SystemPassword = "somepwd",
                AccessKeyDuration = new TimeSpan(86400000L)
            };
            var logger = new Mock<ILogger<DamRestClient>>(MockBehavior.Strict);
            var client = new Mock<IRestClient>(MockBehavior.Strict);
            var restRequest = new Mock<RestRequest>(MockBehavior.Strict);
            client.Setup(x => x.UseSerializer(It.IsAny<Func<RestSharp.Serialization.IRestSerializer>>()));
            restRequest.SetupAllProperties();

            var damRest = new DamRestClient(configuration, logger.Object, client.Object);

            client.Setup(x =>
                x.ExecuteAsync<DigiResponse<object>>(
                    It.IsAny<RestRequest>(), It.IsAny<System.Threading.CancellationToken>()
                )
            ).ReturnsAsync(() => null);

            await damRest.Execute<DigiResponse<object>>(Method.POST, restRequest.Object, accessKey);

            restRequest.VerifyNoOtherCalls();
            Assert.That(restRequest.Object.Parameters.Find(p =>
                    string.CompareOrdinal(p.Name, DigizuiteConstants.AccessKeyParameter) == 0)?.Value,
                Is.EqualTo(accessKey));
        }
        [Test(Description = "Test that AccessKey and Method is handled correctly in DamRestClient")]
        public async Task TestDamRest3()
        {
            string accessKey = "expected access key";
            var configuration = new DigizuiteConfiguration
            {
                BaseUrl = new Uri("https://unittest-dam.dev.digizuite.com"),
                SystemUsername = "someUser",
                SystemPassword = "somepwd",
                AccessKeyDuration = new TimeSpan(86400000L)
            };
            var logger = new Mock<ILogger<DamRestClient>>(MockBehavior.Strict);
            var client = new Mock<IRestClient>(MockBehavior.Strict);
            var restRequest = new Mock<RestRequest>(MockBehavior.Strict);
            client.Setup(x => x.UseSerializer(It.IsAny<Func<RestSharp.Serialization.IRestSerializer>>()));
            restRequest.SetupAllProperties();
            restRequest.Object.AddParameter(DigizuiteConstants.AccessKeyParameter, "accesskey to be overwritten");
            var damRest = new DamRestClient(configuration, logger.Object, client.Object);

            client.Setup(x =>
                x.ExecuteAsync<DigiResponse<object>>(
                    It.IsAny<RestRequest>(), It.IsAny<System.Threading.CancellationToken>()
                )
            ).ReturnsAsync(() => null);

            await damRest.Execute<DigiResponse<object>>(Method.POST, restRequest.Object, accessKey);

            restRequest.VerifyNoOtherCalls();
            Assert.That(restRequest.Object.Parameters.Find(p =>
                    string.CompareOrdinal(p.Name, DigizuiteConstants.AccessKeyParameter) == 0)?.Value,
                Is.EqualTo(accessKey));
        }
        [Test(Description = "Test that Execute throws an exception if request is null")]
        public  void TestDamRest4()
        {
            string accessKey = "expected access key";
            var configuration = new DigizuiteConfiguration
            {
                BaseUrl = new Uri("https://unittest-dam.dev.digizuite.com"),
                SystemUsername = "someUser",
                SystemPassword = "somepwd",
                AccessKeyDuration = new TimeSpan(86400000L)
            };
            var logger = new Mock<ILogger<DamRestClient>>(MockBehavior.Strict);
            var client = new Mock<IRestClient>(MockBehavior.Strict);
            var restRequest = new Mock<RestRequest>(MockBehavior.Strict);
            client.Setup(x => x.UseSerializer(It.IsAny<Func<RestSharp.Serialization.IRestSerializer>>()));
            restRequest.SetupAllProperties();
            var damRest = new DamRestClient(configuration, logger.Object, client.Object);

            client.Setup(x =>
                x.ExecuteAsync<DigiResponse<object>>(
                    It.IsAny<RestRequest>(), It.IsAny<System.Threading.CancellationToken>()
                )
            ).ReturnsAsync(() => null);

            var ex = Assert.Throws<ArgumentNullException>(() =>
                {
                    damRest.Execute<DigiResponse<object>>(Method.POST, null, accessKey).GetAwaiter().GetResult();

                }
            );
            Assert.That(ex.ParamName, Is.EqualTo("request"));
            restRequest.VerifyNoOtherCalls();
        }
    }
}
