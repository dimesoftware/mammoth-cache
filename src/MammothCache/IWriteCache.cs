using System;

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
        /// Removes the value from the cache
        /// </summary>
        /// <param name="key">The identifier of the cache entry</param>
        /// <param name="exactMatch">A boolean to indicate an exact match or a wildcared</param>
        void Remove(string key, bool exactMatch);
    }
}