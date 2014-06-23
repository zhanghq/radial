using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Radial.Serialization;
using Radial.Security;

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
        /// Prepares the cache key.
        /// </summary>
        /// <param name="keyDepends">The objects which cache key depends on.</param>
        /// <returns></returns>
        public static string PrepareKey(IEnumerable<KeyValuePair<string, object>> keyDepends)
        {
            Checker.Parameter(keyDepends != null && keyDepends.Count() > 0, "cache key depends can not be null or empty");

            return CryptoProvider.SHA1Encrypt(JsonSerializer.Serialize(keyDepends));
        }

        /// <summary>
        /// Put cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public static void Put(string key, object value, int? cacheSeconds = null)
        {
            try
            {
                Instance.Put(key, value, cacheSeconds);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


        /// <summary>
        /// Get cache data.
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
        /// Get cache data.
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
        /// Drop cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public static void Drop(string key)
        {
            try
            {
                Instance.Drop(key);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Put cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="region">The cache region.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public static void Put(string key, string region, object value, int? cacheSeconds = null)
        {
            try
            {
                Instance.Put(key, region, value, cacheSeconds);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Drop cache region.
        /// </summary>
        /// <param name="region">The cache region.</param>
        public static void DropRegion(string region)
        {
            try
            {
                Instance.DropRegion(region);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Gets the cache regions.
        /// </summary>
        /// <returns>The cache regions.</returns>
        public static string[] GetRegions()
        {
            try
            {
                return Instance.GetRegions();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return new string[] { };
        }
    }
}
