using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Radial.Serialization;

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

        private static Logger Logger
        {
            get
            {
                return Logger.GetInstance(typeof(CacheStatic).Name);
            }
        }

        /// <summary>
        /// Normalizes the cache key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>Normalize cache key.</returns>
        public static string NormalizeKey(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "cache key can not be empty or null");

            return key.Trim();
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public static void Set(string key, object value, int? cacheSeconds = null)
        {
            try
            {
                Instance.Set(key, value, cacheSeconds);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>If there has matched key, return the cached data, otherwise return null.</returns>
        public static object Get(string key)
        {
            try
            {
                return Instance.Get(key);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The cache value type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="converter">The object to T type converter.</param>
        /// <returns>
        /// If there has matched key, return the cached value, otherwise return null.
        /// </returns>
        public static T Get<T>(string key, Func<object, T> converter = null)
        {
            var obj = Instance.Get(key);

            if (obj == null)
                return default(T);

            if (converter != null)
                return converter(obj);

            return (T)obj;
        }

        /// <summary>
        /// Remove cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public static void Remove(string key)
        {
            try
            {
                Instance.Remove(key);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
