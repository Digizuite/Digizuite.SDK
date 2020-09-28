using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.Metadata;
using Digizuite.Models.Metadata.Fields;
using Digizuite.Models.Metadata.Values;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests.Metadata
{
    [TestFixture]
    public class MetadataUpdateServiceTest : IntegrationTestBase<IMetadataUpdateService>
    {
        private const int testAssetItemId = 12083;
        private const int testMetaFieldId = 50550;
        private const int testMetaFieldItemId = 12087;

        private async Task<ComboMetafield> GetField()
        {
            var valueService = ServiceProvider.GetRequiredService<IMetadataValueService>();
            return await valueService.GetComboMetafield(testAssetItemId, testMetaFieldItemId);
        }

        private async Task UpdateField(ComboMetafield field)
        {
            var valueService = ServiceProvider.GetRequiredService<IMetadataValueService>();

            await valueService.UpdateFields(testAssetItemId, field);
        }
        
        private async Task ClearTestField()
        {
            var field = await GetField();
            field.Value = null;
            await UpdateField(field);
        }

        [Test]
        public async Task SetsComboValueIfAlreadyExists()
        {
            await ClearTestField();


            await Service.SetComboValues(testAssetItemId, testMetaFieldId, new List<string>
            {
                "known"
            }, true);


            var field = await GetField();
            Assert.That(field.Value, Is.Not.Null.With.Property(nameof(ComboValue.Label)).EqualTo("known"));
        }

        [Test]
        public async Task SetsComboValueIfNotExists()
        {
            await ClearTestField();

            var guid = Guid.NewGuid().ToString();

            await Service.SetComboValues(testAssetItemId, testMetaFieldId, new List<string> {guid}, true);
            
            var field = await GetField();
            Assert.That(field.Value, Is.Not.Null.With.Property(nameof(ComboValue.Label)).EqualTo(guid));
        }
    }
}