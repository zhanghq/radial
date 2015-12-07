using NHibernate;
using Radial.Cache;
using System.Configuration;
using System.Linq;

namespace Radial.Persist.Nhs.Cache
{
    //CREATE TABLE [dbo].[CacheRegion](
    //[CacheKey] [varchar](50) NOT NULL PRIMARY KEY, 
    //[RegionName] [nvarchar](500) NOT NULL
    //)


    /// <summary>
    /// NhClusterRegion.
    /// </summary>
    public class NhClusterRegion : IClusterRegion
    {
        /// <summary>
        /// Storage alias.
        /// </summary>
        private readonly string StorageAlias;

        /// <summary>
        /// Initializes a new instance of the <see cref="NhClusterRegion"/> class.
        /// </summary>
        public NhClusterRegion()
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NhClusterRegion.StorageAlias"]))
                StorageAlias = ConfigurationManager.AppSettings["NhClusterRegion.StorageAlias"].Trim().ToLower();
        }


        /// <summary>
        /// Inserts cache key with the specified region.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="region">The cache region.</param>
        public void Insert(string cacheKey, string region)
        {
            if (string.IsNullOrWhiteSpace(cacheKey) || string.IsNullOrWhiteSpace(region))
                return;

            using (IUnitOfWork uow = new UnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;

                ISQLQuery query1 = session.CreateSQLQuery("SELECT COUNT(0) FROM CacheRegion WHERE CacheKey=:CacheKey");
                query1.SetString("CacheKey", cacheKey);

                if (query1.UniqueResult<int>() == 0)
                {
                    ISQLQuery query2 = session.CreateSQLQuery("INSERT INTO CacheRegion (CacheKey,RegionName) VALUES (:CacheKey,:RegionName)");
                    query2.SetString("CacheKey", cacheKey);
                    query2.SetString("RegionName", region);
                    query2.ExecuteUpdate();
                }
            }
        }

        /// <summary>
        /// Gets the cache keys contained in the specified region.
        /// </summary>
        /// <param name="region">The cache region.</param>
        /// <returns></returns>
        public string[] GetKeys(string region)
        {
            if (string.IsNullOrWhiteSpace(region))
                return new string[] { };

            using (IUnitOfWork uow = new UnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;

                ISQLQuery query1 = session.CreateSQLQuery("SELECT CacheKey FROM CacheRegion WHERE RegionName=:RegionName");
                query1.SetString("RegionName", region);

                return query1.List<string>().ToArray();
            }
        }

        /// <summary>
        /// Deletes the specified cache key.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        public void Delete(string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
                return;

            using (IUnitOfWork uow = new UnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;

                ISQLQuery query1 = session.CreateSQLQuery("DELETE FROM CacheRegion WHERE CacheKey=:CacheKey");
                query1.SetString("CacheKey", cacheKey);

                query1.ExecuteUpdate();
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
            using (IUnitOfWork uow = new UnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;

                ISQLQuery query1 = session.CreateSQLQuery("SELECT DISTINCT RegionName FROM CacheRegion");

                return query1.List<string>().ToArray();
            }
        }
    }
}
