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
        /// <param name="serializeFormat">The value serialize format.</param>
        public static void Set(string key, object value, int? cacheSeconds = null, SerializeFormat serializeFormat= SerializeFormat.Json)
        {
            if (value == null)
            {
                Instance.SetString(key, null, cacheSeconds);
                return;
            }

            string valueString = null;

            if (Toolkits.TryConvertToString(value, out valueString))
            {
                Instance.SetString(key, valueString, cacheSeconds);
                return;
            }

            switch (serializeFormat)
            {
                case SerializeFormat.Binary: Instance.SetBinary(key, BinarySerializer.Serialize(value), cacheSeconds); break;
                case SerializeFormat.Xml: Instance.SetString(key, XmlSerializer.Serialize(value), cacheSeconds); break;
                case SerializeFormat.Json: Instance.SetString(key, JsonSerializer.Serialize(value), cacheSeconds); break;
            }
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="serializeFormat">The value serialize format.</param>
        /// <returns>If there has matched data, return the cached object, otherwise return null.</returns>
        public static object Get(string key, SerializeFormat serializeFormat = SerializeFormat.Json)
        {
            switch (serializeFormat)
            {
                case SerializeFormat.Binary: return BinarySerializer.Deserialize(Instance.GetBinary(key));
                case SerializeFormat.Xml: return XmlSerializer.Deserialize(Instance.GetString(key), typeof(object));
                case SerializeFormat.Json: return JsonSerializer.Deserialize(Instance.GetString(key));
                default: throw new NotSupportedException(string.Format("serialize format not supported: {0}", serializeFormat));
            }
        }

        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <typeparam name="T">The cache value type.</typeparam>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="serializeFormat">The serialize format.</param>
        /// <returns>
        /// If there has matched data, return the cached object, otherwise return null.
        /// </returns>
        public static T Get<T>(string key, SerializeFormat serializeFormat = SerializeFormat.Json)
        {
            switch (serializeFormat)
            {
                case SerializeFormat.Binary: return BinarySerializer.Deserialize<T>(Instance.GetBinary(key));
                case SerializeFormat.Xml: return XmlSerializer.Deserialize<T>(Instance.GetString(key));
                case SerializeFormat.Json: return JsonSerializer.Deserialize<T>(Instance.GetString(key));
                default: throw new NotSupportedException(string.Format("serialize format not supported: {0}", serializeFormat));
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
