using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Radial.Cache
{
    /// <summary>
    /// The entrance static class of ICache instance.
    /// </summary>
    public static class CacheStatic
    {
        /// <summary>
        /// Gets the ICache instance.
        /// </summary>
        private static ICache Instance
        {
            get
            {
                if (!Components.Container.IsRegistered<ICache>())
                    return new EmptyCache();
                return Components.Container.Resolve<ICache>();
            }
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        public static void Set(string key, object value)
        {
            Instance.Set(key, value);
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public static void Set(string key, object value, int cacheSeconds)
        {
            Instance.Set(key, value, cacheSeconds);
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="ts">The cache holding time.</param>
        public static void Set(string key, object value, TimeSpan ts)
        {
            Instance.Set(key, value, ts);
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>If there has matched data, return the cached object, otherwise return null.</returns>
        public static object Get(string key)
        {
            return Instance.Get(key);
        }
        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>If there has matched data, return the cached object, otherwise return null.</returns>
        public static T Get<T>(string key)
        {
            return Instance.Get<T>(key);
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="keys">The cache keys(case insensitive).</param>
        /// <returns>If there has matched data, return the cached objects, otherwise return an empty array.</returns>
        public static object[] Gets(string[] keys)
        {
            return Instance.Gets(keys);
        }
        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="keys">The cache keys(case insensitive).</param>
        /// <returns>If there has matched data, return the cached objects, otherwise return an empty array.</returns>
        public static T[] Gets<T>(string[] keys)
        {
            return Instance.Gets<T>(keys);
        }

        /// <summary>
        /// Remove cache key and its value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public static void Remove(string key)
        {
            Instance.Remove(key);
        }
        /// <summary>
        ///  Clear cache.
        /// </summary>
        public static void Clear()
        {
            Instance.Clear();
        }
    }
}
