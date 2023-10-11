using System.Threading.Tasks;

namespace MammothCache
{
    /// <summary>
    /// Contracts of the capabilities of a read-only cache
    /// </summary>
    public interface IReadCache
    {
        /// <summary>
        /// Gets the value from the cache
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The unique key to identify the cache entry</param>
        /// <returns>The value</returns>
        T Get<T>(string key);

        /// <summary>
        /// Gets the value from the cache
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The unique key to identify the cache entry</param>
        /// <returns>The value</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Checks if the cache contains a key
        /// </summary>
        /// <param name="key">The unique key to identify the cache entry</param>
        /// <returns>True if the value exists. Otherwise, false.</returns>
        bool Contains(string key);

        /// <summary>
        /// Checks if the cache contains a key
        /// </summary>
        /// <param name="key">The unique key to identify the cache entry</param>
        /// <returns>True if the value exists. Otherwise, false.</returns>
        Task<bool> ContainsAsync(string key);
    }
}