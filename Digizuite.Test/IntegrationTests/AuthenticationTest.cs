using System;
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
            Assert.That(() => service.GetAccessKey().GetAwaiter().GetResult(), Throws.TypeOf<AuthenticationException>()
                .With.Property("Message").EqualTo("Authentication failed"));
        }

        [Test]
        public async Task GetMemberId()
        {
            var service = ServiceProvider.GetRequiredService<IDamAuthenticationService>();
            var memberId = await service.GetMemberId().ConfigureAwait(false);
            Assert.AreEqual(30024, memberId);
        }

        [Test]
        public async Task CanImpersonateAccessKey()
        {
            var service = ServiceProvider.GetRequiredService<IDamAuthenticationService>();
            var ak = await service.Impersonate(30024, new DamAuthenticationService.AccessKeyOptions()
            {
                ConfigId = "/1/",
                Duration = new TimeSpan(7, 0, 0, 0),
                LanguageId = 3,
                PersistLanguage = false
            });
            Assert.That(ak, Is.Not.Empty);
        }
    }
}