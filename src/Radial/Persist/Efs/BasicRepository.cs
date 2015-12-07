using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Efs
{
    /// <summary>
    /// Basic class of IRepository
    /// </summary>
    /// <typeparam name="TObject">The type of persistent object.</typeparam>
    /// <typeparam name="TKey">The type of the object key.</typeparam>
    public abstract class BasicRepository<TObject, TKey> :
        IRepository<TObject, TKey> where TObject : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicRepository&lt;TObject, TKey&gt;"/> class.
        /// </summary>
        /// <param name="uow">The IUnitOfWorkEssential instance.</param>
        public BasicRepository(IUnitOfWorkEssential uow)
        {
            Checker.Parameter(uow != null, "the IUnitOfWorkEssential instance can not be null");
            UnitOfWork = uow;
        }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        protected IUnitOfWorkEssential UnitOfWork
        {
            get;
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        protected virtual DbContext DbContext
        {
            get
            {
                return (DbContext)UnitOfWork.UnderlyingContext;
            }
        }

        /// <summary>
        /// Find the object with the specified key.
        /// </summary>
        public TObject this[TKey key]
        {
            get
            {
                return Find(key);
            }
        }

        /// <summary>
        /// Add an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Add(TObject obj)
        {
            UnitOfWork.RegisterNew<TObject>(obj);
        }

        /// <summary>
        /// Add objects.
        /// </summary>
        /// <param name="objs">The objects.</param>
        public virtual void Add(IEnumerable<TObject> objs)
        {
            UnitOfWork.RegisterNew<TObject>(objs);
        }

        /// <summary>
        /// Add or update an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Save(TObject obj)
        {
            UnitOfWork.RegisterSave<TObject>(obj);
        }

        /// <summary>
        /// Remove an object with the specified key.
        /// </summary>
        /// <param name="key">The object key.</param>
        public virtual void Remove(TKey key)
        {
            UnitOfWork.RegisterDelete<TObject, TKey>(key);
        }

        /// <summary>
        /// Remove an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Remove(TObject obj)
        {
            UnitOfWork.RegisterDelete<TObject>(obj);
        }

        /// <summary>
        /// Clear all objects.
        /// </summary>
        public virtual void Clear()
        {
            UnitOfWork.RegisterClear<TObject>();
        }

        /// <summary>
        /// Updates an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public virtual void Update(TObject obj)
        {
            UnitOfWork.RegisterUpdate<TObject>(obj);
        }

        /// <summary>
        /// Remove object with the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        public virtual void Remove(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)
        {
            foreach (TObject o in FindAll(condition))
                Remove(o);
        }

        /// <summary>
        /// Exists the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public virtual bool Exist(Expression<Func<TObject, bool>> condition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Exists the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual bool Exist(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the specified condition.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public virtual TObject Find(Expression<Func<TObject, bool>> condition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual TObject Find(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="orderBys">The order bys.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(params OrderBySnippet<TObject>[] orderBys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="returnObjectCount">The return object count.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, int returnObjectCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, params OrderBySnippet<TObject>[] orderBys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(int pageSize, int pageIndex, out int objectTotal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <param name="returnObjectCount">The return object count.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, IEnumerable<OrderBySnippet<TObject>> orderBys, int returnObjectCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, int pageSize, int pageIndex, out int objectTotal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, IEnumerable<OrderBySnippet<TObject>> orderBys, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindAll(Expression<Func<TObject, bool>> condition, IEnumerable<OrderBySnippet<TObject>> orderBys, int pageSize, int pageIndex, out int objectTotal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the by keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <returns></returns>
        public virtual IList<TObject> FindByKeys(IEnumerable<TKey> keys, params OrderBySnippet<TObject>[] orderBys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the first.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <returns></returns>
        public TObject FindFirst(Expression<Func<TObject, bool>> condition, params OrderBySnippet<TObject>[] orderBys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the keys.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <returns></returns>
        public virtual TKey[] FindKeys(Expression<Func<TObject, bool>> condition, params OrderBySnippet<TObject>[] orderBys)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the average.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public TResult GetAverage<TResult>(Expression<Func<TObject, object>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the average.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public virtual TResult GetAverage<TResult>(Expression<Func<TObject, object>> selector, Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        public virtual int GetCount()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public virtual int GetCount(Expression<Func<TObject, bool>> condition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the count int64.
        /// </summary>
        /// <returns></returns>
        public virtual long GetCountInt64()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the count int64.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public virtual long GetCountInt64(Expression<Func<TObject, bool>> condition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the maximum.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public TResult GetMax<TResult>(Expression<Func<TObject, object>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the maximum.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public virtual TResult GetMax<TResult>(Expression<Func<TObject, object>> selector, Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the minimum.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public TResult GetMin<TResult>(Expression<Func<TObject, object>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the minimum.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public virtual TResult GetMin<TResult>(Expression<Func<TObject, object>> selector, Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the sum.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public TResult GetSum<TResult>(Expression<Func<TObject, object>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the sum.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public virtual TResult GetSum<TResult>(Expression<Func<TObject, object>> selector, Expression<Func<TObject, bool>> condition) where TResult : struct
        {
            throw new NotImplementedException();
        }
    }
}
