using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.BatchUpdate;
using Digizuite.BatchUpdate.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class BatchUpdateClientTest : IntegrationTestBase
    {
        [Test]
        public async Task CanSendUpdate()
        {
            var update = new Batch(new BatchPart
            {
                ItemId = 10192,
                Target = FieldType.Asset,
                BatchType = BatchType.ItemIdsValuesRowId,
                Values = new List<BatchValue>
                {
                    new StringBatchValue(FieldType.Metafield, "Title set from unit tests, please don't touch", new BatchLabelIdProperties(50723))
                }
            });

            var client = ServiceProvider.GetRequiredService<IBatchUpdateClient>();
            await client.ApplyBatch(update);
        }
    }
}