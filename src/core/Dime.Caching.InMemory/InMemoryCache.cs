using System;
using CacheManager.Core;
using Newtonsoft.Json;

namespace Dime.Caching.Web
{
    /// <summary>
    /// In-memory cache for ASP.NET applications using the CacheManager framework.
    /// This class is suitable for single-server setups.
    /// </summary>
    public class InMemoryCache : ICache
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCache"/> class
        /// </summary>
        public InMemoryCache()
        {
        }

        #endregion Constructor

        #region Properties

        private static ICacheManager<string> _cacheManager;

        /// <summary>
        ///
        /// </summary>
        public ICacheManager<string> CacheManager
        {
            get
            {
                if (_cacheManager == null)
                    _cacheManager = CreateCacheManager();

                return _cacheManager;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private ICacheManager<string> CreateCacheManager()
        {
            return CacheFactory.Build<string>(settings => settings
                .WithUpdateMode(CacheUpdateMode.Up)
                .WithSystemWebCacheHandle("handleName")
                .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(15))
                );
        }

        /// <summary>
        /// Gets the value from the cache
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The unique key to identify the cache entry</param>
        /// <returns>The value</returns>
        public T Get<T>(string key)
        {
            try
            {
                string cachedItem = CacheManager?.Get(key);
                return !string.IsNullOrEmpty(cachedItem) ? JsonConvert.DeserializeObject<T>(cachedItem) : default;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// Sets the value in the cache
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="key">The unique key to identify the cache entry</param>
        /// <param name="value">The value</param>
        public void Set<T>(string key, T value) 
            => CacheManager?.Put(key, JsonConvert.SerializeObject(value));

        /// <summary>
        /// Removes the value from the cache
        /// </summary>
        /// <param name="key">The identifier of the cache entry</param>
        public void Remove(string key)
        {
            try
            {
                CacheManager?.Remove(key);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
        }

        #endregion Methods
    }
}