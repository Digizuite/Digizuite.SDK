using System.IO;
using System.Text;
using System.Threading.Tasks;
using Digizuite.HttpAbstraction;
using NUnit.Framework;

namespace Digizuite.Test.UnitTests
{
    [TestFixture]
    public class LimitedReaderStreamTests
    {
        [Test]
        public async Task OnlyReadsExactBytes_Async()
        {
            await using var fullStream = new MemoryStream(Encoding.UTF8.GetBytes("HelloWorld"));

            await using (var limit1 = new LimitedReaderStream(fullStream, 6))
            {
                await using var final1 = new MemoryStream(20);
                await limit1.CopyToAsync(final1);

                var content = Encoding.UTF8.GetString(final1.ToArray());
                Assert.That(content, Is.EqualTo("HelloW"));
            }

            await using (var limit2 = new LimitedReaderStream(fullStream, 6))
            {
                await using var final2 = new MemoryStream(20);
                await limit2.CopyToAsync(final2);

                var content = Encoding.UTF8.GetString(final2.ToArray());
                Assert.That(content, Is.EqualTo("orld"));
            }
        }

        [Test]
        public async Task ReadsCorrectBytesAcrossMultipleReads_Async()
        {
            await using var fullStream = new MemoryStream(Encoding.UTF8.GetBytes("HelloWorld"));

            await using (var limit1 = new LimitedReaderStream(fullStream, 6))
            {
                await using var final1 = new MemoryStream(20);
                var buf = new byte[4];
                await limit1.ReadAsync(buf, 0, buf.Length);

                var content = Encoding.UTF8.GetString(buf);
                Assert.That(content, Is.EqualTo("Hell"));

                buf = new byte[4];
                await limit1.ReadAsync(buf, 0, buf.Length);
                content = Encoding.UTF8.GetString(buf);
                Assert.That(content, Is.EqualTo("oW\0\0"));
            }

            await using (var limit2 = new LimitedReaderStream(fullStream, 6))
            {
                await using var final2 = new MemoryStream(20);
                await limit2.CopyToAsync(final2);

                var content = Encoding.UTF8.GetString(final2.ToArray());
                Assert.That(content, Is.EqualTo("orld"));
            }
        }
        
        [Test]
        public void OnlyReadsExactBytes_Sync()
        {
            using var fullStream = new MemoryStream(Encoding.UTF8.GetBytes("HelloWorld"));

            using (var limit1 = new LimitedReaderStream(fullStream, 6))
            {
                using var final1 = new MemoryStream(20);
                limit1.CopyTo(final1);

                var content = Encoding.UTF8.GetString(final1.ToArray());
                Assert.That(content, Is.EqualTo("HelloW"));
            }

            using (var limit2 = new LimitedReaderStream(fullStream, 6))
            {
                using var final2 = new MemoryStream(20);
                limit2.CopyTo(final2);

                var content = Encoding.UTF8.GetString(final2.ToArray());
                Assert.That(content, Is.EqualTo("orld"));
            }
        }

        [Test]
        public void ReadsCorrectBytesAcrossMultipleReads_Sync()
        {
            using var fullStream = new MemoryStream(Encoding.UTF8.GetBytes("HelloWorld"));

            using (var limit1 = new LimitedReaderStream(fullStream, 6))
            {
                using var final1 = new MemoryStream(20);
                var buf = new byte[4];
                limit1.Read(buf, 0, buf.Length);

                var content = Encoding.UTF8.GetString(buf);
                Assert.That(content, Is.EqualTo("Hell"));

                buf = new byte[4];
                limit1.Read(buf, 0, buf.Length);
                content = Encoding.UTF8.GetString(buf);
                Assert.That(content, Is.EqualTo("oW\0\0"));
            }

            using (var limit2 = new LimitedReaderStream(fullStream, 6))
            {
                using var final2 = new MemoryStream(20);
                limit2.CopyTo(final2);

                var content = Encoding.UTF8.GetString(final2.ToArray());
                Assert.That(content, Is.EqualTo("orld"));
            }
        }
    }
}