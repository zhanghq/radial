using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Extensions;

namespace Radial.Cache
{
    /// <summary>
    /// The local memory cache implement.
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
            public DateTime? ExpiredAt { get; set; }
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
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void SetBinary(string key, byte [] value, int? cacheSeconds = null)
        {
            key = CacheStatic.NormalizeKey(key);

            lock (SyncRoot)
            {
                var entry = CacheEntries.SingleOrDefault(o => o.Key == key);

                if (entry != null)
                {
                    entry.Value = value;

                    if (!cacheSeconds.HasValue)
                        entry.ExpiredAt = DateTime.Now.AddSeconds(cacheSeconds.Value);
                    else
                        entry.ExpiredAt = null;
                }
                else
                {
                    entry = new LocalCacheEntry { Key = key, Value = value };

                    if (cacheSeconds.HasValue)
                        entry.ExpiredAt = DateTime.Now.AddSeconds(cacheSeconds.Value);

                    CacheEntries.Add(entry);
                }

            }
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached binary value, otherwise return null.
        /// </returns>
        public byte [] GetBinary(string key)
        {
            key = CacheStatic.NormalizeKey(key);

            lock (SyncRoot)
            {
                var entry = CacheEntries.SingleOrDefault(o => o.Key == key);

                if (entry.Value == null || (entry.ExpiredAt.HasValue && DateTime.Now > entry.ExpiredAt))
                    return null;

                return (byte[])entry.Value;
            }
        }



        /// <summary>
        /// Remove cache key and its value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public void Remove(string key)
        {
            key = CacheStatic.NormalizeKey(key);

            lock (SyncRoot)
                CacheEntries.RemoveWhere(o => o.Key == key);
        }


        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void SetString(string key, string value, int? cacheSeconds = null)
        {
            key = CacheStatic.NormalizeKey(key);

            lock (SyncRoot)
            {
                var entry = CacheEntries.SingleOrDefault(o => o.Key == key);

                if (entry != null)
                {
                    entry.Value = value;

                    if (!cacheSeconds.HasValue)
                        entry.ExpiredAt = DateTime.Now.AddSeconds(cacheSeconds.Value);
                    else
                        entry.ExpiredAt = null;
                }
                else
                {
                    entry = new LocalCacheEntry { Key = key, Value = value };

                    if (cacheSeconds.HasValue)
                        entry.ExpiredAt = DateTime.Now.AddSeconds(cacheSeconds.Value);

                    CacheEntries.Add(entry);
                }

            }
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
            key = CacheStatic.NormalizeKey(key);

            lock (SyncRoot)
            {
                var entry = CacheEntries.SingleOrDefault(o => o.Key == key);

                if (entry.Value == null || (entry.ExpiredAt.HasValue && DateTime.Now > entry.ExpiredAt))
                    return null;

                return (string)entry.Value;
            }
        }
    }
}
