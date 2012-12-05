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
        /// Determine whether contains objects that match the where condition.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <returns>
        ///   <c>true</c> if objects that match the where condition is exists; otherwise, <c>false</c>.
        /// </returns>
        bool Exist(Expression<Func<TObject, bool>> where);


        /// <summary>
        /// Gets objects total.
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
        /// <param name="where">The where condition.</param>
        /// <returns>
        /// If data exists, return the object, otherwise return null.
        /// </returns>
        TObject Find(Expression<Func<TObject, bool>> where);

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll();

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(OrderBySnippet<TObject>[] orderBys);

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <returns>If data exists, return an objects list, otherwise return an empty list.</returns>
        IList<TObject> FindAll(Expression<Func<TObject, bool>> where, params OrderBySnippet<TObject>[] orderBys);

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(int returnObjectCount);

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(OrderBySnippet<TObject>[] orderBys, int returnObjectCount);


        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(Expression<Func<TObject, bool>> where, int returnObjectCount);


        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="orderBys">The order by snippets.</param>
        /// <param name="returnObjectCount">The number of objects returned.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int returnObjectCount);


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
        /// <param name="where">The where condition</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(Expression<Func<TObject, bool>> where, int pageSize, int pageIndex);

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
        IList<TObject> FindAll(Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex);

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
        /// <param name="where">The where condition</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(Expression<Func<TObject, bool>> where, int pageSize, int pageIndex, out int objectTotal);

        /// <summary>
        /// Find all objects.
        /// </summary>
        /// <param name="where">The where condition</param>
        /// <param name="orderBys">The order by snippets</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The number of total objects.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        IList<TObject> FindAll(Expression<Func<TObject, bool>> where, OrderBySnippet<TObject>[] orderBys, int pageSize, int pageIndex, out int objectTotal);

        /// <summary>
        /// Adds an object to the repository (or delegate to RegisterNew method if using unit of work).
        /// </summary>
        /// <param name="obj">The object.</param>
        void Add(TObject obj);

        /// <summary>
        /// Adds objects to the repository (or delegate to RegisterNew method if using unit of work).
        /// </summary>
        /// <param name="objs">The objects.</param>
        void Add(IEnumerable<TObject> objs);

        /// <summary>
        /// Saves or updates the specified object (or delegate to RegisterSave method if using unit of work).
        /// </summary>
        /// <param name="obj">The object.</param>
        void Save(TObject obj);

        /// <summary>
        /// Removes an object with the specified key from the repository (or delegate to RegisterDelete method if using unit of work).
        /// </summary>
        /// <param name="key">The object key.</param>
        void Remove(TKey key);

        /// <summary>
        /// Removes the specified object from the repository (or delegate to RegisterDelete method if using unit of work).
        /// </summary>
        /// <param name="obj">The object.</param>
        void Remove(TObject obj);

        /// <summary>
        /// Clear all objects (or delegate to RegisterClear method if using unit of work).
        /// </summary>
        void Clear();
    }
}
