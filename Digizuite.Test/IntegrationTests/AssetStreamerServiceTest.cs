using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class AssetStreamerServiceTest : IntegrationTestBase
    {
        [Test]
        [Explicit]
        public async Task GeneratesValidDownloadUrl()
        {
            var streamer = ServiceProvider.GetRequiredService<IAssetStreamerService>();
            var url = await streamer.GetAssetDownloadUrl(91);

            await TestDownload(url);
        }

        [Test]
        [Explicit]
        public async Task ShouldDownloadSpecificQuality()
        {
            var streamer = ServiceProvider.GetRequiredService<IAssetStreamerService>();
            var url = await streamer.GetAssetDownloadUrl(91, null, 50034, 10010);

            await TestDownload(url);
        }

        private async Task TestDownload(Uri url)
        {
            Console.WriteLine(url);
            var tempFileName = Path.GetTempFileName();

            try
            {
                using var wc = new WebClient();
                await wc.DownloadFileTaskAsync(url, tempFileName);

                var fileInfo = new FileInfo(tempFileName);

                Assert.That(fileInfo.Length, Is.GreaterThan(0));
            }
            finally
            {
                File.Delete(tempFileName);
            }
        }
    }
}