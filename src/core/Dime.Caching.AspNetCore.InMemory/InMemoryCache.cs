using CacheManager.Core;
using Newtonsoft.Json;
using System;

namespace Dime.Caching.Web
{
    /// <summary>
    /// In Memory Cache for ASP.NET applications. This class is suitable for single-server setups.
    /// </summary>
    public class InMemoryCache : ICache<T>
    {
        #region Constructor

        public InMemoryCache()
        {
            try
            {
                this.CacheManager = CacheFactory.Build<string>(settings => settings
                .WithUpdateMode(CacheUpdateMode.Up)
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

        public T this[string key, string region] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public T this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        public bool Add(string key, T value)
        {
            throw new NotImplementedException();
        }

        public bool Add(string key, T value, string region)
        {
            throw new NotImplementedException();
        }

        public bool Add(CacheItem<T> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void ClearRegion(string region)
        {
            throw new NotImplementedException();
        }

        public void Expire(string key, ExpirationMode mode, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void Expire(string key, string region, ExpirationMode mode, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void Expire(string key, DateTimeOffset absoluteExpiration)
        {
            throw new NotImplementedException();
        }

        public void Expire(string key, string region, DateTimeOffset absoluteExpiration)
        {
            throw new NotImplementedException();
        }

        public void Expire(string key, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }

        public void Expire(string key, string region, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }

        public T Get(string key)
        {
            throw new NotImplementedException();
        }

        public T Get(string key, string region)
        {
            throw new NotImplementedException();
        }

        public TOut Get<TOut>(string key, string region)
        {
            throw new NotImplementedException();
        }

        public CacheItem<T> GetCacheItem(string key)
        {
            throw new NotImplementedException();
        }

        public CacheItem<T> GetCacheItem(string key, string region)
        {
            throw new NotImplementedException();
        }

        public void Put(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void Put(string key, T value, string region)
        {
            throw new NotImplementedException();
        }

        public void Put(CacheItem<T> item)
        {
            throw new NotImplementedException();
        }

        bool ICache<T>.Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key, string region)
        {
            throw new NotImplementedException();
        }

        public void RemoveExpiration(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveExpiration(string key, string region)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}