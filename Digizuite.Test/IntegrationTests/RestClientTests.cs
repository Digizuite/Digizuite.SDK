using System.Threading.Tasks;
using Digizuite.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class RestClientTests : IntegrationTestBase
    {
        
        protected override void SetupDependencies(IServiceCollection services)
        {
            services.Replace(new ServiceDescriptor(typeof(ILogger<>), typeof(NoLogger<>), ServiceLifetime.Singleton));
            base.SetupDependencies(services);
        }

        class NoLogger<T> : ConsoleLogger<T>
        {
            public override bool IsLogLevelEnabled(LogLevel level)
            {
                return false;
            }
        }

        /// <summary>
        /// Long story short: If the logger is in debug mode, it dumps the whole api response
        /// for, you know, easier debugging. However when not in debug mode it uses a more aggressive
        /// streaming model to minimize memory usage. That however was not working, thus, this test. 
        /// </summary>
        [Test]
        [Timeout(5000)]
        public async Task WorksWhenDebugLoggingIsNotEnabled()
        {
            // Just use the access key service to test, since that is the first thing people will have problems with
            var ak = await ServiceProvider.GetRequiredService<IDamAuthenticationService>().GetAccessKey();
            Assert.That(ak, Is.Not.Null);

            var metaAsset = await ServiceProvider.GetRequiredService<IAssetService>()
                .CreateMetaAsset("rest client rest", 40);
            Assert.That(metaAsset, Is.Not.Null);
        }
        
    }
}
