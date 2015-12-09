using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Radial.Persist
{
    /// <summary>
    /// IRepository interface.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TKey">The type of the object key.</typeparam>
    public interface IRepository<TObject, TKey> where TObject : class
    {
        /// <summary>
        /// Determine whether the object is exists.
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <returns>
        ///   <c>true</c> if the object is exists; otherwise, <c>false</c>.
        /// </returns>
        bool Exist(TKey key);

        /// <summary>
        /// Determine whether contains objects that match The condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        ///   <c>true</c> if objects that match The condition. is exists; otherwise, <c>false</c>.
        /// </returns>
        bool Exist(Expression<Func<TObject, bool>> condition);


        /// <summary>
        /// Gets objects count.
        /// </summary>
        /// <returns>
        /// The objects count.
        /// </returns>
        int GetCount();

        /// <summary>
        /// Gets objects count using the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The objects count.
        /// </returns>
        int GetCount(Expression<Func<TObject, bool>> condition);

        /// <summary>
        /// Counts objects count.
        /// </summary>
        /// <returns>
        /// The objects count.
        /// </returns>
        long GetCountInt64();

        /// <summary>
        /// Gets objects count using the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The objects count.
        /// </returns>
        long GetCountInt64(Expression<Func<TObject, bool>> condition);

        /// <summary>
        /// Gets the min value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The min value.
        /// </returns>
        TResult GetMin<TResult>(Expression<Func<TObject, object>> selector) where TResult : struct;

        /// <summary>
        /// Gets the min value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The min value.
        /// </returns>
        TResult GetMin<TResult>(Expression<Func<TObject, object>> selector, Expression<Func<TObject, bool>> condition) where TResult : struct;

        /// <summary>
        /// Gets the max value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>The max value.</returns>
        TResult GetMax<TResult>(Expression<Func<TObject, object>> selector) where TResult : struct;

        /// <summary>
        /// Gets the max value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The max value.
        /// </returns>
        TResult GetMax<TResult>(Expression<Func<TObject, object>> selector, Expression<Func<TObject, bool>> condition) where TResult : struct;

        /// <summary>
        /// Gets the sum value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>The sum value.</returns>
        TResult GetSum<TResult>(Expression<Func<TObject, object>> selector) where TResult : struct;

        /// <summary>
        /// Gets the sum value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The sum value.
        /// </returns>
        TResult GetSum<TResult>(Expression<Func<TObject, object>> selector, Expression<Func<TObject, bool>> condition) where TResult : struct;

        /// <summary>
        /// Gets the average value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>The average value.</returns>
        TResult GetAverage<TResult>(Expression<Func<TObject, object>> selector) where TResult : struct;

        /// <summary>
        /// Gets the average value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// The average value.
        /// </returns>
        TResult GetAverage<TResult>(Expression<Func<TObject, object>> selector, Expression<Func<TObject, bool>> condition) where TResult : struct;

        /// <summary>
        /// Find object with the specified key.
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <returns>If data exists, return the object, otherwise return null.</returns>
        TObject Find(TKey key);

        /// <summary>
        /// Find the object with the specified key.
        /// </summary>
        TObject this[TKey key] { get; }

        /// <summary>
        /// Find object.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>
        /// If data exists, return the object, otherwise return null.
        /// </returns>
        TObject Find(Expression<Func<TObject, bool>> condition);

        /// <summary>
        /// Find the first object.
        /// </summary>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>If data exists, return the first object, otherwise return null.</returns>
        TObject FindFirst(params OrderBySnippet<TObject>[] orderBys);

        /// <summary>
        /// Find the first object.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>If data exists, return the first object, otherwise return null.</returns>
        TObject FindFirst(Expression<Func<TObject, bool>> condition, params OrderBySnippet<TObject>[] orderBys);

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <returns>
        /// <param name="orderBys">The order by snippets</param>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(params OrderBySnippet<TObject>[] orderBys);

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>If data exists, return an objects list, otherwise return an empty list.</returns>
        IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, params OrderBySnippet<TObject>[] orderBys);

        ///// <summary>
        ///// Find all objects.
        ///// </summary>
        ///// <param name="returnObjectCount">The number of objects returned.</param>
        ///// <returns>
        ///// If data exists, return an objects list, otherwise return an empty list.
        ///// </returns>
        //IList<TObject> FindAll(int returnObjectCount);

        ///// <summary>
        ///// Find all objects.
        ///// </summary>
        ///// <param name="orderBys">The order by snippets.</param>
        ///// <param name="returnObjectCount">The number of objects returned.</param>
        ///// <returns>
        ///// If data exists, return an objects list, otherwise return an empty list.
        ///// </returns>
        //IList<TObject> FindAll(IEnumerable<OrderBySnippet<TObject>> orderBys, int returnObjectCount);


        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, int returnObjectCount);


        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, IEnumerable<OrderBySnippet<TObject>> orderBys, int returnObjectCount);


        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(int pageSize, int pageIndex);

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, int pageSize, int pageIndex);

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
        IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, IEnumerable<OrderBySnippet<TObject>> orderBys, int pageSize, int pageIndex);

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(int pageSize, int pageIndex, out int objectTotal);

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
        IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, int pageSize, int pageIndex, out int objectTotal);

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
        IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, IEnumerable<OrderBySnippet<TObject>> orderBys, int pageSize, int pageIndex, out int objectTotal);

        /// <summary>
        /// Find all objects by keys.
        /// </summary>
        /// <param name="keys">The object keys.</param>
        /// <param name="orderBys">The order by snippets.</param>
        /// <returns>
        /// If data exists and keys not empty, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindByKeys(IEnumerable<TKey> keys, params OrderBySnippet<TObject>[] orderBys);


        /// <summary>
        /// Find all object keys.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>
        /// If data exists, return an array, otherwise return an empty array.
        /// </returns>
        TKey[] FindKeys(Expression<Func<TObject, bool>> condition, params OrderBySnippet<TObject>[] orderBys);

        /// <summary>
        /// Add an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        void Add(TObject obj);

        /// <summary>
        /// Add objects.
        /// </summary>
        /// <param name="objs">The objects.</param>
        void Add(IEnumerable<TObject> objs);

        /// <summary>
        /// Add or update an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        void Save(TObject obj);

        /// <summary>
        /// Update an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        void Update(TObject obj);

        /// <summary>
        /// Remove an object with the specified key.
        /// </summary>
        /// <param name="key">The object key.</param>
        void Remove(TKey key);

        /// <summary>
        /// Remove an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        void Remove(TObject obj);

        /// <summary>
        /// Remove object with the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        void Remove(Expression<Func<TObject, bool>> condition);

        /// <summary>
        /// Clear all objects.
        /// </summary>
        void Clear();
    }
}
