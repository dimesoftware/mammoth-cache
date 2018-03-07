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
            this.Cache = cache;
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
            object value = default(object);
            this.Cache.TryGetValue(key, out value);

            if (value is T)
                return (T)value;
            else
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
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
            c.ICacheEntry entry = this.Cache.CreateEntry(key);
            entry.Value = value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            this.Cache.Remove(key);
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            this.Cache.Dispose();
        }

        #endregion Methods
    }
}