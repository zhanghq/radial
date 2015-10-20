using System;
using System.Collections.Generic;
using Enyim.Caching;
using NHibernate;
using NHibernate.Cache;
using Radial.Security;

namespace Radial.Persist.Nhs.Cache.Memcached
{
    /// <summary>
    /// CacheClient
    /// </summary>
    public class CacheClient : ICache
    {
        IMemcachedClient _enyimClient;

        static readonly IInternalLogger Logger = LoggerProvider.LoggerFor(typeof(CacheClient));

        string _regionName;
        string _region_prefix;
        int _default_expiration=1200;


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheClient" /> class.
        /// </summary>
        /// <param name="memClient">The mem client.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <param name="properties">The config properties.</param>
        public CacheClient(IMemcachedClient memClient, string regionName, IDictionary<string, string> properties)
        {
            _enyimClient = memClient;

            _regionName = regionName;

            if (properties.ContainsKey("cache.default_expiration") && !string.IsNullOrWhiteSpace(properties["cache.default_expiration"]))
                int.TryParse(properties["cache.default_expiration"], out _default_expiration);
            if (properties.ContainsKey("cache.region_prefix") && !string.IsNullOrWhiteSpace(properties["cache.region_prefix"]))
                _region_prefix = properties["cache.region_prefix"].Trim();

        }

        /// <summary>
        /// Clear the Cache
        /// </summary>
        public void Clear()
        {
            Logger.Debug("Clear memcached");

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

            object obj= _enyimClient.Get(stringkey);

            Logger.DebugFormat("Get from memcached, key: {0}, exists={1}", stringkey, obj == null ? "no" : "yes");

            return obj;
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

            Logger.DebugFormat("Set to memcached, key: {0}, validFor: {1}", stringkey, ts.ToString());

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

            Logger.DebugFormat("Remove from memcached, key: {0}", stringkey);

            _enyimClient.Remove(stringkey);
        }

        /// <summary>
        /// Get a reasonable "lock timeout"
        /// </summary>
        public int Timeout
        {
            get { return 0; }
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
            IList<string> plist = new List<string>();

            if (!string.IsNullOrWhiteSpace(_region_prefix))
                plist.Add(_region_prefix);
            if (!string.IsNullOrWhiteSpace(_regionName))
                plist.Add(_regionName);

            return CryptoProvider.MD5Encrypt(string.Join("#", plist) + "#" + key.ToString());
        }
    }
}
