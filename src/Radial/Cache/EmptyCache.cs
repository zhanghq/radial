using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Cache
{
    /// <summary>
    /// The default ICache concret class with no cache implement
    /// </summary>
    public sealed class EmptyCache : ICache
    {

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        public void Set(string key, object value)
        {

        }


        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        public void Set<T>(string key, T value)
        {

        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="ts">The cache holding time.</param>
        public void Set(string key, object value, TimeSpan ts)
        {

        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="ts">The cache holding time.</param>
        public void Set<T>(string key, T value, TimeSpan ts)
        {

        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached object, otherwise return null.
        /// </returns>
        public object Get(string key)
        {
            return null;
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached object, otherwise return null.
        /// </returns>
        public T Get<T>(string key)
        {
            return default(T);
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="keys">The cache keys(case insensitive).</param>
        /// <returns>If there has matched data, return the cached objects, otherwise return an empty array.</returns>
        public object[] Gets(string[] keys)
        {
            return new object[] { };
        }
        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="keys">The cache keys(case insensitive).</param>
        /// <returns>If there has matched data, return the cached objects, otherwise return an empty array.</returns>
        public T[] Gets<T>(string[] keys)
        {
            return new T[] { };
        }


        /// <summary>
        /// Remove cache key and its value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public void Remove(string key)
        {
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
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Set(string key, object value, int cacheSeconds) { }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <typeparam name="T">The type of cache value.</typeparam>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Set<T>(string key, T value, int cacheSeconds) { }

    }
}
