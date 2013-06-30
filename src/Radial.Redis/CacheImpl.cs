using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Cache;

namespace Radial.Redis
{
    /// <summary>
    /// Redis cache implementation.
    /// </summary>
    public class CacheImpl : ICache
    {
        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        public void Set(string key, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Set(string key, object value, int cacheSeconds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="ts">The cache holding time.</param>
        public void Set(string key, object value, TimeSpan ts)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="keys">The cache keys(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached objects, otherwise return an empty array.
        /// </returns>
        public object[] Gets(string[] keys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Getses the specified keys.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public T[] Gets<T>(string[] keys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove cache key and its value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clear cache.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
