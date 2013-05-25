using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Extensions;

namespace Radial.Cache
{
    /// <summary>
    /// The local memory ICache implement.
    /// </summary>
    public sealed class LocalCache : ICache
    {
        /// <summary>
        /// LocalCacheEntry
        /// </summary>
        class LocalCacheEntry
        {
            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            /// <value>
            /// The key.
            /// </value>
            public string Key { get; set; }
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public object Value { get; set; }

            /// <summary>
            /// Gets or sets the expired at.
            /// </summary>
            /// <value>
            /// The expired at.
            /// </value>
            public DateTime ExpiredAt { get; set; }
        }

        static HashSet<LocalCacheEntry> CacheEntries;
        static object SyncRoot = new object();

        /// <summary>
        /// Initializes the <see cref="LocalCache"/> class.
        /// </summary>
        public LocalCache()
        {
            lock (SyncRoot)
            {
                if (CacheEntries == null)
                    CacheEntries = new HashSet<LocalCacheEntry>();
            }
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Set(string key, object value)
        {
            key = NormalizeKey(key);

            lock (SyncRoot)
            {
                if (CacheEntries.Contains(o => o.Key == key))
                {
                    var entry = CacheEntries.SingleOrDefault(o => o.Key == key);
                    entry.Value = Toolkits.DeepClone(value);
                    entry.ExpiredAt = DateTime.MaxValue;
                }
                else
                    CacheEntries.Add(new LocalCacheEntry { Key = key, Value = Toolkits.DeepClone(value), ExpiredAt = DateTime.MaxValue });
            }
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Set(string key, object value, int cacheSeconds)
        {
            key = NormalizeKey(key);

            lock (SyncRoot)
            {
                if (CacheEntries.Contains(o => o.Key == key))
                {
                    var entry = CacheEntries.SingleOrDefault(o => o.Key == key);
                    entry.Value = Toolkits.DeepClone(value);
                    entry.ExpiredAt = DateTime.Now.AddSeconds(cacheSeconds);
                }
                else
                    CacheEntries.Add(new LocalCacheEntry { Key = key, Value = Toolkits.DeepClone(value), ExpiredAt = DateTime.Now.AddSeconds(cacheSeconds) });
            }
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="ts">The cache holding time.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Set(string key, object value, TimeSpan ts)
        {
            key = NormalizeKey(key);

            lock (SyncRoot)
            {
                if (CacheEntries.Contains(o => o.Key == key))
                {
                    var entry = CacheEntries.SingleOrDefault(o => o.Key == key);
                    entry.Value = Toolkits.DeepClone(value);
                    entry.ExpiredAt = DateTime.Now.Add(ts);
                }
                else
                    CacheEntries.Add(new LocalCacheEntry { Key = key, Value = Toolkits.DeepClone(value), ExpiredAt = DateTime.Now.Add(ts) });
            }
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached object, otherwise return null.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Get(string key)
        {
            key = NormalizeKey(key);

            lock (SyncRoot)
            {
                var entry = CacheEntries.SingleOrDefault(o => o.Key == key);

                if (DateTime.Now > entry.ExpiredAt)
                    return null;

                return Toolkits.DeepClone(entry.Value);
            }
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached object, otherwise return null.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public T Get<T>(string key)
        {
            return (T)Get(key);
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="keys">The cache keys(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached objects, otherwise return an empty array.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object[] Gets(string[] keys)
        {
            keys = NormalizeKeys(keys);

            IList<object> objs = new List<object>();

            lock (SyncRoot)
            {
                foreach (string k in keys)
                {
                    if (CacheEntries.Contains(o => o.Key == k & o.ExpiredAt >= DateTime.Now))
                        objs.Add(CacheEntries.Single(o => o.Key == k & o.ExpiredAt >= DateTime.Now).Value);
                }
            }

            return objs.ToArray();
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="keys">The cache keys(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached objects, otherwise return an empty array.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public T[] Gets<T>(string[] keys)
        {
            keys = NormalizeKeys(keys);

            IList<T> objs = new List<T>();

            lock (SyncRoot)
            {
                foreach (string k in keys)
                {
                    if (CacheEntries.Contains(o => o.Key == k & o.ExpiredAt <= DateTime.Now))
                        objs.Add((T)CacheEntries.Single(o => o.Key == k & o.ExpiredAt <= DateTime.Now).Value);
                }
            }

            return objs.ToArray();
        }

        /// <summary>
        /// Remove cache key and its value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public void Remove(string key)
        {
            key = NormalizeKey(key);

            lock (SyncRoot)
                CacheEntries.RemoveWhere(o => o.Key == key);
        }

        /// <summary>
        /// Clear cache.
        /// </summary>
        public void Clear()
        {
            lock (SyncRoot)
                CacheEntries.Clear();
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
        /// Normalizes the keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        private string[] NormalizeKeys(string[] keys)
        {
            Checker.Parameter(keys != null && keys.Length > 0, "cache keys can not be empty or null");

            IList<string> list = new List<string>();

            foreach (string k in keys)
                list.Add(k);

            return list.ToArray();
        }
    }
}
