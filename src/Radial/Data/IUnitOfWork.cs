using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Radial.Data
{
    /// <summary>
    /// Unit of work interface.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the data context object.
        /// </summary>
        object DataContext { get; }

        /// <summary>
        /// Gets the current transaction instance.
        /// </summary>
        object Transaction { get; }

        /// <summary>
        /// Register new object which will be inserted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        void RegisterNew<TObject>(TObject obj) where TObject : class;

        /// <summary>
        /// Register new object set which will be inserted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        void RegisterNew<TObject>(IEnumerable<TObject> objs) where TObject : class;

        /// <summary>
        /// Register object which will be updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        void RegisterUpdate<TObject>(TObject obj) where TObject : class;

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        void RegisterDelete<TObject>(TObject obj) where TObject : class;

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <typeparam name="TKey">The type of object key.</typeparam>
        /// <param name="key">The object key.</param>
        void RegisterDelete<TObject, TKey>(TKey key) where TObject : class;


        /// <summary>
        /// Register delete all objects.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        void RegisterClear<TObject>() where TObject : class;


        /// <summary>
        /// Begin a new transaction object.
        /// </summary>
        /// <returns>A transaction instance.</returns>
        void BeginTransaction();


        /// <summary>
        /// Begin a transaction with the specified <c>isolationLevel</c>
        /// </summary>
        /// <param name="isolationLevel">Isolation level for the new transaction.</param>
        /// <returns>
        /// A transaction instance having the specified isolation level.
        /// </returns>
        void BeginTransaction(IsolationLevel isolationLevel);


        /// <summary>
        /// Commit changes (if Transaction property is null, will use TransactionScope automatically).
        /// </summary>
        void Commit();

        /// <summary>
        /// Commit changes (if Transaction property is null, will use TransactionScope automatically). 
        /// </summary>
        /// <param name="option">The commit option.</param>
        void Commit(object option);

        /// <summary>
        /// Commit changes (if Transaction property is null, will use TransactionScope automatically).
        /// </summary>
        /// <typeparam name="TOption">The type of commit option.</typeparam>
        /// <param name="option">The commit option.</param>
        void Commit<TOption>(TOption option);

        /// <summary>
        /// Rollback current transaction instance.
        /// </summary>
        void Rollback();
    }
}
