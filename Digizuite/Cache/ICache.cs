using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Digizuite.Cache
{
    /// <summary>
    /// Caching of a specific type of items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICache<T>
    {
        /// <summary>
        /// Gets or calculated the given entry in the cache
        /// </summary>
        /// <param name="key">A unique key for that value of that type. Prefixing for different types is handled
        /// internally, so no need to do that</param>
        /// <param name="duration">How long the entry should be stored in the cache</param>
        /// <param name="loadPredicate">A predicate to actually load the data</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "CA1716")]
        Task<T> Get(string key, TimeSpan duration, Func<Task<T>> loadPredicate);

        /// <summary>
        /// Clears the given entry from the cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        void Clear(string key);
    }
}