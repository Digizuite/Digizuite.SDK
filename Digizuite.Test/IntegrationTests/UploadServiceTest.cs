using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class OldUploadServiceTest : UploadServiceTest
    {
        protected override DigizuiteOption Options => DigizuiteOption.Nothing;
    }

    [Explicit]
    [TestFixture]
    public class NewUploadServiceTest : UploadServiceTest
    {
        protected override DigizuiteOption Options => DigizuiteOption.UseNewUploadService;
    }
    
    public abstract class UploadServiceTest : IntegrationTestBase
    {
        private const string TestFileName = "TestFiles/large_test_image.png";

        [Test]
        public async Task CanUploadFile()
        {
            var service = ServiceProvider.GetRequiredService<IUploadService>();

            var file = new FileInfo(TestFileName);
#pragma warning disable CA2000
            await using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
#pragma warning restore CA2000

            var listener = new SimpleUploadProgressListener();
            var uploadResponse = await service.Upload(stream, "uploaded-from-unit-test.png", "unittest", listener)
                .ConfigureAwait(false);

            Assert.That(uploadResponse.ItemId, Is.EqualTo(listener.UploadInitiatedItemId));
            Assert.That(uploadResponse.ItemId, Is.EqualTo(listener.FinishedItemId));
            Assert.That(listener.ChunkUploadedEvents.Last().Item2, Is.EqualTo(file.Length));
        }

        [Test]
        public async Task CanReplace()
        {
            var service = ServiceProvider.GetRequiredService<IUploadService>();

            var file = new FileInfo(TestFileName);
#pragma warning disable CA2000
            await using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
#pragma warning restore CA2000
            var listener = new SimpleUploadProgressListener();
            var uploadResponse = await service.Replace(stream, "replace-from-unit-test.png", "unittest", 257,
                KeepMetadata.Keep, Overwrite.AddHistoryEntry, listener).ConfigureAwait(false);

            Assert.That(uploadResponse.ItemId, Is.EqualTo(listener.FinishedItemId));
            Assert.That(uploadResponse.AssetId, Is.EqualTo(56));
            Assert.That(listener.ChunkUploadedEvents.Last().Item2, Is.EqualTo(file.Length));
        }

        private class SimpleUploadProgressListener : IUploadProgressListener
        {
            public int UploadInitiatedItemId;
            public List<(int, long)> ChunkUploadedEvents = new();
            public int FinishedItemId;

            public Task UploadInitiated(int itemId, CancellationToken _)
            {
                UploadInitiatedItemId = itemId;
                return Task.CompletedTask;
            }

            public Task ChunkUploaded(int itemId, long totalUploaded, CancellationToken _)
            {
                ChunkUploadedEvents.Add((itemId, totalUploaded));
                return Task.CompletedTask;
            }

            public Task FinishedUpload(int itemId, CancellationToken _)
            {
                FinishedItemId = itemId;
                return Task.CompletedTask;
            }
        }
    }
}