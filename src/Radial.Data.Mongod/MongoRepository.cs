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
        protected virtual string CollectionName
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
        protected virtual MongoDatabase ReadDatabase
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
        protected virtual MongoDatabase WriteDatabase
        {
            get
            {
                return _writeDatabase;
            }
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
        public virtual void Remove(TKey key)
        {
            Remove(Find(key));
        }

        /// <summary>
        /// Removes the specified object from the repository.
        /// </summary>
        /// <param name="obj">The object.</param>
        public abstract void Remove(TObject obj);

        /// <summary>
        /// Exists the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public abstract bool Exists(TKey key);

        /// <summary>
        /// Exists the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public virtual bool Exists(System.Linq.Expressions.Expression<Func<TObject, bool>> where)
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
        public abstract TObject Find(TKey key);

        /// <summary>
        /// Gets the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        public virtual TObject Find(System.Linq.Expressions.Expression<Func<TObject, bool>> where)
        {
            Checker.Parameter(where != null, "where condition can not be null");

            return ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable().Where(where).SingleOrDefault();

        }

        /// <summary>
        /// Getses this instance.
        /// </summary>
        /// <returns></returns>
        public virtual IList<TObject> FindAll()
        {
            return FindAll(null, null);
        }

        /// <summary>
        /// Getses the specified order bys.
        /// </summary>
        /// <param name="orderBys">The order bys.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(OrderBySnippet<TObject>[] orderBys)
        {
            return FindAll(null, orderBys);
        }

        /// <summary>
        /// Getses the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> where, params OrderBySnippet<TObject>[] orderBys)
        {
            var query = ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable();

            if (where != null)
                query = query.Where(where);

            if (orderBys != null)
            {
                Checker.Requires(orderBys.Length == 1, "Only one OrderBy or OrderByDescending clause is allowed (use ThenBy or ThenByDescending for multiple order by clauses in your own code)");

                OrderBySnippet<TObject> order = orderBys[0];

                if (order.IsAscending)
                    query = query.OrderBy(order.Property);
                else
                    query = query.OrderByDescending(order.Property);
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
        public virtual IList<TObject> FindAll(int pageSize, int pageIndex, out int objectTotal)
        {
            return FindAll(null, pageSize, pageIndex, out objectTotal);
        }

        /// <summary>
        /// Getses the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> where, int pageSize, int pageIndex, out int objectTotal)
        {
            return FindAll(where, null, pageSize, pageIndex, out objectTotal);
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
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex, out int objectTotal)
        {
            var query = ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable();

            if (where != null)
                query = query.Where(where);

            objectTotal = query.Count();

            if (orderBys != null)
            {
                Checker.Requires(orderBys.Length == 1, "Only one OrderBy or OrderByDescending clause is allowed (use ThenBy or ThenByDescending for multiple order by clauses in your own code)");

                OrderBySnippet<TObject> order = orderBys[0];

                if (order.IsAscending)
                    query = query.OrderBy(order.Property);
                else
                    query = query.OrderByDescending(order.Property);
            }
            

            return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public virtual void Clear()
        {
            WriteDatabase.GetCollection<TObject>(CollectionName).RemoveAll();
        }


        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(int returnObjectCount)
        {
            return FindAll(null, null, returnObjectCount);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(OrderBySnippet<TObject>[] orderBys, int returnObjectCount)
        {
            return FindAll(null, orderBys, returnObjectCount);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> where, int returnObjectCount)
        {
            return FindAll(where, null, returnObjectCount);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int returnObjectCount)
        {
            var query = ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable();

            if (where != null)
                query = query.Where(where);

            if (orderBys != null)
            {
                Checker.Requires(orderBys.Length == 1, "Only one OrderBy or OrderByDescending clause is allowed (use ThenBy or ThenByDescending for multiple order by clauses in your own code)");

                OrderBySnippet<TObject> order = orderBys[0];

                if (order.IsAscending)
                    query = query.OrderBy(order.Property);
                else
                    query = query.OrderByDescending(order.Property);
            }

            return query.Take(returnObjectCount).ToList();
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(int pageSize, int pageIndex)
        {
            return FindAll(null, pageSize, pageIndex);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> where, int pageSize, int pageIndex)
        {
            return FindAll(where, null, pageSize, pageIndex);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex)
        {
            var query = ReadDatabase.GetCollection<TObject>(CollectionName).AsQueryable();

            if (where != null)
                query = query.Where(where);

            if (orderBys != null)
            {
                Checker.Requires(orderBys.Length == 1, "Only one OrderBy or OrderByDescending clause is allowed (use ThenBy or ThenByDescending for multiple order by clauses in your own code)");

                OrderBySnippet<TObject> order = orderBys[0];

                if (order.IsAscending)
                    query = query.OrderBy(order.Property);
                else
                    query = query.OrderByDescending(order.Property);
            }

            return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        }

        #endregion

        /// <summary>
        /// Find the object with the specified key.
        /// </summary>
        public TObject this[TKey key]
        {
            get { return Find(key); }
        }
    }
}
