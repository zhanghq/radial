using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Radial.Cache
{
    /// <summary>
    /// The entrance static class of cache.
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
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public static void SetBinary(string key, byte[] value, int? cacheSeconds = null)
        {
            Instance.SetBinary(key, value, cacheSeconds);
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>If there has matched data, return the cached binary, otherwise return null.</returns>
        public static byte[] GetBinary(string key)
        {
            return Instance.GetBinary(key);
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public static void SetString(string key, string value, int? cacheSeconds = null)
        {
            Instance.SetString(key, value, cacheSeconds);
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>If there has matched data, return the cached string, otherwise return null.</returns>
        public static string GetString(string key)
        {
            return Instance.GetString(key);
        }


        /// <summary>
        /// Remove cache key and its value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public static void Remove(string key)
        {
            Instance.Remove(key);
        }
    }
}
