using System;
using c = Microsoft.Extensions.Caching.Memory;

namespace Dime.Caching.Web
{
    /// <summary>
    ///
    /// </summary>
    public class InMemoryCache : ICache
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="cache"></param>
        public InMemoryCache(c.IMemoryCache cache)
        {
            Cache = cache;
        }

        #endregion Constructor

        #region Properties

        private c.IMemoryCache Cache { get; set; }

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
            Cache.TryGetValue(key, out var value);

            if (value is T value1)
                return value1;
            
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException)
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
            c.ICacheEntry entry = Cache.CreateEntry(key);
            entry.Value = value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key) 
            => Cache.Remove(key);

        /// <summary>
        ///
        /// </summary>
        public void Dispose() 
            => Cache.Dispose();

        #endregion Methods
    }
}