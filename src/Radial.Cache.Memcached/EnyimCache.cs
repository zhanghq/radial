using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;

namespace Radial.Cache.Memcached
{
    /// <summary>
    /// Enyim.Caching memcached implement class.
    /// </summary>
    public class EnyimCache : ICache
    {
        MemcachedClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnyimCache"/> class.
        /// </summary>
        public EnyimCache()
        {
            _client = new MemcachedClient();
        }

        /// <summary>
        /// Normalizes the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string NormalizeKey(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "cache key can not be empty or null");

            return key.Trim().ToLower();
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <remarks>The cache will be discarding only if server or explicitly removed.</remarks>
        public void Set(string key, object value)
        {
            _client.Store(Enyim.Caching.Memcached.StoreMode.Set, NormalizeKey(key), value);
        }


        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Set(string key, object value, int cacheSeconds)
        {
            Set(key, value, TimeSpan.FromSeconds(cacheSeconds));
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="ts">The cache holding time.</param>
        public void Set(string key, object value, TimeSpan ts)
        {
            _client.Store(Enyim.Caching.Memcached.StoreMode.Set, NormalizeKey(key), value, ts);
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>If there has matched data, return the cached object, otherwise return null.</returns>
        public object Get(string key)
        {
            return _client.Get(NormalizeKey(key));
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>If there has matched data, return the cached object, otherwise return null.</returns>
        public T Get<T>(string key)
        {
            return _client.Get<T>(NormalizeKey(key));
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
                for (int i = 0; i < keys.Length; i++)
                    keys[i] = NormalizeKey(keys[i]);

                IDictionary<string, object> vals = _client.Get(keys);

                foreach (KeyValuePair<string, object> item in vals)
                    objs.Add(item.Value);

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
                for (int i = 0; i < keys.Length; i++)
                    keys[i] = NormalizeKey(keys[i]);

                IDictionary<string, object> vals = _client.Get(keys);

                foreach (KeyValuePair<string, object> item in vals)
                    objs.Add((T)item.Value);

            }

            return objs.ToArray();
        }

        /// <summary>
        /// Remove cache key and its value.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public void Remove(string key)
        {
            _client.Remove(NormalizeKey(key));
        }

        /// <summary>
        ///  Clear cache.
        /// </summary>
        public void Clear()
        {
            _client.FlushAll();
        }
    }
}
