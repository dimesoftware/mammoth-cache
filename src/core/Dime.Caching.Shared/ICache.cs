using System;

namespace Dime.Caching
{
    /// <summary>
    /// Contracts of the capabilities of the cache
    /// </summary>
    public interface ICache : IReadCache, IDisposable
    {
        /// <summary>
        /// Sets the value in the cache
        /// </summary>
        /// <typeparam name="T">The type of the valule</typeparam>
        /// <param name="key">The unique key to identify the cache entry</param>
        /// <param name="value">The value</param>
        void Set<T>(string key, T value);

        /// <summary>
        /// Removes the value from the cache
        /// </summary>
        /// <param name="key">The identifier of the cache entry</param>
        void Remove(string key);
    }
}