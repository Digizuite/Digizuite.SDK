using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test
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
    }
}