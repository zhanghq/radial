using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace Radial.Cache
{
    /// <summary>
    /// The local memory cache implement.
    /// </summary>
    public sealed class LocalCache : ICache
    {
        static object SyncRoot = new object();
        static IDictionary<string, string> CacheKeyRegions;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalCache"/> class.
        /// </summary>
        public LocalCache()
        {
            lock (SyncRoot)
            {
                if (CacheKeyRegions == null)
                    CacheKeyRegions = new Dictionary<string, string>();
            }
        }

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
            key = CacheStatic.NormalizeKey(key);

            ObjectCache.Remove(CacheStatic.NormalizeKey(key));

            lock (SyncRoot)
                CacheKeyRegions.Remove(key);
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
            Put(key, null, value, cacheSeconds);
        }

        /// <summary>
        /// Put cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="region">The cache region.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Put(string key, string region, object value, int? cacheSeconds = null)
        {
            if (value == null)
                return;

            key = CacheStatic.NormalizeKey(key);

            if (cacheSeconds.HasValue)
                ObjectCache.Set(key, value, new DateTimeOffset(DateTime.Now.AddSeconds(cacheSeconds.Value)));
            else
                ObjectCache.Set(key, value, null);

            lock (SyncRoot)
            {
                if (!string.IsNullOrWhiteSpace(region))
                    CacheKeyRegions.Add(key, region.Trim());
            }
        }

        /// <summary>
        /// Drop cache region.
        /// </summary>
        /// <param name="region">The cache region.</param>
        public void DropRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region))
                return;

            lock (SyncRoot)
            {
                var kvs = CacheKeyRegions.Where(o => string.Compare(o.Value, region.Trim(), true) == 0);
                foreach (var kv in kvs)
                    Drop(kv.Key);
            }
        }


        /// <summary>
        /// Gets the cache regions.
        /// </summary>
        /// <returns>
        /// The cache regions.
        /// </returns>
        public string[] GetRegions()
        {
            return CacheKeyRegions.Values.Distinct().ToArray();
        }
    }
}
