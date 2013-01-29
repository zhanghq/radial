using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using NHibernate;
using NHibernate.Cache;

namespace Radial.Persist.Nhs.Memcached
{
    public class CacheClient : ICache
    {
        IMemcachedClient _enyimClient;

        const string DefaultRegionName = "nmch";

        static readonly IInternalLogger Logger = LoggerProvider.LoggerFor(typeof(CacheClient));

        string _regionName;
        string _region_prefix;
        int _default_expiration=300;
        int _default_lock_timeout=0;


        /// <summary>
        /// Normalizes the name of the region.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public static string NormalizeRegionName(string regionName, IDictionary<string, string> properties)
        {
            if (string.IsNullOrWhiteSpace(regionName))
            {
                if (properties.ContainsKey("cache.default_region") && !string.IsNullOrWhiteSpace(properties["cache.default_region"]))
                    return properties["cache.default_region"].Trim();
                else
                    return DefaultRegionName;
            }

            return regionName.Trim();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheClient" /> class.
        /// </summary>
        /// <param name="memClient">The mem client.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <param name="properties">The config properties.</param>
        public CacheClient(IMemcachedClient memClient, string regionName, IDictionary<string, string> properties)
        {
            _enyimClient = memClient;

            _regionName = NormalizeRegionName(regionName, properties);

            if (properties.ContainsKey("cache.default_expiration") && !string.IsNullOrWhiteSpace(properties["cache.default_expiration"]))
                int.TryParse(properties["cache.default_expiration"], out _default_expiration);
            if (properties.ContainsKey("cache.region_prefix") && !string.IsNullOrWhiteSpace(properties["cache.region_prefix"]))
                _region_prefix = properties["cache.region_prefix"].Trim();
            if (properties.ContainsKey("cache.default_lock_timeout") && !string.IsNullOrWhiteSpace(properties["cache.default_lock_timeout"]))
                int.TryParse(properties["cache.default_lock_timeout"], out _default_lock_timeout);

        }

        /// <summary>
        /// Clear the Cache
        /// </summary>
        public void Clear()
        {
            Logger.Debug("clear cache");

            _enyimClient.FlushAll();
        }

        /// <summary>
        /// Clean up.
        /// </summary>
        public void Destroy()
        {
            Clear();
        }

        /// <summary>
        /// Get the object from the Cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(object key)
        {
            string stringkey=BuildRealCacheKey(key);
            Logger.DebugFormat("get from cache, key: {0}", stringkey);

            return _enyimClient.Get(stringkey);
        }

        /// <summary>
        /// If this is a clustered cache, lock the item
        /// </summary>
        /// <param name="key">The Key of the Item in the Cache to lock.</param>
        public void Lock(object key)
        {
        }

        /// <summary>
        /// Generate a timestamp
        /// </summary>
        /// <returns></returns>
        public long NextTimestamp()
        {
            return Timestamper.Next();
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(object key, object value)
        {
            string stringkey = BuildRealCacheKey(key);
            TimeSpan ts = TimeSpan.FromSeconds(_default_expiration);

            Logger.DebugFormat("put to cache, key: {0}, validFor: {1}", stringkey, ts.ToString());

            _enyimClient.Store(Enyim.Caching.Memcached.StoreMode.Set, stringkey, value, ts);
        }

        /// <summary>
        /// Gets the name of the cache region
        /// </summary>
        public string RegionName
        {
            get { return _regionName; }
        }

        /// <summary>
        /// Remove an item from the Cache.
        /// </summary>
        /// <param name="key">The Key of the Item in the Cache to remove.</param>
        public void Remove(object key)
        {
            string stringkey = BuildRealCacheKey(key);

            Logger.DebugFormat("remove from cache, key: {0}", stringkey);

            _enyimClient.Remove(stringkey);
        }

        /// <summary>
        /// Get a reasonable "lock timeout"
        /// </summary>
        public int Timeout
        {
            get { return _default_lock_timeout; }
        }

        /// <summary>
        /// If this is a clustered cache, unlock the item
        /// </summary>
        /// <param name="key">The Key of the Item in the Cache to unlock.</param>
        public void Unlock(object key)
        {
        }

        /// <summary>
        /// Builds the real cache key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string BuildRealCacheKey(object key)
        {
            if (key == null)
                throw new ArgumentNullException("cache key can not be null");

            IList<string> plist = new List<string>();

            if (!string.IsNullOrWhiteSpace(_region_prefix))
                plist.Add(_region_prefix);
            if (!string.IsNullOrWhiteSpace(_regionName))
                plist.Add(_regionName);

            return string.Join("@", plist) + key.ToString();
        }
    }
}
