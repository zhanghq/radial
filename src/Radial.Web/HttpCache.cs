using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Radial.Cache;

namespace Radial.Web
{
    /// <summary>
    /// Http cache class.
    /// </summary>
    public sealed class HttpCache : ICache
    {
        string _cachePrefix;
        TimeSpan _defaultExpiration;

        static object SyncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpCache"/> class.
        /// </summary>
        public HttpCache()
            : this(1200)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpCache"/> class.
        /// </summary>
        /// <param name="expirationSeconds">The default expiration seconds.</param>
        public HttpCache(int expirationSeconds)
            : this("httpcache", expirationSeconds)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpCache"/> class.
        /// </summary>
        /// <param name="cachePrefix">The cache prefix.</param>
        /// <param name="expirationSeconds">The default expiration seconds.</param>
        public HttpCache(string cachePrefix, int expirationSeconds)
        {
            _cachePrefix = string.IsNullOrWhiteSpace(cachePrefix) ? string.Empty : cachePrefix.Trim();
            _defaultExpiration = TimeSpan.FromSeconds((double)expirationSeconds);
        }

        /// <summary>
        /// Normalizes the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string NormalizeKey(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "cache key can not be empty or null");

            return string.Format("{0}_{1}", _cachePrefix, key.Trim().ToLower());
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        public void Set(string key, object value)
        {
            Set(key, value, _defaultExpiration);
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        public void Set<T>(string key, T value)
        {
            Set<T>(key, value, _defaultExpiration);
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="ts">The cache holding time.</param>
        public void Set(string key, object value, TimeSpan ts)
        {
            Checker.Parameter(value != null, "cache value can not be null");
            lock (SyncRoot)
                HttpContext.Current.Cache.Insert(NormalizeKey(key), value, null, DateTime.UtcNow.Add(ts), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="ts">The cache holding time.</param>
        public void Set<T>(string key, T value, TimeSpan ts)
        {
            Checker.Parameter(value != null, "cache value can not be null");

            lock (SyncRoot)
                HttpContext.Current.Cache.Insert(NormalizeKey(key), value, null, DateTime.UtcNow.Add(ts), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>
        /// If there has matched data, return the cached object, otherwise return null.
        /// </returns>
        public object Get(string key)
        {
            return HttpContext.Current.Cache[NormalizeKey(key)];
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>
        /// If there has matched data, return the cached object, otherwise return null.
        /// </returns>
        public T Get<T>(string key)
        {
            return (T)Get(key);
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="keys">The cache keys.</param>
        /// <returns>If there has matched data, return the cached objects, otherwise return an empty array.</returns>
        public object[] Gets(string[] keys)
        {
            List<object> objs = new List<object>();

            if (keys != null)
            {
                foreach (string key in keys)
                {
                    objs.Add(Get(key));
                }
            }

            return objs.ToArray();
        }
        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="keys">The cache keys.</param>
        /// <returns>If there has matched data, return the cached objects, otherwise return an empty array.</returns>
        public T[] Gets<T>(string[] keys)
        {
            List<T> objs = new List<T>();

            if (keys != null)
            {
                foreach (string key in keys)
                {
                    objs.Add(Get<T>(key));
                }
            }

            return objs.ToArray();
        }

        /// <summary>
        /// Remove cache key and its value.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public void Remove(string key)
        {
            lock (SyncRoot)
                HttpContext.Current.Cache.Remove(NormalizeKey(key));
        }

        /// <summary>
        /// Clear cache.
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        /// <remarks>The cache will be discarding only if memory lack or explicitly removed.</remarks>
        public void Set(string key, object value, int cacheSeconds)
        {
            Set(key, value, TimeSpan.FromSeconds((double)cacheSeconds));
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        /// <remarks>The cache will be discarding only if memory lack or explicitly removed.</remarks>
        public void Set<T>(string key, T value, int cacheSeconds)
        {
            Set<T>(key, value, TimeSpan.FromSeconds((double)cacheSeconds));
        }
    }
}
