using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace Digizuite.Cache
{
    /// <summary>
    /// Is a cache implementation, that relies on the standard IMemoryCache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InMemoryCache<T> : ICache<T>, IDisposable
    {
        private IMemoryCache _internalCache;
        private ILogger<InMemoryCache<T>> _logger;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private ConcurrentDictionary<string, Task<T>> loadLocks = new ConcurrentDictionary<string, Task<T>>();

        public InMemoryCache(IMemoryCache internalCache, ILogger<InMemoryCache<T>> logger)
        {
            _internalCache = internalCache;
            _logger = logger;
        }

        public async Task<T> Get(string key, TimeSpan duration, Func<Task<T>> loadPredicate)
        {
            if (_internalCache.TryGetValue<T>(key, out var result))
            {
                return result;
            }

            Task<T> cachedTask;
            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (loadLocks.TryGetValue(key, out var task))
                {
                    _logger.LogDebug("Entry task was already in cache", nameof(key), key);
                    cachedTask = task;
                }
                else
                {
                    _logger.LogDebug("Entry task was not in cache", nameof(key), key);
                    cachedTask = loadPredicate().AndThen(data =>
                    {
                        _logger.LogDebug("Loaded new data for cache", nameof(key), key);
                        _internalCache.Set(key, data, duration);
                        // Remove this pending task from the load list, so we don't leak memory
                        loadLocks.TryRemove(key, out _);
                        return Task.CompletedTask;
                    });
                    loadLocks.TryAdd(key, cachedTask);
                }
            }
            finally
            {
                _semaphore.Release();
            }

            return await cachedTask.ConfigureAwait(false);
        }

        public void Clear(string key)
        {
            _internalCache.Remove(key);
            loadLocks.TryRemove(key, out _);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    
                    _internalCache?.Dispose();
                    _semaphore?.Dispose();
                }

                _disposed = true;
            }
        }
    }
}