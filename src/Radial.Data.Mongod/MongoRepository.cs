using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Norm;
using Norm.Collections;
using Norm.Responses;
using Norm.Linq;
using Radial.Data.Mongod.Cfg;

namespace Radial.Data.Mongod
{
    /// <summary>
    /// Basic class of IRepository using MongoDB
    /// </summary>
    /// <typeparam name="TObject">The type of persistent object.</typeparam>
    /// <typeparam name="TKey">The type of the object key.</typeparam>
    public abstract class MongoRepository<TObject, TKey> : IRepository<TObject, TKey> where TObject : class
    {
        /// <summary>
        /// Gets the mongod context.
        /// </summary>
        protected MongodContext DbContext
        {
            get
            {
                return MongodConfig.GetContext<TObject>();
            }
        }

        /// <summary>
        /// Gets the servers.
        /// </summary>
        protected ServerGroup Servers
        {
            get
            {
                return DbContext.Servers;
            }
        }

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        protected string CollectionName
        {
            get
            {
                string name = DbContext.CollectionName;
                if (string.IsNullOrWhiteSpace(name))
                    return typeof(TObject).Name;
                return name;
            }
        }

        /// <summary>
        /// Creates IMongo object of the read database .
        /// </summary>
        /// <returns>IMongo object.</returns>
        protected IMongo CreateReadDb()
        {
            return Mongo.Create(Servers.Read.Connection);
        }

        /// <summary>
        /// Creates IMongo object of the write database .
        /// </summary>
        /// <returns>IMongo object.</returns>
        protected IMongo CreateWriteDb()
        {
            return Mongo.Create(Servers.Write.Connection);
        }


        /// <summary>
        /// Maps the reduce.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="reduce">The reduce.</param>
        /// <returns></returns>
        public virtual TObject MapReduce(string map, string reduce)
        {
            TObject result = default(TObject);
            using (var db = CreateReadDb())
            {
                var mr = db.Database.CreateMapReduce();
                MapReduceResponse response =
                    mr.Execute(new MapReduceOptions(CollectionName)
                    {
                        Map = map,
                        Reduce = reduce
                    });
                IMongoCollection<MapReduceResult<TObject>> coll = response.GetCollection<MapReduceResult<TObject>>();
                MapReduceResult<TObject> r = coll.Find().FirstOrDefault();
                result = r.Value;
            }
            return result;
        }


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
                using (var db = CreateWriteDb())
                {
                    db.GetCollection<TObject>(CollectionName).Insert(objs);
                }
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
                using (var db = CreateWriteDb())
                {
                    db.GetCollection<TObject>(CollectionName).Save(obj);
                }
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
                using (var db = CreateWriteDb())
                {
                    db.GetCollection<TObject>(CollectionName).Delete(obj);
                }
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
            using (var db = CreateReadDb())
            {
                var query = db.GetCollection<TObject>(CollectionName).AsQueryable();

                if (where != null)
                    query = query.Where(where);
                return query.Count();
            }
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
            using (var db = CreateReadDb())
            {
                var query = db.GetCollection<TObject>(CollectionName).AsQueryable();

                if (where != null)
                    query = query.Where(where);
                return query.LongCount();
            }
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

            using (var db = CreateReadDb())
            {
                return db.GetCollection<TObject>(CollectionName).AsQueryable().Where(where).SingleOrDefault();
            }
        }

        /// <summary>
        /// Getses this instance.
        /// </summary>
        /// <returns></returns>
        public virtual IList<TObject> Gets()
        {
            return Gets(null,null);
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
            if (orderBys != null)
                throw new NotSupportedException("OrderBySnippet<TObject> is not supported in MongoRepository, please use native Linq to specify order by");

            using (var db = CreateReadDb())
            {
                var query = db.GetCollection<TObject>(CollectionName).AsQueryable();

                if (where != null)
                    query = query.Where(where);

                return query.ToList();
            }
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
            if (orderBys != null)
                throw new NotSupportedException("OrderBySnippet<TObject> is not supported in MongoRepository, please use native Linq to specify order by");

            using (var db = CreateReadDb())
            {
                var query = db.GetCollection<TObject>(CollectionName).AsQueryable();

                if (where != null)
                    query = query.Where(where);

                objectTotal = query.Count();

                return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public virtual void Clear()
        {
            if (SystemVariables.CompileType == CompileType.Debug)
            {
                using (var db = CreateWriteDb())
                {
                    try
                    {
                        db.Database.DropCollection(CollectionName);
                    }
                    catch { }
                }
            }
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
            if (orderBys != null)
                throw new NotSupportedException("OrderBySnippet<TObject> is not supported in MongoRepository, please use native Linq to specify order by");

            using (var db = CreateReadDb())
            {
                var query = db.GetCollection<TObject>(CollectionName).AsQueryable();

                if (where != null)
                    query = query.Where(where);

                return query.Take(returnObjectCount).ToList();
            }
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
            if (orderBys != null)
                throw new NotSupportedException("OrderBySnippet<TObject> is not supported in MongoRepository, please use native Linq to specify order by");

            using (var db = CreateReadDb())
            {
                var query = db.GetCollection<TObject>(CollectionName).AsQueryable();

                if (where != null)
                    query = query.Where(where);

                return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            }
        }

        #endregion
    }
}
