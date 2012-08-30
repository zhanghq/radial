using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Radial.Data
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
        /// Determine whether the object is exists.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        ///   <c>true</c> if the object is exists; otherwise, <c>false</c>.
        /// </returns>
        bool Exist(Expression<Func<TObject, bool>> where);


        /// <summary>
        /// Counts objects total.
        /// </summary>
        /// <returns>
        /// The objects total.
        /// </returns>
        int GetTotal();

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        int GetTotal(Expression<Func<TObject, bool>> where);

        /// <summary>
        /// Counts objects total.
        /// </summary>
        /// <returns>
        /// The objects total.
        /// </returns>
        long GetTotalInt64();

        /// <summary>
        /// Gets objects total using the specified condition.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        /// The objects total.
        /// </returns>
        long GetTotalInt64(Expression<Func<TObject, bool>> where);

        /// <summary>
        /// Get object with the specified key.
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <returns>If data exists, return the object, otherwise return null.</returns>
        TObject Get(TKey key);

        /// <summary>
        /// Gets the object with the specified key.
        /// </summary>
        TObject this[TKey key] { get; }

        /// <summary>
        /// Get object.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        /// If data exists, return the object, otherwise return null.
        /// </returns>
        TObject Get(Expression<Func<TObject, bool>> where);

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> Gets();

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> Gets(OrderBySnippet<TObject>[] orderBys);

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>If data exists, return an objects list, otherwise return an empty list.</returns>
        IList<TObject> Gets(Expression<Func<TObject, bool>> where, params OrderBySnippet<TObject>[] orderBys);

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> Gets(int returnObjectCount);

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> Gets(OrderBySnippet<TObject>[] orderBys, int returnObjectCount);


        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> Gets(Expression<Func<TObject, bool>> where, int returnObjectCount);


        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> Gets(Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int returnObjectCount);


        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> Gets(int pageSize, int pageIndex);

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> Gets(Expression<Func<TObject, bool>> where, int pageSize, int pageIndex);

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
        IList<TObject> Gets(Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex);

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> Gets(int pageSize, int pageIndex, out int objectTotal);

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
        IList<TObject> Gets(Expression<Func<TObject, bool>> where, int pageSize, int pageIndex, out int objectTotal);

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
        IList<TObject> Gets(Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex, out int objectTotal);
    }
}
