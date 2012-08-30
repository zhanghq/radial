﻿using System;
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
        IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicRepository&lt;TObject, TKey&gt;"/> class.
        /// </summary>
        /// <param name="uow">The IUnitOfWork instance.</param>
        public BasicRepository(IUnitOfWork uow)
        {
            Checker.Parameter(uow != null, "the IUnitOfWork instance can not be null");
            _uow = uow;
        }

        /// <summary>
        /// Gets the NHibernate session object.
        /// </summary>
        protected virtual ISession Session
        {
            get
            {
                return (ISession)_uow.DataContext;
            }
        }

        ///// <summary>
        ///// Saves or updates the specified object.
        ///// </summary>
        ///// <param name="obj">The object.</param>
        //public virtual void Save(TObject obj)
        //{
        //    if (obj != null)
        //        Session.SaveOrUpdate(obj);
        //}

        ///// <summary>
        ///// Removes an object with the specified key from the repository.
        ///// </summary>
        ///// <param name="key">The object key.</param>
        //public virtual void Remove(TKey key)
        //{
        //    var metadata = Session.SessionFactory.GetClassMetadata(typeof(TObject));

        //    Checker.Requires(metadata.HasIdentifierProperty, "{0} does not has identifier property", typeof(TObject).FullName);

        //    Session.Delete(string.Format("from {0} o where o.{1}=?", typeof(TObject).Name, metadata.IdentifierPropertyName), key, metadata.IdentifierType);
        //}

        ///// <summary>
        ///// Removes the specified object from the repository.
        ///// </summary>
        ///// <param name="obj">The object.</param>
        //public virtual void Remove(TObject obj)
        //{
        //    if (obj != null)
        //        Session.Delete(obj);
        //}

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

            IQuery query = Session.CreateQuery(string.Format("select count(*) from {0} o where o.{1}=:key", typeof(TObject).Name, metadata.IdentifierPropertyName));
            query.SetParameter("key", key);

            return Convert.ToInt32(query.UniqueResult()) > 0;
        }

        /// <summary>
        /// Determine whether the object is exists.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        ///   <c>true</c> if the object is exists; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Exist(System.Linq.Expressions.Expression<Func<TObject, bool>> where)
        {
            return GetTotal(where) > 0;
        }

        /// <summary>
        /// Determine whether the object is exists.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        ///   <c>true</c> if the object is exists; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool Exist(ICriterion where)
        {
            return GetTotal(where) > 0;
        }


        /// <summary>
        /// Counts objects total.
        /// </summary>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual int GetTotal()
        {
            return Session.QueryOver<TObject>().RowCount();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual int GetTotal(System.Linq.Expressions.Expression<Func<TObject, bool>> where)
        {
            if (where != null)
                return Session.QueryOver<TObject>().Where(where).RowCount();
            return Session.QueryOver<TObject>().RowCount();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        protected virtual int GetTotal(ICriterion where)
        {
            if (where != null)
                return Session.QueryOver<TObject>().Where(where).RowCount();
            return Session.QueryOver<TObject>().RowCount();
        }

        /// <summary>
        /// Counts objects total.
        /// </summary>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual long GetTotalInt64()
        {
            return Session.QueryOver<TObject>().RowCountInt64();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        public virtual long GetTotalInt64(System.Linq.Expressions.Expression<Func<TObject, bool>> where)
        {
            if (where != null)
                return Session.QueryOver<TObject>().Where(where).RowCountInt64();
            return Session.QueryOver<TObject>().RowCountInt64();
        }

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        protected virtual long GetTotalInt64(ICriterion where)
        {
            if (where != null)
                return Session.QueryOver<TObject>().Where(where).RowCountInt64();
            return Session.QueryOver<TObject>().RowCountInt64();
        }

        /// <summary>
        /// Get object with the specified key.
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <returns>If data exists, return the object, otherwise return null.</returns>
        public virtual TObject Get(TKey key)
        {
            return Session.Get<TObject>(key);
        }

        /// <summary>
        /// Get object.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        /// If data exists, return the object, otherwise return null.
        /// </returns>
        public virtual TObject Get(System.Linq.Expressions.Expression<Func<TObject, bool>> where)
        {
            Checker.Parameter(where != null, "where condition can not be null");
            return Session.QueryOver<TObject>().Where(where).SingleOrDefault();
        }

        /// <summary>
        /// Get object.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        /// If data exists, return the object, otherwise return null.
        /// </returns>
        protected virtual TObject Get(ICriterion where)
        {
            Checker.Parameter(where != null, "where condition can not be null");
            return Session.QueryOver<TObject>().Where(where).SingleOrDefault();
        }

        /// <summary>
        /// Gets the object with the specified key.
        /// </summary>
        public TObject this[TKey key]
        {
            get { return Get(key); }
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets()
        {
            return Gets(null, null);
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(OrderBySnippet<TObject>[] orderBys)
        {
            return Gets(null, orderBys);
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>If data exists, return an objects list, otherwise return an empty list.</returns>
        public virtual IList<TObject> Gets(System.Linq.Expressions.Expression<Func<TObject, bool>> where, params OrderBySnippet<TObject>[] orderBys)
        {
            IQueryOver<TObject, TObject> query = Session.QueryOver<TObject>();

            if (where != null)
                query = query.Where(where);

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
        /// Get all objects.
        /// </summary>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(int pageSize, int pageIndex, out int objectTotal)
        {
            return Gets(null, pageSize, pageIndex, out objectTotal);
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(System.Linq.Expressions.Expression<Func<TObject, bool>> where, int pageSize, int pageIndex, out int objectTotal)
        {
            return Gets(where, null, pageSize, pageIndex, out objectTotal);
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<TObject> Gets(System.Linq.Expressions.Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex, out int objectTotal)
        {
            IQueryOver<TObject, TObject> countQuery = Session.QueryOver<TObject>();
            IQueryOver<TObject, TObject> dataQuery = Session.QueryOver<TObject>();

            if (where != null)
            {
                countQuery = countQuery.Where(where);
                dataQuery = dataQuery.Where(where);
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

        ///// <summary>
        ///// Clear all objects.
        ///// </summary>
        //public virtual void Clear()
        //{
        //    Session.Delete(string.Format("from {0}", typeof(TObject).Name));
        //}


        ///// <summary>
        ///// Adds objects to the repository.
        ///// </summary>
        ///// <param name="objs">The objects.</param>
        //public virtual void Add(IEnumerable<TObject> objs)
        //{
        //    if (objs != null)
        //    {
        //        foreach (TObject obj in objs)
        //        {
        //            if (obj != null)
        //                Session.Save(obj);
        //        }
        //    }
        //}


        ///// <summary>
        ///// Adds an object to the repository.
        ///// </summary>
        ///// <param name="obj">The object.</param>
        //public virtual void Add(TObject obj)
        //{
        //    if (obj != null)
        //        Add(new TObject[] { obj });
        //}


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
            IQueryOver<TObject, TObject> query = Session.QueryOver<TObject>();

            if (where != null)
                query = query.Where(where);

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
            IQueryOver<TObject, TObject> query = Session.QueryOver<TObject>();

            if (where != null)
                query = query.Where(where);

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
        /// To unique item list.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>The list does not contains duplicate elements</returns>
        public virtual IList<TObject> ToUniqueList(IEnumerable<TObject> collection)
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
        /// Validates the paging parameter.
        /// </summary>
        /// <param name="pageSize">The page size.</param>
        /// <param name="pageIndex">The page index.</param>
        public virtual void ValidatePagingParameter(int pageSize, int pageIndex)
        {
            Checker.Parameter(pageSize >= 0, "pageSize must be greater than or equal to 0");
            Checker.Parameter(pageIndex >= 1, "pageIndex must be greater than or equal to 1");
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
            ValidatePagingParameter(pageSize, pageIndex);

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
            ValidatePagingParameter(pageSize, pageIndex);

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
            ValidatePagingParameter(pageSize, pageIndex);

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
            ValidatePagingParameter(pageSize, pageIndex);

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
            ValidatePagingParameter(pageSize, pageIndex);

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
            ValidatePagingParameter(pageSize, pageIndex);


            return dataQuery.Skip(pageSize * (pageIndex - 1)).Take(pageSize).List();
        }

        #endregion
    }
}
