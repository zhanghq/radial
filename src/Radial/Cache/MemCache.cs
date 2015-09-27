using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using System.Threading;
using Radial.Serialization;
using Microsoft.Practices.Unity;

namespace Radial.Cache
{
    /// <summary>
    /// Memcached cache implement class.
    /// </summary>
    public class MemCache : ICache
    {
        static object SyncRoot = new object();
        static MemcachedClient Client=null;
        static IClusterRegion ClusterRegion = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemCache"/> class.
        /// </summary>
        public MemCache()
        {
            lock (SyncRoot)
            {
                if (Client == null)
                    Client = new MemcachedClient();
                if (ClusterRegion == null && Dependency.Container.IsRegistered<IClusterRegion>())
                    ClusterRegion = Dependency.Container.Resolve<IClusterRegion>();
            }
        }

        /// <summary>
        /// Complex type serialization method.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        private static byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            return StaticVariables.Encoding.GetBytes(JsonSerializer.Serialize(obj));
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

            return JsonSerializer.Deserialize(StaticVariables.Encoding.GetString(bytes));
        }

        /// <summary>
        /// Get cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>
        /// If there has matched key, return the cached value, otherwise return null.
        /// </returns>
        public object Get(string key)
        {
            return Deserialize(Client.Get<byte[]>(CacheStatic.NormalizeKey(key)));
        }

        /// <summary>
        /// Put cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Put(string key, object value, int? cacheSeconds = null)
        {
            Put(key, null, value, cacheSeconds);
        }


        /// <summary>
        /// Drop cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public void Drop(string key)
        {
            key = CacheStatic.NormalizeKey(key);
            Client.Remove(key);

            if (ClusterRegion != null)
                ClusterRegion.Delete(key);
        }

        /// <summary>
        /// Put cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="region">The cache region.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Put(string key, string region, object value, int? cacheSeconds = null)
        {
            if (value == null)
                return;

            key = CacheStatic.NormalizeKey(key);

            if (cacheSeconds.HasValue)
                Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, Serialize(value), TimeSpan.FromSeconds(cacheSeconds.Value));
            else
                Client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, Serialize(value));

            if (!string.IsNullOrWhiteSpace(region) && ClusterRegion != null)
                ClusterRegion.Insert(key, region.Trim());
        }

        /// <summary>
        /// Drop cache region.
        /// </summary>
        /// <param name="region">The cache region.</param>
        public void DropRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region))
                return;

            if (ClusterRegion != null)
            {
                var keys = ClusterRegion.GetKeys(region.Trim());
                foreach (var k in keys)
                    Drop(k);
            }
        }


        /// <summary>
        /// Gets the cache regions.
        /// </summary>
        /// <returns>
        /// The cache regions.
        /// </returns>
        public string[] GetRegions()
        {
            if (ClusterRegion != null)
                return ClusterRegion.GetRegions();

            return new string[] { };
        }
    }
}
