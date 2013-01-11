using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using System.Collections;

namespace Radial.Data.Nhs
{
    /// <summary>
    /// Basic class of IRepository
    /// </summary>
    /// <typeparam name="TObject">The type of persistent object.</typeparam>
    /// <typeparam name="TKey">The type of the object key.</typeparam>
    public abstract class BasicRepository<TObject, TKey> : IRepository<TObject, TKey> where TObject : class
    {
        IUnitOfWorkEssential _uow;

        static string[] SupportedAggregationResultTypeNames = new string[] { typeof(int).FullName, typeof(long).FullName, typeof(decimal).FullName, typeof(float).FullName, typeof(double).FullName };

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicRepository&lt;TObject, TKey&gt;"/> class.
        /// </summary>
        /// <param name="uow">The IUnitOfWorkEssential instance.</param>
        public BasicRepository(IUnitOfWorkEssential uow)
        {
            Checker.Parameter(uow != null, "the IUnitOfWorkEssential instance can not be null");
            _uow = uow;
        }

        /// <summary>
        /// Gets the NHibernate session object.
        /// </summary>
        protected virtual ISession Session
        {
            get
            {
                return (ISession)_uow.UnderlyingContext;
            }
        }

        /// <summary>
        /// Determine whether the object is exists.
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <returns>
        ///   <c>true</c> if the object is exists; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Exist(TKey key)
        {
            var metadata = Session.SessionFactory.GetClassMetadata(typeof(TObject));

            Checker.Requires(metadata.HasIdentifierProperty, "{0} does not has identifier property", typeof(TObject).FullName);

            IQuery query = Session.CreateQuery(string.Format("select count(*) from {0} o condition o.{1}=:key", typeof(TObject).Name, metadata.IdentifierPropertyName));
            query.SetParameter("key", key);

            return Convert.ToInt32(query.UniqueResult()) > 0;
        }

        /// <summary>
        /// Determine whether contains objects that match the where condition.
        /// </summary>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        ///   <c>true</c> if objects that match the where condition is exists; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Exist(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            return GetCount(condition) > 0;
        }

        /// <summary>
        /// Determine whether contains objects that match the where condition.
        /// </summary>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        ///   <c>true</c> if objects that match the where condition is exists; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool Exist(ICriterion condition)
        {
            return GetCount(condition) > 0;
        }


        /// <summary>
        /// Gets objects total.
        /// </summary>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual int GetCount()
        {
            return Session.QueryOver<TObject>().RowCount();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual int GetCount(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            if (condition != null)
                return Session.QueryOver<TObject>().Where(condition).RowCount();
            return Session.QueryOver<TObject>().RowCount();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        protected virtual int GetCount(ICriterion condition)
        {
            if (condition != null)
                return Session.QueryOver<TObject>().Where(condition).RowCount();
            return Session.QueryOver<TObject>().RowCount();
        }

        /// <summary>
        /// Counts objects total.
        /// </summary>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual long GetCountInt64()
        {
            return Session.QueryOver<TObject>().RowCountInt64();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual long GetCountInt64(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            if (condition != null)
                return Session.QueryOver<TObject>().Where(condition).RowCountInt64();
            return Session.QueryOver<TObject>().RowCountInt64();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        protected virtual long GetCountInt64(ICriterion condition)
        {
            if (condition != null)
                return Session.QueryOver<TObject>().Where(condition).RowCountInt64();
            return Session.QueryOver<TObject>().RowCountInt64();
        }

        /// <summary>
        /// Find object with the specified key.
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <returns>If data exists, return the object, otherwise return null.</returns>
        public virtual TObject Find(TKey key)
        {
            return Session.Get<TObject>(key);
        }

        /// <summary>
        /// Find object.
        /// </summary>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        /// If data exists, return the object, otherwise return null.
        /// </returns>
        public virtual TObject Find(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            Checker.Parameter(condition != null, "where condition can not be null");
            return Session.QueryOver<TObject>().Where(condition).SingleOrDefault();
        }

        /// <summary>
        /// Find object.
        /// </summary>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        /// If data exists, return the object, otherwise return null.
        /// </returns>
        protected virtual TObject Get(ICriterion condition)
        {
            Checker.Parameter(condition != null, "where condition can not be null");
            return Session.QueryOver<TObject>().Where(condition).SingleOrDefault();
        }

        /// <summary>
        /// Find the object with the specified key.
        /// </summary>
        public TObject this[TKey key]
        {
            get { return Find(key); }
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll()
        {
            return FindAll(null, null);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(OrderBySnippet<TObject>[] orderBys)
        {
            return FindAll(null, orderBys);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The where condition</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>If data exists, return an objects list, otherwise return an empty list.</returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, params OrderBySnippet<TObject>[] orderBys)
        {
            IQueryOver<TObject, TObject> query = Session.QueryOver<TObject>();

            if (condition != null)
                query = query.Where(condition);

            if (orderBys != null)
            {
                foreach (OrderBySnippet<TObject> order in orderBys)
                {
                    if (order.IsAscending)
                        query = query.OrderBy(order.Property).Asc;
                    else
                        query = query.OrderBy(order.Property).Desc;
                }
            }

            return query.List();
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(int pageSize, int pageIndex, out int objectTotal)
        {
            return FindAll(null, pageSize, pageIndex, out objectTotal);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The where condition</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, int pageSize, int pageIndex, out int objectTotal)
        {
            return FindAll(condition, null, pageSize, pageIndex, out objectTotal);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The where condition</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex, out int objectTotal)
        {
            IQueryOver<TObject, TObject> countQuery = Session.QueryOver<TObject>();
            IQueryOver<TObject, TObject> dataQuery = Session.QueryOver<TObject>();

            if (condition != null)
            {
                countQuery = countQuery.Where(condition);
                dataQuery = dataQuery.Where(condition);
            }

            if (orderBys != null)
            {
                foreach (OrderBySnippet<TObject> order in orderBys)
                {
                    if (order.IsAscending)
                        dataQuery = dataQuery.OrderBy(order.Property).Asc;
                    else
                        dataQuery = dataQuery.OrderBy(order.Property).Desc;
                }
            }

            return ExecutePagingQuery(countQuery, dataQuery, pageSize, pageIndex, out objectTotal);
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
        /// <param name="condition">The where condition.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, int returnObjectCount)
        {
            return FindAll(condition, null, returnObjectCount);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The where condition.</param>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, OrderBySnippet<TObject>[] orderBys, int returnObjectCount)
        {
            IQueryOver<TObject, TObject> query = Session.QueryOver<TObject>();

            if (condition != null)
                query = query.Where(condition);

            if (orderBys != null)
            {
                foreach (OrderBySnippet<TObject> order in orderBys)
                {
                    if (order.IsAscending)
                        query = query.OrderBy(order.Property).Asc;
                    else
                        query = query.OrderBy(order.Property).Desc;
                }
            }

            return query.Take(returnObjectCount).List();
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
        /// <param name="condition">The where condition</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, int pageSize, int pageIndex)
        {
            return FindAll(condition, null, pageSize, pageIndex);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The where condition</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex)
        {
            IQueryOver<TObject, TObject> query = Session.QueryOver<TObject>();

            if (condition != null)
                query = query.Where(condition);

            if (orderBys != null)
            {
                foreach (OrderBySnippet<TObject> order in orderBys)
                {
                    if (order.IsAscending)
                        query = query.OrderBy(order.Property).Asc;
                    else
                        query = query.OrderBy(order.Property).Desc;
                }
            }

            return ExecutePagingQuery(query, pageSize, pageIndex);
        }

        /// <summary>
        /// Adds an object to the repository (or delegate to RegisterNew method if using unit of work).
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Add(TObject obj)
        {
            _uow.RegisterNew<TObject>(obj);
        }

        /// <summary>
        /// Adds objects to the repository (or delegate to RegisterNew method if using unit of work).
        /// </summary>
        /// <param name="objs">The objects.</param>
        public virtual void Add(IEnumerable<TObject> objs)
        {
            _uow.RegisterNew<TObject>(objs);
        }

        /// <summary>
        /// Saves or updates the specified object (or delegate to RegisterSave method if using unit of work).
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Save(TObject obj)
        {
            _uow.RegisterSave<TObject>(obj);
        }

        /// <summary>
        /// Removes an object with the specified key from the repository (or delegate to RegisterDelete method if using unit of work).
        /// </summary>
        /// <param name="key">The object key.</param>
        public virtual void Remove(TKey key)
        {
            _uow.RegisterDelete<TObject, TKey>(key);
        }

        /// <summary>
        /// Removes the specified object from the repository (or delegate to RegisterDelete method if using unit of work).
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Remove(TObject obj)
        {
            _uow.RegisterDelete<TObject>(obj);
        }

        /// <summary>
        /// Clear all objects (or delegate to RegisterClear method if using unit of work).
        /// </summary>
        public virtual void Clear()
        {
            _uow.RegisterClear<TObject>();
        }

        /// <summary>
        /// To unique item list.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>The list does not contains duplicate elements</returns>
        protected virtual IList<TObject> ToUniqueList(IEnumerable<TObject> collection)
        {
            ISet<TObject> set = new HashSet<TObject>(collection);

            return set.ToList();
        }

        #region Multi Query

        /// <summary>
        /// Creates multiple queries object.
        /// </summary>
        /// <param name="hqls">The hql queries.</param>
        /// <returns>The query result list.</returns>
        protected virtual IMultiQuery CreateMultiQuery(string[] hqls)
        {
            Checker.Parameter(hqls != null, "multiple hql queries can not be null");

            IMultiQuery mq = Session.CreateMultiQuery();
            foreach (string q in hqls)
                mq.Add(q);
            return mq;
        }

        /// <summary>
        /// Creates multiple queries object.
        /// </summary>
        /// <param name="criterias">The criterias.</param>
        /// <returns>The IMultiCriteria object.</returns>
        protected virtual IMultiCriteria CreateMultiQuery(ICriteria[] criterias)
        {
            Checker.Parameter(criterias != null, "multiple criterias can not be null");

            IMultiCriteria mc = Session.CreateMultiCriteria();
            foreach (ICriteria c in criterias)
                mc.Add(c);
            return mc;
        }

        /// <summary>
        /// Creates multiple queries object.
        /// </summary>
        /// <param name="queries">The queries.</param>
        /// <returns>The IMultiQuery object.</returns>
        protected virtual IMultiQuery CreateMultiQuery(IQuery[] queries)
        {
            Checker.Parameter(queries != null, "multiple queries can not be null");

            IMultiQuery mq = Session.CreateMultiQuery();
            foreach (IQuery q in queries)
                mq.Add(q);
            return mq;
        }

        /// <summary>
        /// Creates multiple queries object.
        /// </summary>
        /// <param name="queries">The queries.</param>
        /// <returns>The IMultiQuery object.</returns>
        protected virtual IMultiQuery CreateMultiQuery(IQueryOver<TObject>[] queries)
        {
            Checker.Parameter(queries != null, "multiple queries can not be null");

            IMultiQuery mq = Session.CreateMultiQuery();
            foreach (IQuery q in queries)
                mq.Add(q);
            return mq;
        }

        /// <summary>
        /// Creates multiple queries object.
        /// </summary>
        /// <param name="hqls">The hql queries.</param>
        /// <returns>The IMultiQuery object.</returns>
        protected virtual IMultiQuery CreateMultiQuery(KeyValuePair<string, string>[] hqls)
        {
            Checker.Parameter(hqls != null, "multiple hql queries can not be null");

            IMultiQuery mq = Session.CreateMultiQuery();
            foreach (KeyValuePair<string, string> q in hqls)
                mq.Add(q.Key, q.Value);
            return mq;
        }

        /// <summary>
        /// Creates multiple queries object.
        /// </summary>
        /// <param name="criterias">The criterias.</param>
        /// <returns>The IMultiCriteria object.</returns>
        protected virtual IMultiCriteria CreateMultiQuery(KeyValuePair<string, ICriteria>[] criterias)
        {
            Checker.Parameter(criterias != null, "multiple criterias can not be null");

            IMultiCriteria mc = Session.CreateMultiCriteria();
            foreach (KeyValuePair<string, ICriteria> c in criterias)
                mc.Add(c.Key, c.Value);
            return mc;
        }

        /// <summary>
        /// Creates multiple queries object.
        /// </summary>
        /// <param name="queries">The queries.</param>
        /// <returns>The IMultiQuery object.</returns>
        protected virtual IMultiQuery CreateMultiQuery(KeyValuePair<string, IQuery>[] queries)
        {
            Checker.Parameter(queries != null, "multiple queries can not be null");

            IMultiQuery mq = Session.CreateMultiQuery();
            foreach (KeyValuePair<string, IQuery> q in queries)
                mq.Add(q.Key, q.Value);
            return mq;
        }


        #endregion

        /// <summary>
        /// Normalizes the page index parameter.
        /// </summary>
        /// <param name="pageIndex">The page index parameter.</param>
        /// <returns>The normalized page index parameter.</returns>
        protected int NormalizePageIndex(int pageIndex)
        {
            if (pageIndex < 1)
                return 1;

            return pageIndex;
        }


        /// <summary>
        /// Normalizes the page size parameter.
        /// </summary>
        /// <param name="pageSize">The page size parameter.</param>
        /// <returns>The normalized page size parameter.</returns>
        protected int NormalizePageSize(int pageSize)
        {
            if (pageSize < 0)
                return 0;

            return pageSize;
        }

        #region Execute Paging Query

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="countQuery">The count query(must contains count sql statements).</param>
        /// <param name="dataQuery">The data query.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>The query result list</returns>
        protected virtual IList<TObject> ExecutePagingQuery(IQuery countQuery, IQuery dataQuery, int pageSize, int pageIndex, out int objectTotal)
        {
            Checker.Parameter(countQuery != null, "countQuery can not be null");
            Checker.Parameter(dataQuery != null, "dataQuery can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);

            IMultiQuery query = Session.CreateMultiQuery();
            query.Add<int>(countQuery);
            query.Add<TObject>(dataQuery.SetFirstResult(pageSize * (pageIndex - 1)).SetMaxResults(pageSize));

            IList resultList = query.List();

            objectTotal = ((IList<int>)resultList[0])[0];

            return (IList<TObject>)resultList[1];
        }

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="countCriteria">The count criteria.</param>
        /// <param name="dataCriteria">The data criteria.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>The query result list</returns>
        protected virtual IList<TObject> ExecutePagingQuery(ICriteria countCriteria, ICriteria dataCriteria, int pageSize, int pageIndex, out int objectTotal)
        {
            Checker.Parameter(countCriteria != null, "countCriteria can not be null");
            Checker.Parameter(dataCriteria != null, "dataCriteria can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);

            IMultiCriteria criteria = Session.CreateMultiCriteria();
            criteria.Add<int>(countCriteria.SetProjection(Projections.RowCount()));
            criteria.Add<TObject>(dataCriteria.SetFirstResult(pageSize * (pageIndex - 1)).SetMaxResults(pageSize));

            IList resultList = criteria.List();

            objectTotal = ((IList<int>)resultList[0])[0];

            return (IList<TObject>)resultList[1];
        }

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="countQuery">The count query.</param>
        /// <param name="dataQuery">The data query.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>The query result list</returns>
        protected virtual IList<TObject> ExecutePagingQuery(IQueryOver<TObject> countQuery, IQueryOver<TObject> dataQuery, int pageSize, int pageIndex, out int objectTotal)
        {
            Checker.Parameter(countQuery != null, "countQuery can not be null");
            Checker.Parameter(dataQuery != null, "dataQuery can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);

            IMultiCriteria criteria = Session.CreateMultiCriteria();
            criteria.Add<int>(countQuery.ToRowCountQuery());
            criteria.Add<TObject>(dataQuery.Skip(pageSize * (pageIndex - 1)).Take(pageSize));

            IList resultList = criteria.List();

            objectTotal = ((IList<int>)resultList[0])[0];

            return (IList<TObject>)resultList[1];
        }

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="dataQuery">The data query.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>The query result list</returns>
        protected virtual IList<TObject> ExecutePagingQuery(IQuery dataQuery, int pageSize, int pageIndex)
        {
            Checker.Parameter(dataQuery != null, "dataQuery can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);

            return dataQuery.SetFirstResult(pageSize * (pageIndex - 1)).SetMaxResults(pageSize).List<TObject>();
        }

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="dataCriteria">The data criteria.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>The query result list</returns>
        protected virtual IList<TObject> ExecutePagingQuery(ICriteria dataCriteria, int pageSize, int pageIndex)
        {
            Checker.Parameter(dataCriteria != null, "dataCriteria can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);

            return dataCriteria.SetFirstResult(pageSize * (pageIndex - 1)).SetMaxResults(pageSize).List<TObject>();
        }

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="dataQuery">The data query.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>The query result list</returns>
        protected virtual IList<TObject> ExecutePagingQuery(IQueryOver<TObject> dataQuery, int pageSize, int pageIndex)
        {
            Checker.Parameter(dataQuery != null, "dataQuery can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);


            return dataQuery.Skip(pageSize * (pageIndex - 1)).Take(pageSize).List();
        }

        #endregion


        /// <summary>
        /// Gets the min value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>The min value.</returns>
        public TResult GetMin<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector) where TResult : struct
        {
            return GetMin<TResult>(selector, null);
        }

        /// <summary>
        /// Gets the min value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        /// The min value.
        /// </returns>
        public virtual TResult GetMin<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector, System.Linq.Expressions.Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            Checker.Requires(SupportedAggregationResultTypeNames.Contains(typeof(TResult).FullName), "not support the aggregation return type: {0}", typeof(TResult).FullName);
            Checker.Parameter(selector != null, "the selector can not be null");

            if (condition == null)
                return Session.QueryOver<TObject>().Select(Projections.Min(selector)).SingleOrDefault<TResult>();
            else
                return Session.QueryOver<TObject>().Select(Projections.Min(selector)).Where(condition).SingleOrDefault<TResult>();
        }

        /// <summary>
        /// Gets the max value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>The max value.</returns>
        public TResult GetMax<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector) where TResult : struct
        {
            return GetMax<TResult>(selector, null);
        }

        /// <summary>
        /// Gets the max value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        /// The max value.
        /// </returns>
        public virtual  TResult GetMax<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector, System.Linq.Expressions.Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            Checker.Requires(SupportedAggregationResultTypeNames.Contains(typeof(TResult).FullName), "not support the aggregation return type: {0}", typeof(TResult).FullName);
            Checker.Parameter(selector != null, "the selector can not be null");

            if (condition == null)
                return Session.QueryOver<TObject>().Select(Projections.Max(selector)).SingleOrDefault<TResult>();
            else
                return Session.QueryOver<TObject>().Select(Projections.Max(selector)).Where(condition).SingleOrDefault<TResult>();
        }

        /// <summary>
        /// Gets the sum value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>The sum value.</returns>
        public TResult GetSum<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector) where TResult : struct
        {
            return GetSum<TResult>(selector, null);
        }

        /// <summary>
        /// Gets the sum value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        /// The sum value.
        /// </returns>
        public virtual TResult GetSum<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector, System.Linq.Expressions.Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            Checker.Requires(SupportedAggregationResultTypeNames.Contains(typeof(TResult).FullName), "not support the aggregation return type: {0}", typeof(TResult).FullName);
            Checker.Parameter(selector != null, "the selector can not be null");

            if (condition == null)
                return Session.QueryOver<TObject>().Select(Projections.Sum(selector)).SingleOrDefault<TResult>();
            else
                return Session.QueryOver<TObject>().Select(Projections.Sum(selector)).Where(condition).SingleOrDefault<TResult>();
        }

        /// <summary>
        /// Gets the average value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>The average value.</returns>
        public TResult GetAverage<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector) where TResult : struct
        {
            return GetAverage<TResult>(selector, null);
        }

        /// <summary>
        /// Gets the average value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The where condition.</param>
        /// <returns>
        /// The average value.
        /// </returns>
        public virtual TResult GetAverage<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector, System.Linq.Expressions.Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            Checker.Requires(SupportedAggregationResultTypeNames.Contains(typeof(TResult).FullName), "not support the aggregation return type: {0}", typeof(TResult).FullName);
            Checker.Parameter(selector != null, "the selector can not be null");

            if (condition == null)
                return Session.QueryOver<TObject>().Select(Projections.Avg(selector)).SingleOrDefault<TResult>();
            else
                return Session.QueryOver<TObject>().Select(Projections.Avg(selector)).Where(condition).SingleOrDefault<TResult>();
        }
    }
}
