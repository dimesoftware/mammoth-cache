using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MammothCache
{
    /// <summary>
    /// Contracts of the capabilities of a read-only cache
    /// </summary>
    public interface IWriteCache
    {
        /// <summary>
        /// Sets the value in the cache
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The unique key to identify the cache entry</param>
        /// <param name="value">The value</param>
        void Set<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// Sets the value in the cache
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The unique key to identify the cache entry</param>
        /// <param name="value">The value</param>

        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// Removes the value from the cache
        /// </summary>
        /// <param name="key">The identifier of the cache entry</param>
        /// <param name="exactMatch">A boolean to indicate an exact match or a wildcard</param>
        void Remove(string key, bool exactMatch = true);

        /// <summary>
        /// Removes the value from the cache
        /// </summary>
        /// <param name="key">The identifier of the cache entry</param>
        /// <param name="exactMatch">A boolean to indicate an exact match or a wildcard</param>
        Task RemoveAsync(string key, bool exactMatch = true);

        /// <summary>
        /// Removes items from the cache in batch
        /// </summary>
        /// <param name="keys">The keys to remove</param>
        Task RemoveAsync(IEnumerable<string> keys);
    }
}