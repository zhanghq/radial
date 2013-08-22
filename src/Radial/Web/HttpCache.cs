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
        static object SyncRoot = new object();

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached binary, otherwise return null.
        /// </returns>
        public byte[] GetBinary(string key)
        {
            var obj = HttpContext.Current.Cache[CacheStatic.NormalizeKey(key)];

            if (obj == null)
                return null;

            return (byte[])obj;
        }

        /// <summary>
        /// Remove cache key and its value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public void Remove(string key)
        {
            lock (SyncRoot)
                HttpContext.Current.Cache.Remove(CacheStatic.NormalizeKey(key));
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void SetBinary(string key, byte[] value, int? cacheSeconds = null)
        {
            lock (SyncRoot)
            {
                HttpContext.Current.Cache.Insert(CacheStatic.NormalizeKey(key), value, null, cacheSeconds.HasValue ? DateTime.Now.AddSeconds(cacheSeconds.Value) : System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }


        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void SetString(string key, string value, int? cacheSeconds = null)
        {
            lock (SyncRoot)
            {
                HttpContext.Current.Cache.Insert(CacheStatic.NormalizeKey(key), value, null, cacheSeconds.HasValue ? DateTime.Now.AddSeconds(cacheSeconds.Value) : System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached string, otherwise return null.
        /// </returns>
        public string GetString(string key)
        {
            var obj = HttpContext.Current.Cache[CacheStatic.NormalizeKey(key)];

            if (obj == null)
                return null;

            return (string)obj;
        }
    }
}
