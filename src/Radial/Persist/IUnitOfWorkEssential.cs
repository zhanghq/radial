using System;
using System.Collections.Generic;
using System.Data;

namespace Radial.Persist
{
    /// <summary>
    /// Represents basic methods of the unit of work.
    /// </summary>
    public interface IUnitOfWorkEssential : IDisposable
    {
        /// <summary>
        /// Gets the underlying data context object.
        /// </summary>
        object UnderlyingContext { get; }

        /// <summary>
        /// Prepares the transaction, typically this method is invoked implicitly, but it  also can be explicit used to implement custom control.
        /// </summary>
        /// <param name="level">The isolation level.</param>
        void PrepareTransaction(IsolationLevel? level = null);

        /// <summary>
        /// Register object which will be inserted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        void RegisterNew<TObject>(TObject obj) where TObject : class;

        /// <summary>
        /// Register object set which will be inserted.
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
        /// Register object set which will be updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        void RegisterUpdate<TObject>(IEnumerable<TObject> objs) where TObject : class;

        /// <summary>
        /// Register object set which will be inserted or updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        void RegisterSave<TObject>(TObject obj) where TObject : class;

        /// <summary>
        /// Register object set which will be inserted or updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        void RegisterSave<TObject>(IEnumerable<TObject> objs) where TObject : class;

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        void RegisterDelete<TObject>(TObject obj) where TObject : class;

        /// <summary>
        /// Register object set which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        void RegisterDelete<TObject>(IEnumerable<TObject> objs) where TObject : class;

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
    }
}
