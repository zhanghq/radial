using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Radial.Cache;
using System.Configuration;

namespace Radial.Web
{
    /// <summary>
    /// Http cache class.
    /// </summary>
    public sealed class HttpCache : ICache
    {
        /// <summary>
        /// Remove cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(CacheStatic.NormalizeKey(key));
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched key, return the cached data, otherwise return null.
        /// </returns>
        public object Get(string key)
        {
            return HttpContext.Current.Cache[CacheStatic.NormalizeKey(key)];
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Set(string key, object value, int? cacheSeconds = null)
        {
            if (value == null)
            {
                Remove(key);
                return;
            }

            HttpContext.Current.Cache.Insert(CacheStatic.NormalizeKey(key), value, null, cacheSeconds.HasValue ? DateTime.Now.AddSeconds(cacheSeconds.Value) : System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
        }
    }
}
