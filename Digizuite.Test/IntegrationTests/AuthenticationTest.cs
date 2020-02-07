using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class AuthenticationTest : IntegrationTestBase
    {
        [Test]
        public async Task CanGetAccessKey()
        {

            var service = ServiceProvider.GetRequiredService<IDamAuthenticationService>();
            var ak = await service.GetAccessKey();
            Assert.That(ak, Is.Not.Empty);
        }
        [Test]
        public void CanGetAccessKey_WrongPassword()
        {
            var service = ServiceProvider.GetRequiredService<IDamAuthenticationService>();
            Configuration.SystemPassword = new string('0', 36);
            var exceptionThrown = Assert.Throws<Exception>(() => service.GetAccessKey().GetAwaiter().GetResult());
            Assert.AreEqual("Request was unsuccessful", exceptionThrown.Message);
        }

        [Test]
        public async Task GetMemberId()
        {
            var service = ServiceProvider.GetRequiredService<IDamAuthenticationService>();
            var memberId = await service.GetMemberId();
            Assert.AreEqual(30024, memberId);
        }
    }
}