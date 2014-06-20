using Radial.Security;
using Radial.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Radial.Cache
{

    /// <summary>
    /// ICacheGroupPersister.
    /// </summary>
    public interface ICacheGroupPersister
    {
        /// <summary>
        /// Inserts the specified group.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="group">The group.</param>
        void Insert(string cacheKey, string group);
        /// <summary>
        /// Gets the cache keys.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        string[] GetCacheKeys(string group);
        /// <summary>
        /// Deletes the specified cache key.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        void Delete(string cacheKey);
        /// <summary>
        /// Deletes the group.
        /// </summary>
        /// <param name="group">The group.</param>
        void DeleteGroup(string group);
    }

    /// <summary>
    /// CacheGroup.
    /// </summary>
    public static class CacheGroup
    {

        /// <summary>
        /// Gets the ICacheGroupPersister instance.
        /// </summary>
        private static ICacheGroupPersister Persister
        {
            get
            {
                if (!Components.Container.IsRegistered<ICacheGroupPersister>())
                    return null;
                return Components.Container.Resolve<ICacheGroupPersister>();
            }
        }

        /// <summary>
        /// Prepares the cache key.
        /// </summary>
        /// <param name="group">The cache group.</param>
        /// <param name="keyDepends">The objects which cache key depends on.</param>
        /// <returns></returns>
        private static Tuple<string, string> PrepareCacheKey(string group, IEnumerable<KeyValuePair<string, object>> keyDepends)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(group), "cache key group can not be empty or null");
            Checker.Parameter(keyDepends != null && keyDepends.Count() > 0, "cache key depends can not be null or empty");

            group = group.Trim();

            //cacheKey,group
            return new Tuple<string, string>(CryptoProvider.SHA1Encrypt(string.Format("{0}${1}", group, JsonSerializer.Serialize(keyDepends))), group);
        }

        /// <summary>
        /// Put cache data.
        /// </summary>
        /// <param name="group">The cache group.</param>
        /// <param name="keyDepends">The objects which cache key depends on.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public static void Put(string group, IEnumerable<KeyValuePair<string, object>> keyDepends, object value, int? cacheSeconds = null)
        {
            var totalKey = PrepareCacheKey(group, keyDepends);
            CacheStatic.Put(totalKey.Item1, value, cacheSeconds);

            if (Persister != null)
                Persister.Insert(totalKey.Item1, totalKey.Item2);
        }


        /// <summary>
        /// Get cache data.
        /// </summary>
        /// <param name="group">The cache group.</param>
        /// <param name="keyDepends">The objects which cache key depends on.</param>
        /// <returns>If there has matched key, return the cached data, otherwise return null.</returns>
        public static object Get(string group, IEnumerable<KeyValuePair<string, object>> keyDepends)
        {
            return CacheStatic.Get(PrepareCacheKey(group, keyDepends).Item1);
        }

        /// <summary>
        /// Get cache data.
        /// </summary>
        /// <typeparam name="T">The cache value type.</typeparam>
        /// <param name="group">The cache group.</param>
        /// <param name="keyDepends">The objects which cache key depends on.</param>
        /// <param name="converter">The object to T type converter.</param>
        /// <returns>
        /// If there has matched key, return the cached value, otherwise return null.
        /// </returns>
        public static T Get<T>(string group, IEnumerable<KeyValuePair<string, object>> keyDepends, Func<object, T> converter = null)
        {
            return CacheStatic.Get<T>(PrepareCacheKey(group, keyDepends).Item1, converter);
        }



        /// <summary>
        /// Drop cache data.
        /// </summary>
        /// <param name="group">The cache group.</param>
        /// <param name="keyDepends">The objects which cache key depends on.</param>
        public static void Drop(string group, IEnumerable<KeyValuePair<string, object>> keyDepends)
        {
            var totalKey = PrepareCacheKey(group, keyDepends);
            CacheStatic.Drop(totalKey.Item1);

            if (Persister != null)
                Persister.Delete(totalKey.Item1);
        }

        /// <summary>
        /// Drop cache group.
        /// </summary>
        /// <param name="group">The cache group.</param>
        public static void DropGroup(string group)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(group), "cache key group can not be empty or null");

            group = group.Trim();

            if (Persister != null)
            {
                foreach (var key in Persister.GetCacheKeys(group))
                    CacheStatic.Drop(key);

                Persister.DeleteGroup(group);
            }
        }
    }
}
