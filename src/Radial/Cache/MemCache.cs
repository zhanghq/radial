using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using System.Threading;

namespace Radial.Cache
{
    /// <summary>
    /// Memcached cache implement class.
    /// </summary>
    public class MemCache : ICache
    {
        static object SyncRoot = new object();
        static MemcachedClient Client=null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemCache"/> class.
        /// </summary>
        public MemCache()
        {
            lock (SyncRoot)
            {
                if (Client == null)
                    Client = new MemcachedClient();
            }
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched key, return the cached value, otherwise return null.
        /// </returns>
        public byte[] Get(string key)
        {
            return Client.Get<byte[]>(CacheStatic.NormalizeKey(key));
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Set(string key, byte[] value, int? cacheSeconds = null)
        {
            if (value == null || value.Length == 0)
            {
                Remove(key);
                return;
            }

            if (cacheSeconds.HasValue)
                Client.Store(Enyim.Caching.Memcached.StoreMode.Set, CacheStatic.NormalizeKey(key), value, TimeSpan.FromSeconds(cacheSeconds.Value));
            else
                Client.Store(Enyim.Caching.Memcached.StoreMode.Set, CacheStatic.NormalizeKey(key), value);
        }


        /// <summary>
        /// Remove cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public void Remove(string key)
        {
            Client.Remove(CacheStatic.NormalizeKey(key));
        }
    }
}
