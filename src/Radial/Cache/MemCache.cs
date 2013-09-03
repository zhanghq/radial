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
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void SetBinary(string key, byte[] value, int? cacheSeconds = null)
        {
            if (cacheSeconds.HasValue)
                Client.Store(Enyim.Caching.Memcached.StoreMode.Set, CacheStatic.NormalizeKey(key), value, TimeSpan.FromSeconds(cacheSeconds.Value));
            else
                Client.Store(Enyim.Caching.Memcached.StoreMode.Set, CacheStatic.NormalizeKey(key), value);
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached binary value, otherwise return null.
        /// </returns>
        public byte[] GetBinary(string key)
        {
            var obj = Client.Get(CacheStatic.NormalizeKey(key));

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
            Client.Remove(CacheStatic.NormalizeKey(key));
        }


        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void SetString(string key, string value, int? cacheSeconds = null)
        {
            if (cacheSeconds.HasValue)
                Client.Store(Enyim.Caching.Memcached.StoreMode.Set, CacheStatic.NormalizeKey(key), value, TimeSpan.FromSeconds(cacheSeconds.Value));
            else
                Client.Store(Enyim.Caching.Memcached.StoreMode.Set, CacheStatic.NormalizeKey(key), value);
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached string value, otherwise return null.
        /// </returns>
        public string GetString(string key)
        {
            var obj = Client.Get(CacheStatic.NormalizeKey(key));

            if (obj == null)
                return null;

            return (string)obj;
        }
    }
}
