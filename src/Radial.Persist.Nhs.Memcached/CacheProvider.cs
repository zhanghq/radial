using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enyim.Caching;
using NHibernate.Cache;

namespace Radial.Persist.Nhs.Memcached
{
    public class CacheProvider : ICacheProvider
    {
        static object SyncRoot = new object();
        static IDictionary<string, ICache> S_Caches = new Dictionary<string, ICache>();

        static IMemcachedClient S_MemcachedClient;

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
                regionName = CacheClient.NormalizeRegionName(regionName, properties);

                if (!S_Caches.ContainsKey(regionName))
                    S_Caches.Add(regionName, new CacheClient(S_MemcachedClient, regionName, properties));

                return S_Caches[regionName];
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
                if (S_MemcachedClient == null)
                    S_MemcachedClient = new MemcachedClient();
            }
        }

        /// <summary>
        /// Callback to perform any necessary cleanup of the underlying cache implementation
        /// during <see cref="M:NHibernate.ISessionFactory.Close" />.
        /// </summary>
        public void Stop()
        {
        }
    }
}
