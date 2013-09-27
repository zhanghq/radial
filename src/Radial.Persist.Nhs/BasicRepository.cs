using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using System.Collections;
using System.Data.Common;
using System.Data;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Basic class of IRepository
    /// </summary>
    /// <typeparam name="TObject">The type of persistent object.</typeparam>
    /// <typeparam name="TKey">The type of the object key.</typeparam>
    public abstract class BasicRepository<TObject, TKey> : IRepository<TObject, TKey> where TObject : class
    {
        IUnitOfWorkEssential _uow;

        static string[] SupportedAggregationResultTypeNames = new string[] { typeof(short).FullName, typeof(ushort).FullName, typeof(int).FullName, typeof(uint).FullName, typeof(long).FullName, typeof(ulong).FullName, typeof(decimal).FullName, typeof(float).FullName, typeof(double).FullName };

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicRepository&lt;TObject, TKey&gt;"/> class.
        /// </summary>
        /// <param name="uow">The IUnitOfWorkEssential instance.</param>
        public BasicRepository(IUnitOfWorkEssential uow)
        {
            Checker.Parameter(uow != null, "the IUnitOfWorkEssential instance can not be null");
            _uow = uow;
            SetDefaultOrderBys(null);
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
        /// Gets the default order by snippets.
        /// </summary>
        protected IEnumerable<OrderBySnippet<TObject>> DefaultOrderBys { get; private set; }

        /// <summary>
        /// Gets the extra condition which will be used in every default query (but not apply to multi-query, hql and your own query).
        /// </summary>
        protected System.Linq.Expressions.Expression<Func<TObject, bool>> ExtraCondition { get; private set; }

        /// <summary>
        /// Sets the default order by snippets.
        /// </summary>
        /// <param name="orderBys">The order by snippets.</param>
        protected void SetDefaultOrderBys(params OrderBySnippet<TObject>[] orderBys)
        {
            if (orderBys != null && orderBys.Count() > 0)
                DefaultOrderBys = orderBys;
            else
                DefaultOrderBys = new List<OrderBySnippet<TObject>>();
        }

        /// <summary>
        /// Sets the extra condition which will be used in every default query (but not apply to multi-query, hql and your own query).
        /// </summary>
        /// <param name="condition">The condition.</param>
        protected void SetExtraCondition(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            ExtraCondition = condition;
        }

        /// <summary>
        /// Builds the query over.
        /// </summary>
        /// <param name="withExtraCondition">if set to <c>true</c> [with extra condition].</param>
        /// <returns></returns>
        protected IQueryOver<TObject, TObject> BuildQueryOver(bool withExtraCondition = true)
        {
            if (withExtraCondition && ExtraCondition != null)
                return Session.QueryOver<TObject>().Where(ExtraCondition);

            return Session.QueryOver<TObject>();
        }

        /// <summary>
        /// Appends custom order bys, if there is no custom value use default order bys instead.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="orderBys">The custom order bys.</param>
        /// <param name="withDefaultOrderBys">if set to <c>true</c> [with default order bys].</param>
        /// <returns></returns>
        protected IQueryOver<TObject, TObject> AppendOrderBys(IQueryOver<TObject, TObject> query, IEnumerable<OrderBySnippet<TObject>> orderBys = null, bool withDefaultOrderBys = true)
        {
            Checker.Requires(query != null, "query can not be null");

            if (withDefaultOrderBys && (orderBys == null || orderBys.Count() == 0))
                orderBys = DefaultOrderBys.ToArray();

            if (orderBys != null)
            {
                foreach (var order in orderBys)
                {
                    if (order.IsAscending)
                        query = query.OrderBy(order.Property).Asc;
                    else
                        query = query.OrderBy(order.Property).Desc;
                }
            }

            return query;
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

            return BuildQueryOver().Where(Expression.Eq(metadata.IdentifierPropertyName, key)).RowCount() > 0;
        }

        /// <summary>
        /// Determine whether contains objects that match The condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        ///   <c>true</c> if objects that match The condition. is exists; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Exist(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            return GetCount(condition) > 0;
        }

        /// <summary>
        /// Determine whether contains objects that match The condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        ///   <c>true</c> if objects that match The condition. is exists; otherwise, <c>false</c>.
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
            return BuildQueryOver().RowCount();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual int GetCount(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            if (condition != null)
                return BuildQueryOver().Where(condition).RowCount();
            return BuildQueryOver().RowCount();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        protected virtual int GetCount(ICriterion condition)
        {
            if (condition != null)
                return BuildQueryOver().Where(condition).RowCount();
            return BuildQueryOver().RowCount();
        }

        /// <summary>
        /// Counts objects total.
        /// </summary>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual long GetCountInt64()
        {
            return BuildQueryOver().RowCountInt64();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual long GetCountInt64(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            if (condition != null)
                return BuildQueryOver().Where(condition).RowCountInt64();
            return BuildQueryOver().RowCountInt64();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        protected virtual long GetCountInt64(ICriterion condition)
        {
            if (condition != null)
                return BuildQueryOver().Where(condition).RowCountInt64();
            return BuildQueryOver().RowCountInt64();
        }

        /// <summary>
        /// Find object with the specified key.
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <returns>If data exists, return the object, otherwise return null.</returns>
        public virtual TObject Find(TKey key)
        {
            if (ExtraCondition != null)
            {
                var metadata = Session.SessionFactory.GetClassMetadata(typeof(TObject));

                Checker.Requires(metadata.HasIdentifierProperty, "{0} does not has identifier property", typeof(TObject).FullName);

                return BuildQueryOver().Where(Expression.Eq(metadata.IdentifierPropertyName, key)).SingleOrDefault();
            }

            return Session.Get<TObject>(key);
        }

        /// <summary>
        /// Find object.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// If data exists, return the object, otherwise return null.
        /// </returns>
        public virtual TObject Find(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            Checker.Parameter(condition != null, "where condition can not be null");
            return BuildQueryOver().Where(condition).SingleOrDefault();
        }

        /// <summary>
        /// Find object.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// If data exists, return the object, otherwise return null.
        /// </returns>
        protected virtual TObject Find(ICriterion condition)
        {
            Checker.Parameter(condition != null, "where condition can not be null");
            return BuildQueryOver().Where(condition).SingleOrDefault();
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
        /// <param name="orderBys">The order by snippets.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(params OrderBySnippet<TObject>[] orderBys)
        {
            return FindAll(null, orderBys);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>If data exists, return an objects list, otherwise return an empty list.</returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, params OrderBySnippet<TObject>[] orderBys)
        {
            IQueryOver<TObject, TObject> query = BuildQueryOver();

            if (condition != null)
                query = query.Where(condition);

            return AppendOrderBys(query, orderBys).List();
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
        /// <param name="condition">The condition.</param>
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
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, IEnumerable<OrderBySnippet<TObject>> orderBys, int pageSize, int pageIndex, out int objectTotal)
        {
            IQueryOver<TObject, TObject> query = BuildQueryOver();

            if (condition != null)
                query = query.Where(condition);


            return ExecutePagingQuery(AppendOrderBys(query, orderBys), pageSize, pageIndex, out objectTotal);
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
        public virtual IList<TObject> FindAll(IEnumerable<OrderBySnippet<TObject>> orderBys, int returnObjectCount)
        {
            return FindAll(null, orderBys, returnObjectCount);
        }

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The condition.</param>
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
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, IEnumerable<OrderBySnippet<TObject>> orderBys, int returnObjectCount)
        {
            IQueryOver<TObject, TObject> query = BuildQueryOver();

            if (condition != null)
                query = query.Where(condition);


            return AppendOrderBys(query, orderBys).Take(returnObjectCount).List();
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
        /// <param name="condition">The condition.</param>
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
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindAll(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, IEnumerable<OrderBySnippet<TObject>> orderBys, int pageSize, int pageIndex)
        {
            IQueryOver<TObject, TObject> query = BuildQueryOver();

            if (condition != null)
                query = query.Where(condition);


            return ExecutePagingQuery(AppendOrderBys(query, orderBys), pageSize, pageIndex);
        }

        /// <summary>
        /// Add an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Add(TObject obj)
        {
            _uow.RegisterNew<TObject>(obj);
        }

        /// <summary>
        /// Add objects.
        /// </summary>
        /// <param name="objs">The objects.</param>
        public virtual void Add(IEnumerable<TObject> objs)
        {
            _uow.RegisterNew<TObject>(objs);
        }

        /// <summary>
        /// Save an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Save(TObject obj)
        {
            _uow.RegisterSave<TObject>(obj);
        }

        /// <summary>
        /// Remove an object with the specified key.
        /// </summary>
        /// <param name="key">The object key.</param>
        public virtual void Remove(TKey key)
        {
            _uow.RegisterDelete<TObject, TKey>(key);
        }

        /// <summary>
        /// Remove an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Remove(TObject obj)
        {
            _uow.RegisterDelete<TObject>(obj);
        }

        /// <summary>
        /// Clear all objects.
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
        protected virtual IMultiQuery CreateMultiQuery(IEnumerable<string> hqls)
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
        protected virtual IMultiCriteria CreateMultiQuery(IEnumerable<ICriteria> criterias)
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
        protected virtual IMultiQuery CreateMultiQuery(IEnumerable<IQuery> queries)
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
        protected virtual IMultiQuery CreateMultiQuery(IEnumerable<IQueryOver<TObject>> queries)
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
        protected virtual IMultiQuery CreateMultiQuery(IEnumerable<KeyValuePair<string, string>> hqls)
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
        protected virtual IMultiCriteria CreateMultiQuery(IEnumerable<KeyValuePair<string, ICriteria>> criterias)
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
        protected virtual IMultiQuery CreateMultiQuery(IEnumerable<KeyValuePair<string, IQuery>> queries)
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
        /// <param name="countQuery">The count query.</param>
        /// <param name="dataQuery">The query.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// The query result list
        /// </returns>
        protected virtual IList<TObject> ExecutePagingQuery(IQuery countQuery,IQuery dataQuery, int pageSize, int pageIndex, out int objectTotal)
        {
            Checker.Parameter(countQuery != null, "count query can not be null");
            Checker.Parameter(dataQuery != null, "data query can not be null");

            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);

            IMultiQuery mquery = Session.CreateMultiQuery();
            mquery.Add<int>(countQuery);
            mquery.Add<TObject>(dataQuery.SetFirstResult(pageSize * (pageIndex - 1)).SetMaxResults(pageSize));

            IList resultList = mquery.List();

            objectTotal = ((IList<int>)resultList[0])[0];

            return (IList<TObject>)resultList[1];
        }

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// The query result list
        /// </returns>
        protected virtual IList<TObject> ExecutePagingQuery(ICriteria criteria, int pageSize, int pageIndex, out int objectTotal)
        {
            Checker.Parameter(criteria != null, "criteria can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);

            ICriteria countCriteria = criteria.Clone() as ICriteria;
            countCriteria.ClearOrders();

            IMultiCriteria mcriteria = Session.CreateMultiCriteria();
            mcriteria.Add<int>(countCriteria.SetProjection(Projections.RowCount()));
            mcriteria.Add<TObject>(criteria.SetFirstResult(pageSize * (pageIndex - 1)).SetMaxResults(pageSize));

            IList resultList = mcriteria.List();

            objectTotal = ((IList<int>)resultList[0])[0];

            return (IList<TObject>)resultList[1];
        }

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// The query result list
        /// </returns>
        protected virtual IList<TObject> ExecutePagingQuery(IQueryOver<TObject> query, int pageSize, int pageIndex, out int objectTotal)
        {
            Checker.Parameter(query != null, "query can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);

            IQueryOver<TObject> countQuery = query.Clone();
            countQuery.ClearOrders();

            IMultiCriteria mcriteria = Session.CreateMultiCriteria();
            mcriteria.Add<int>(countQuery.ToRowCountQuery());
            mcriteria.Add<TObject>(query.Skip(pageSize * (pageIndex - 1)).Take(pageSize));

            IList resultList = mcriteria.List();

            objectTotal = ((IList<int>)resultList[0])[0];

            return (IList<TObject>)resultList[1];
        }

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="query">The data query.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>The query result list</returns>
        protected virtual IList<TObject> ExecutePagingQuery(IQuery query, int pageSize, int pageIndex)
        {
            Checker.Parameter(query != null, "query can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);

            return query.SetFirstResult(pageSize * (pageIndex - 1)).SetMaxResults(pageSize).List<TObject>();
        }

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="criteria">The data criteria.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>The query result list</returns>
        protected virtual IList<TObject> ExecutePagingQuery(ICriteria criteria, int pageSize, int pageIndex)
        {
            Checker.Parameter(criteria != null, "criteria can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);

            return criteria.SetFirstResult(pageSize * (pageIndex - 1)).SetMaxResults(pageSize).List<TObject>();
        }

        /// <summary>
        /// Execute the paging query.
        /// </summary>
        /// <param name="query">The data query.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>The query result list</returns>
        protected virtual IList<TObject> ExecutePagingQuery(IQueryOver<TObject> query, int pageSize, int pageIndex)
        {
            Checker.Parameter(query != null, "query can not be null");
            pageSize = NormalizePageSize(pageSize);
            pageIndex = NormalizePageIndex(pageIndex);


            return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).List();
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
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The min value.
        /// </returns>
        public virtual TResult GetMin<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector, System.Linq.Expressions.Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            Checker.Requires(SupportedAggregationResultTypeNames.Contains(typeof(TResult).FullName), "not support the aggregation return type: {0}", typeof(TResult).FullName);
            Checker.Parameter(selector != null, "the selector can not be null");

            if (condition == null)
                return BuildQueryOver().Select(Projections.Min(selector)).SingleOrDefault<TResult>();
            else
                return BuildQueryOver().Select(Projections.Min(selector)).Where(condition).SingleOrDefault<TResult>();
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
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The max value.
        /// </returns>
        public virtual TResult GetMax<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector, System.Linq.Expressions.Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            Checker.Requires(SupportedAggregationResultTypeNames.Contains(typeof(TResult).FullName), "not support the aggregation return type: {0}", typeof(TResult).FullName);
            Checker.Parameter(selector != null, "the selector can not be null");

            if (condition == null)
                return BuildQueryOver().Select(Projections.Max(selector)).SingleOrDefault<TResult>();
            else
                return BuildQueryOver().Select(Projections.Max(selector)).Where(condition).SingleOrDefault<TResult>();
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
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The sum value.
        /// </returns>
        public virtual TResult GetSum<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector, System.Linq.Expressions.Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            Checker.Requires(SupportedAggregationResultTypeNames.Contains(typeof(TResult).FullName), "not support the aggregation return type: {0}", typeof(TResult).FullName);
            Checker.Parameter(selector != null, "the selector can not be null");

            if (condition == null)
                return BuildQueryOver().Select(Projections.Sum(selector)).SingleOrDefault<TResult>();
            else
                return BuildQueryOver().Select(Projections.Sum(selector)).Where(condition).SingleOrDefault<TResult>();
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
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The average value.
        /// </returns>
        public virtual TResult GetAverage<TResult>(System.Linq.Expressions.Expression<Func<TObject, object>> selector, System.Linq.Expressions.Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            Checker.Requires(SupportedAggregationResultTypeNames.Contains(typeof(TResult).FullName), "not support the aggregation return type: {0}", typeof(TResult).FullName);
            Checker.Parameter(selector != null, "the selector can not be null");

            if (condition == null)
                return BuildQueryOver().Select(Projections.Avg(selector)).SingleOrDefault<TResult>();
            else
                return BuildQueryOver().Select(Projections.Avg(selector)).Where(condition).SingleOrDefault<TResult>();
        }

        /// <summary>
        /// Find the first object.
        /// </summary>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>If data exists, return the first object, otherwise return null.</returns>
        public TObject FindFirst(params OrderBySnippet<TObject>[] orderBys)
        {
            return FindAll(orderBys, 1).FirstOrDefault();
        }

        /// <summary>
        /// Find the first object.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>If data exists, return the first object, otherwise return null.</returns>
        public TObject FindFirst(System.Linq.Expressions.Expression<Func<TObject, bool>> condition, params OrderBySnippet<TObject>[] orderBys)
        {
            return FindAll(condition, orderBys, 1).FirstOrDefault();
        }


        /// <summary>
        /// Remove object with the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        public void Remove(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            foreach (TObject o in FindAll(condition))
                Remove(o);
        }

        /// <summary>
        /// Find all objects by keys.
        /// </summary>
        /// <param name="keys">The object keys.</param>
        /// <param name="orderBys">The order by snippets.</param>
        /// <returns>
        /// If data exists and keys not empty, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> FindByKeys(IEnumerable<TKey> keys, params OrderBySnippet<TObject>[] orderBys)
        {
            if (keys == null || keys.Count() == 0)
                return new List<TObject>();

            var metadata = Session.SessionFactory.GetClassMetadata(typeof(TObject));

            Checker.Requires(metadata.HasIdentifierProperty, "{0} does not has identifier property", typeof(TObject).FullName);

            var query = BuildQueryOver().Where(Expression.InG<TKey>(metadata.IdentifierPropertyName, keys));

            return AppendOrderBys(query, orderBys, false).List();
        }


        #region Stored Procedure

        /// <summary>
        /// Stored procedure ExecuteNonQuery.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>The number of rows affected.</returns>
        protected int SpExecuteNonQuery(string spName, params DbParameter[] parameters)
        {
            var cmd = SpCreateCommand(spName, parameters);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Stored procedure ExecuteScalar.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>The first column of the first result row.</returns>
        protected object SpExecuteScalar(string spName, params DbParameter[] parameters)
        {
            var cmd = SpCreateCommand(spName, parameters);

            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Stored procedure ExecuteReader.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>System.Data.IDataReader instance.</returns>
        protected IDataReader SpExecuteReader(string spName, params DbParameter[] parameters)
        {
            var cmd = SpCreateCommand(spName, parameters);

            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Stored procedure ExecuteDataTable.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>System.Data.DataTable instance.</returns>
        protected DataTable SpExecuteDataTable(string spName, params DbParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (var reader = SpExecuteReader(spName, parameters))
            {
                dt.Load(reader);
            }

            return dt;
        }

        /// <summary>
        /// Create System.Data.IDbCommand instance for stored procedure.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>
        /// System.Data.IDbCommand instance.
        /// </returns>
        private IDbCommand SpCreateCommand(string spName, params DbParameter[] parameters)
        {
            var cmd = Session.Connection.CreateCommand();

            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    if (p.Direction == ParameterDirection.InputOutput && p.Value == null)
                        p.Value = DBNull.Value;

                    cmd.Parameters.Add(p);
                }
            }

            return cmd;
        }

        #endregion
    }
}
