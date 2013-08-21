using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using NHibernate.Cache;

namespace Radial.Persist.Nhs.Cache.Memcached
{
    /// <summary>
    /// CacheProvider
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        static object SyncRoot = new object();
        static IDictionary<string, ICache> Caches = new Dictionary<string, ICache>();

        static IMemcachedClient MemcachedClient;

        /// <summary>
        /// Configure the cache
        /// </summary>
        /// <param name="regionName">the name of the cache region</param>
        /// <param name="properties">configuration settings</param>
        /// <returns></returns>
        public ICache BuildCache(string regionName, IDictionary<string, string> properties)
        {
            lock (SyncRoot)
            {
                if (!Caches.ContainsKey(regionName))
                    Caches.Add(regionName, new CacheClient(MemcachedClient, regionName, properties));

                return Caches[regionName];
            }
        }

        /// <summary>
        /// generate a timestamp
        /// </summary>
        /// <returns></returns>
        public long NextTimestamp()
        {
            return Timestamper.Next();
        }

        /// <summary>
        /// Callback to perform any necessary initialization of the underlying cache implementation
        /// during ISessionFactory construction.
        /// </summary>
        /// <param name="properties">current configuration settings</param>
        public void Start(IDictionary<string, string> properties)
        {
            lock (SyncRoot)
            {
                if (MemcachedClient == null)
                    MemcachedClient = new MemcachedClient();
            }
        }

        /// <summary>
        /// Callback to perform any necessary cleanup of the underlying cache implementation
        /// during <see cref="M:NHibernate.ISessionFactory.Close" />.
        /// </summary>
        public void Stop()
        {
            MemcachedClient.Dispose();
        }
    }
}
