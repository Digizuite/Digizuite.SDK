using System;
using System.Threading.Tasks;
using Digizuite.Extensions;
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
            };
            var client = new Mock<IRestClient>(MockBehavior.Strict);
            var restRequest = new Mock<RestRequest>(MockBehavior.Strict);
            client.Setup(x => x.UseSerializer(It.IsAny<Func<RestSharp.Serialization.IRestSerializer>>()));
            restRequest.SetupAllProperties();

            var damRest = new DamRestClient(configuration, client.Object);

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
            };
            var client = new Mock<IRestClient>(MockBehavior.Strict);
            var restRequest = new Mock<RestRequest>(MockBehavior.Strict);
            client.Setup(x => x.UseSerializer(It.IsAny<Func<RestSharp.Serialization.IRestSerializer>>()));
            restRequest.SetupAllProperties();

            var damRest = new DamRestClient(configuration, client.Object);

            client.Setup(x =>
                x.ExecuteAsync<DigiResponse<object>>(
                    It.IsAny<RestRequest>(), It.IsAny<System.Threading.CancellationToken>()
                )
            ).ReturnsAsync(() => null);

            await damRest.Execute<DigiResponse<object>>(Method.POST, restRequest.Object, accessKey);

            restRequest.VerifyNoOtherCalls();
            Assert.That(restRequest.Object.Parameters.Find(p =>
                    string.Equals(p.Name, "Authorization", StringComparison.Ordinal))?.Value,
                Is.EqualTo("AccessKey some access key"));
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
            };
            var client = new Mock<IRestClient>(MockBehavior.Strict);
            var restRequest = new Mock<RestRequest>(MockBehavior.Strict);
            client.Setup(x => x.UseSerializer(It.IsAny<Func<RestSharp.Serialization.IRestSerializer>>()));
            restRequest.SetupAllProperties();
            restRequest.Object.AddAccessKey("accesskey to be overwritten");
            var damRest = new DamRestClient(configuration, client.Object);

            client.Setup(x =>
                x.ExecuteAsync<DigiResponse<object>>(
                    It.IsAny<RestRequest>(), It.IsAny<System.Threading.CancellationToken>()
                )
            ).ReturnsAsync(() => null);

            await damRest.Execute<DigiResponse<object>>(Method.POST, restRequest.Object, accessKey);

            restRequest.VerifyNoOtherCalls();
            Assert.That(restRequest.Object.Parameters.Find(p =>
                    string.CompareOrdinal(p.Name, "Authorization") == 0)?.Value,
                Is.EqualTo("AccessKey " + accessKey));
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
            };
            var client = new Mock<IRestClient>(MockBehavior.Strict);
            var restRequest = new Mock<RestRequest>(MockBehavior.Strict);
            client.Setup(x => x.UseSerializer(It.IsAny<Func<RestSharp.Serialization.IRestSerializer>>()));
            restRequest.SetupAllProperties();
            var damRest = new DamRestClient(configuration, client.Object);

            client.Setup(x =>
                x.ExecuteAsync<DigiResponse<object>>(
                    It.IsAny<RestRequest>(), It.IsAny<System.Threading.CancellationToken>()
                )
            ).ReturnsAsync(() => null);

            var ex = Assert.Throws<ArgumentNullException>(() =>
                {
                    damRest.Execute<DigiResponse<object>>(Method.POST, null!, accessKey).GetAwaiter().GetResult();

                }
            );
            Assert.That(ex.ParamName, Is.EqualTo("request"));
            restRequest.VerifyNoOtherCalls();
        }
    }
}
