using CacheManager.Core;
using Newtonsoft.Json;
using System;

namespace Dime.Caching.Web
{
    /// <summary>
    /// In Memory Cache for ASP.NET applications. This class is suitable for single-server setups.
    /// </summary>
    public class InMemoryCache : ICache
    {
        #region Constructor

        public InMemoryCache()
        {
            try
            {
                this.CacheManager = CacheFactory.Build<string>(settings => settings
                .WithUpdateMode(CacheUpdateMode.Up)
                .WithSystemWebCacheHandle("handleName")
                .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(15))
                );
            }
            catch (Exception)
            {
                // Swallow exception. If cache manager is null, we know why
            }
        }

        #endregion Constructor

        #region Properties

        private ICacheManager<string> CacheManager { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            try
            {
                string cachedItem = this.CacheManager?.Get(key);
                if (!string.IsNullOrEmpty(cachedItem))
                {
                    return JsonConvert.DeserializeObject<T>(cachedItem);
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string key, T value)
        {
            this.CacheManager?.Put(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            this.CacheManager?.Remove(key);
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