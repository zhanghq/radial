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
        /// Register delete all objects.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        void RegisterClear<TObject>() where TObject : class;


        /// <summary>
        /// Commit changes to data source.
        /// </summary>
        /// <param name="autoGenerateTransaction">If need to use automatic transaction set to <c>true</c>, default is <c>false</c>.</param>
        void Commit(bool autoGenerateTransaction = false);

        /// <summary>
        /// Commit changes to data source with automatic transaction.
        /// </summary>
        /// <param name="isolationLevel">Isolation level for the new transaction.</param>
        void Commit(IsolationLevel isolationLevel);

    }
}
