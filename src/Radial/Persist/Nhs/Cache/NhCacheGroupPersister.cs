using NHibernate;
using Radial.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Nhs.Cache
{
    //CREATE TABLE [dbo].[CacheGroup](
    //[CacheKey] [varchar](50) NOT NULL PRIMARY KEY, 
    //[GroupName] [nvarchar](500) NOT NULL
    //)

    /// <summary>
    /// NhCacheGroupPersister.
    /// </summary>
    public class NhCacheGroupPersister : ICacheGroupPersister
    {
        /// <summary>
        /// Storage alias.
        /// </summary>
        private readonly string StorageAlias;

        /// <summary>
        /// Initializes a new instance of the <see cref="NhCacheGroupPersister"/> class.
        /// </summary>
        public NhCacheGroupPersister()
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NhCacheGroup.StorageAlias"]))
                StorageAlias = ConfigurationManager.AppSettings["NhCacheGroup.StorageAlias"].Trim().ToLower();
        }

        /// <summary>
        /// Inserts the specified group.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="group">The group.</param>
        public void Insert(string cacheKey, string group)
        {
            if (string.IsNullOrWhiteSpace(cacheKey) || string.IsNullOrWhiteSpace(group))
                return;

            using (IUnitOfWork uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;

                ISQLQuery query1 = session.CreateSQLQuery("SELECT COUNT(0) FROM CacheGroup WHERE CacheKey=:CacheKey");
                query1.SetString("CacheKey", cacheKey);

                if (query1.UniqueResult<int>() == 0)
                {
                    ISQLQuery query2 = session.CreateSQLQuery("INSERT INTO CacheGroup (CacheKey,GroupName) VALUES (:CacheKey,:GroupName)");
                    query2.SetString("CacheKey", cacheKey);
                    query2.SetString("GroupName", group);
                    query2.ExecuteUpdate();
                }
            }
        }

        /// <summary>
        /// Gets the cache keys.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public string[] GetCacheKeys(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                return new string[] { };

            using (IUnitOfWork uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;

                ISQLQuery query1 = session.CreateSQLQuery("SELECT CacheKey FROM CacheGroup WHERE GroupName=:GroupName");
                query1.SetString("GroupName", group);

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

            using (IUnitOfWork uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;

                ISQLQuery query1 = session.CreateSQLQuery("DELETE FROM CacheGroup WHERE CacheKey=:CacheKey");
                query1.SetString("CacheKey", cacheKey);

                query1.ExecuteUpdate();
            }
        }

        /// <summary>
        /// Deletes the group.
        /// </summary>
        /// <param name="group">The group.</param>
        public void DeleteGroup(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                return;

            using (IUnitOfWork uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;

                ISQLQuery query1 = session.CreateSQLQuery("DELETE FROM CacheGroup WHERE GroupName=:GroupName");
                query1.SetString("GroupName", group);

                query1.ExecuteUpdate();
            }
        }
    }
}
