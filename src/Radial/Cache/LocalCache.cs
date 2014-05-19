using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Extensions;
using System.Runtime.Caching;

namespace Radial.Cache
{
    /// <summary>
    /// The local memory cache implement.
    /// </summary>
    public sealed class LocalCache : ICache
    {

        private ObjectCache ObjectCache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// Remove cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public void Remove(string key)
        {
            ObjectCache.Remove(CacheStatic.NormalizeKey(key));
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>
        /// If there has matched key, return the cached value, otherwise return null.
        /// </returns>
        public object Get(string key)
        {
            return ObjectCache.Get(CacheStatic.NormalizeKey(key));
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Set(string key, object value, int? cacheSeconds = null)
        {
            if (value == null)
                return;

            key = CacheStatic.NormalizeKey(key);

            if (cacheSeconds.HasValue)
                ObjectCache.Set(key, value, new DateTimeOffset(DateTime.Now.AddSeconds(cacheSeconds.Value)));
            else
                ObjectCache.Set(key, value, null);

        }
    }
}
