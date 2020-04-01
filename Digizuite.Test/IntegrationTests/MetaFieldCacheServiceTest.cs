using System.Threading.Tasks;
using Digizuite.Metadata;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class MetaFieldCacheServiceTest : IntegrationTestBase
    {
        [Test]
        public async Task LoadsMetaFields()
        {
            var service = ServiceProvider.GetRequiredService<IMetaFieldCacheService>();

            var metafields = await service.GetMetaFields();
            
            Assert.That(metafields, Is.Not.Empty);
        }
    }
}