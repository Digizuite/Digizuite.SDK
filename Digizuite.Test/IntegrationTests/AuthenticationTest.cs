using System.Threading.Tasks;
using Digizuite.Exceptions;
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
            var ak = await service.GetAccessKey().ConfigureAwait(false);
            Assert.That(ak, Is.Not.Empty);
        }

        [Test]
        public void CanGetAccessKey_WrongPassword()
        {
            var service = ServiceProvider.GetRequiredService<IDamAuthenticationService>();
            Configuration.SystemPassword = new string('0', 36);
            var exceptionThrown =
                Assert.Throws<AuthenticationException>(() => service.GetAccessKey().GetAwaiter().GetResult());
            Assert.AreEqual("Authentication failed", exceptionThrown.Message);
        }

        [Test]
        public async Task GetMemberId()
        {
            var service = ServiceProvider.GetRequiredService<IDamAuthenticationService>();
            var memberId = await service.GetMemberId().ConfigureAwait(false);
            Assert.AreEqual(30024, memberId);
        }
    }
}