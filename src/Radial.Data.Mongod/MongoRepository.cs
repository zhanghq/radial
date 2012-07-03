using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Data.Mongod.Cfg;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Radial.Data.Mongod
{
    /// <summary>
    /// Basic class of IRepository using MongoDB
    /// </summary>
    /// <typeparam name="TObject">The type of persistent object.</typeparam>
    /// <typeparam name="TKey">The type of the object key.</typeparam>
    public abstract class MongoRepository<TObject, TKey> : IRepository<TObject, TKey> where TObject : class
    {

        string _collectionName;
        MongoDatabase _readDatabase;
        MongoDatabase _writeDatabase;


        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository&lt;TObject, TKey&gt;"/> class.
        /// </summary>
        public MongoRepository()
        {
            PersistenceConfig persistenceConfig = MongodConfig.GetPersistenceConfig<TObject>();

            var readUrlBuilder = new MongoUrlBuilder(persistenceConfig.Servers.Read.Connection);
            var writeUrlBuilder = new MongoUrlBuilder(persistenceConfig.Servers.Read.Connection);

            _collectionName = persistenceConfig.Collection;
            _readDatabase = MongoServer.Create(persistenceConfig.Servers.Read.Connection).GetDatabase(readUrlBuilder.DatabaseName);
            _writeDatabase = MongoServer.Create(persistenceConfig.Servers.Write.Connection).GetDatabase(writeUrlBuilder.DatabaseName);
        }

        /// <summary>
        /// Gets the persistent collection name.
        /// </summary>
        protected string CollectionName
        {
            get
            {
                return _collectionName;
            }
        }

        /// <summary>
        /// Gets new MongoDatabase object of the read database .
        /// </summary>
        /// <returns>MongoDatabase object.</returns>
        protected MongoDatabase ReadDatabase
        {
            get
            {
                return _readDatabase;
            }
        }

        /// <summary>
        /// Gets new MongoDatabase object of the write database .
        /// </summary>
        /// <returns>MongoDatabase object.</returns>
        protected MongoDatabase WriteDatabase
        {
            get
            {
                return _writeDatabase;
            }
        }


        ///// <summary>
        ///// Maps the reduce.
        ///// </summary>
        ///// <param name="map">The map.</param>
        ///// <param name="reduce">The reduce.</param>
        ///// <returns></returns>
        //public virtual TObject MapReduce(string map, string reduce)
        //{
        //    TObject result = default(TObject);
        //    using (var db = CreateReadDb())
        //    {
        //        var mr = db.Database.CreateMapReduce();
        //        MapReduceResponse response =
        //            mr.Execute(new MapReduceOptions(CollectionName)
        //            {
        //                Map = map,
        //                Reduce = reduce
        //            });
        //        IMongoCollection<MapReduceResult<TObject>> coll = response.GetCollection<MapReduceResult<TObject>>();
        //        MapReduceResult<TObject> r = coll.Find().FirstOrDefault();
        //        result = r.Value;
        //    }
        //    return result;
        //}


        #region IRepository<TObject,TKey> Members

        /// <summary>
        /// Adds an object to the repository.
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Add(TObject obj)
        {
            if (obj != null)
                Add(new TObject[] { obj });
        }

        /// <summary>
        /// Adds objects to the repository.
        /// </summary>
        /// <param name="objs">The objects.</param>
        public virtual void Add(IEnumerable<TObject> objs)
        {
            if (objs != null)
            {
                WriteDatabase.GetCollection<TObject>(CollectionName).InsertBatch<TObject>(objs);
            }
        }

        /// <summary>
        /// Saves or updates the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Save(TObject obj)
        {
            if (obj != null)
            {
                WriteDatabase.GetCollection<TObject>(CollectionName).Save<TObject>(obj);
            }
        }

        /// <summary>
        /// Removes an object with the specified key from the repository.
        /// </summary>
        /// <param name="key">The object key.</param>
        public abstract void Remove(TKey key);

        /// <summary>
        /// Removes the specified object from the repository.
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Remove(TObject obj)
        {
            if (obj != null)
            {
                //WriteDatabase.GetCollection<TObject>(CollectionName).Remove(obj);
            }
        }

        /// <summary>
        /// Exists the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public abstract bool Exist(TKey key);

        /// <summary>
        /// Exists the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public virtual bool Exist(System.Linq.Expressions.Expression<Func<TObject, bool>> where)
        {
            return GetTotal(where) > 0;
        }

        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <returns></returns>
        public virtual int GetTotal()
        {
            return GetTotal(null);
        }

        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public virtual int GetTotal(System.Linq.Expressions.Expression<Func<TObject, bool>> where)
        {
            var query = ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable();

            if (where != null)
                query = query.Where(where);
            return query.Count();

        }

        /// <summary>
        /// Gets the total int64.
        /// </summary>
        /// <returns></returns>
        public virtual long GetTotalInt64()
        {
            return GetTotalInt64(null);
        }

        /// <summary>
        /// Gets the total int64.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public virtual long GetTotalInt64(System.Linq.Expressions.Expression<Func<TObject, bool>> where)
        {
            var query = ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable();

            if (where != null)
                query = query.Where(where);
            return query.LongCount();
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public abstract TObject Get(TKey key);

        /// <summary>
        /// Gets the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public virtual TObject Get(System.Linq.Expressions.Expression<Func<TObject, bool>> where)
        {
            Checker.Parameter(where != null, "where condition can not be null");

            return ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable().Where(where).SingleOrDefault();

        }

        /// <summary>
        /// Getses this instance.
        /// </summary>
        /// <returns></returns>
        public virtual IList<TObject> Gets()
        {
            return Gets(null, null);
        }

        /// <summary>
        /// Getses the specified order bys.
        /// </summary>
        /// <param name="orderBys">The order bys.</param>
        /// <returns></returns>
        public virtual IList<TObject> Gets(OrderBySnippet<TObject>[] orderBys)
        {
            return Gets(null, orderBys);
        }

        /// <summary>
        /// Getses the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <returns></returns>
        public virtual IList<TObject> Gets(System.Linq.Expressions.Expression<Func<TObject, bool>> where, params OrderBySnippet<TObject>[] orderBys)
        {
            var query = ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable();

            if (where != null)
                query = query.Where(where);

            if (orderBys != null)
            {
                foreach (OrderBySnippet<TObject> order in orderBys)
                {
                    if (order.IsAscending)
                        query = query.OrderBy(order.Property);
                    else
                        query = query.OrderByDescending(order.Property);
                }
            }

            return query.ToList();
        }

        /// <summary>
        /// Getses the specified page size.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns></returns>
        public virtual IList<TObject> Gets(int pageSize, int pageIndex, out int objectTotal)
        {
            return Gets(null, pageSize, pageIndex, out objectTotal);
        }

        /// <summary>
        /// Getses the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns></returns>
        public virtual IList<TObject> Gets(System.Linq.Expressions.Expression<Func<TObject, bool>> where, int pageSize, int pageIndex, out int objectTotal)
        {
            return Gets(where, null, pageSize, pageIndex, out objectTotal);
        }


        /// <summary>
        /// Getses the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns></returns>
        public virtual IList<TObject> Gets(System.Linq.Expressions.Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex, out int objectTotal)
        {
            var query = ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable();

            if (where != null)
                query = query.Where(where);

            objectTotal = query.Count();

            if (orderBys != null)
            {
                foreach (OrderBySnippet<TObject> order in orderBys)
                {
                    if (order.IsAscending)
                        query = query.OrderBy(order.Property);
                    else
                        query = query.OrderByDescending(order.Property);
                }
            }

            return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public virtual void Clear()
        {

            try
            {
                WriteDatabase.GetCollection<TObject>(CollectionName).RemoveAll();
            }
            catch { }

        }


        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(int returnObjectCount)
        {
            return Gets(null, null, returnObjectCount);
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(OrderBySnippet<TObject>[] orderBys, int returnObjectCount)
        {
            return Gets(null, orderBys, returnObjectCount);
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(System.Linq.Expressions.Expression<Func<TObject, bool>> where, int returnObjectCount)
        {
            return Gets(where, null, returnObjectCount);
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(System.Linq.Expressions.Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int returnObjectCount)
        {
            var query = ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable();

            if (where != null)
                query = query.Where(where);

            if (orderBys != null)
            {
                foreach (OrderBySnippet<TObject> order in orderBys)
                {
                    if (order.IsAscending)
                        query = query.OrderBy(order.Property);
                    else
                        query = query.OrderByDescending(order.Property);
                }
            }

            return query.Take(returnObjectCount).ToList();
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(int pageSize, int pageIndex)
        {
            return Gets(null, pageSize, pageIndex);
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(System.Linq.Expressions.Expression<Func<TObject, bool>> where, int pageSize, int pageIndex)
        {
            return Gets(where, null, pageSize, pageIndex);
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(System.Linq.Expressions.Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex)
        {
            var query = ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable();

            if (where != null)
                query = query.Where(where);

            if (orderBys != null)
            {
                foreach (OrderBySnippet<TObject> order in orderBys)
                {
                    if (order.IsAscending)
                        query = query.OrderBy(order.Property);
                    else
                        query = query.OrderByDescending(order.Property);
                }
            }

            return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        }

        #endregion

        /// <summary>
        /// Gets the object with the specified key.
        /// </summary>
        public TObject this[TKey key]
        {
            get { return Get(key); }
        }
    }
}
