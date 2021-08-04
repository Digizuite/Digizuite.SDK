using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Digizuite.HttpAbstraction
{
    public class LimitedReaderStream : Stream
    {
        private readonly Stream _underlyingStream;
        private long _remainingToRead;
        public long TotalRead { get; private set; }

        public LimitedReaderStream(Stream underlyingStream, long maxToRead)
        {
            _underlyingStream = underlyingStream;
            _remainingToRead = maxToRead;
        }

        public override void Flush()
        {
            _underlyingStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count > _remainingToRead)
            {
                count = (int) _remainingToRead;
            }

            Console.WriteLine($"Reading {count} bytes from stream");
            
            var amountRead = _underlyingStream.Read(buffer, offset, count);
            _remainingToRead -= amountRead;
            TotalRead += amountRead;
            Console.WriteLine($"Actually read {amountRead}");
            return amountRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _underlyingStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _underlyingStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _underlyingStream.Write(buffer, offset, count);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (count > _remainingToRead)
            {
                count = (int)_remainingToRead;
            }

            Console.WriteLine($"Reading {count} bytes from stream (async)");
            
            var amountRead = await _underlyingStream.ReadAsync(buffer, offset, count, cancellationToken);
            _remainingToRead -= amountRead;
            TotalRead += amountRead;
            Console.WriteLine($"Actually read {amountRead} (async)");
            return amountRead;
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            await _underlyingStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            await _underlyingStream.FlushAsync(cancellationToken);
        }

        public override bool CanRead => _underlyingStream.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => _underlyingStream.Position;
            set => _underlyingStream.Position = value;
        }
    }
}