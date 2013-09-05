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
        /// Complex type serialization method.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        private static byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
        }

        /// <summary>
        /// Complex type deserialization method.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        private static object Deserialize(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            return JsonSerializer.Deserialize(Encoding.UTF8.GetString(bytes));
        }

        /// <summary>
        /// Complex type deserialization method.
        /// </summary>
        /// <typeparam name="T">The deserialize type.</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        private static T Deserialize<T>(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return default(T);

            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(bytes));
        }
         

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
        /// Normalizes the cache key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>Normalize cache key.</returns>
        public static string NormalizeKey(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "cache key can not be empty or null");

            return key.Trim().ToLower();
        }




        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public static void Set(string key, object value, int? cacheSeconds = null)
        {
            Instance.Set(key, Serialize(value), cacheSeconds);
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>If there has matched value, return the cached data, otherwise return null.</returns>
        /// <param name="serializeFormat">The cache value serialize format.</param>
        public static object Get(string key, SerializeFormat serializeFormat = SerializeFormat.Json)
        {
            return Deserialize(Instance.Get(key));
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The cache value type.</typeparam>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched key, return the cached value, otherwise return null.
        /// </returns>
        public static T Get<T>(string key)
        {
            return Deserialize<T>(Instance.Get(key));
        }


        /// <summary>
        /// Remove cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public static void Remove(string key)
        {
            Instance.Remove(key);
        }
    }
}
