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

        /// <summary>
        /// Gets the object cache.
        /// </summary>
        private ObjectCache ObjectCache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// Drop cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public void Drop(string key)
        {
            ObjectCache.Remove(CacheStatic.NormalizeKey(key));
        }


        /// <summary>
        /// Get cache data.
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
        /// Put cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Put(string key, object value, int? cacheSeconds = null)
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
