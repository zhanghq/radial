using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Data;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// NHibernate unit of work class.
    /// </summary>
    public class NhUnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;
        private IsolationLevel? _isolationLevel;
        private ITransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="NhUnitOfWork"/> class.
        /// </summary>
        public NhUnitOfWork()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NhUnitOfWork"/> class.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive, can be null or empty).</param>
        public NhUnitOfWork(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
                _session = HibernateEngine.OpenSession();
            else
                _session = SessionFactoryPool.OpenSession(alias);
        }

        /// <summary>
        /// Reset the transaction isolation level.
        /// </summary>
        /// <param name="level">The new isolation level.</param>
        public void ResetIsolationLevel(IsolationLevel? level = null)
        {
            _isolationLevel = level;
        }

        /// <summary>
        /// Prepares the transaction.
        /// </summary>
        protected virtual void PrepareTransaction()
        {
            if (_transaction == null && System.Transactions.Transaction.Current == null)
            {
                if (!_isolationLevel.HasValue)
                    _transaction = _session.BeginTransaction();
                else
                    _transaction = _session.BeginTransaction(_isolationLevel.Value);
            }
        }

        /// <summary>
        /// Gets the underlying data context object.
        /// </summary>
        public virtual object UnderlyingContext
        {
            get { return _session; }
        }

        /// <summary>
        /// Register object which will be inserted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public virtual void RegisterNew<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
            {
                PrepareTransaction();
                _session.Save(obj);
            }
        }

        /// <summary>
        /// Register object set which will be inserted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        public virtual void RegisterNew<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null && objs.Count() > 0)
            {
                PrepareTransaction();

                foreach (TObject obj in objs)
                    _session.Save(obj);

            }
        }

        /// <summary>
        /// Register object set which will be inserted or updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public virtual void RegisterSave<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
            {
                PrepareTransaction();
                _session.Save(obj);
            }
        }

        /// <summary>
        /// Register object set which will be inserted or updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        public virtual void RegisterSave<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null && objs.Count() > 0)
            {
                PrepareTransaction();

                foreach (TObject obj in objs)
                    _session.SaveOrUpdate(obj);
            }
        }

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public virtual void RegisterDelete<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
            {
                PrepareTransaction();
                _session.Delete(obj);
            }
        }

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object instance.</param>
        public void RegisterDelete<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null && objs.Count() > 0)
            {
                PrepareTransaction();

                foreach (TObject obj in objs)
                    _session.Delete(obj);
            }
        }

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <typeparam name="TKey">The type of object key.</typeparam>
        /// <param name="key">The object key.</param>
        public virtual void RegisterDelete<TObject, TKey>(TKey key) where TObject : class
        {
            var metadata = _session.SessionFactory.GetClassMetadata(typeof (TObject));

            Checker.Requires(metadata.HasIdentifierProperty, "{0} does not has identifier property",
                             typeof (TObject).FullName);

            string query = string.Format("from {0} o where o.{1}=?", typeof (TObject).Name,
                                         metadata.IdentifierPropertyName);

            PrepareTransaction();

            _session.Delete(query, key, metadata.IdentifierType);
        }

        /// <summary>
        /// Register delete all objects.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        public virtual void RegisterClear<TObject>() where TObject : class
        {
            PrepareTransaction();

            _session.Delete(string.Format("from {0}", typeof (TObject).Name));
        }

        /// <summary>
        /// Commit changes to data source.
        /// </summary>
        public void Commit()
        {
            if (_transaction != null && System.Transactions.Transaction.Current == null)
            {
                try
                {
                    _transaction.Commit();
                }
                catch
                {
                    _transaction.Rollback();
                    throw;
                }
                finally
                {
                    _transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
            if (_session != null)
                _session.Dispose();
        }

        /// <summary>
        /// Register object which will be updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public void RegisterUpdate<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
            {
                PrepareTransaction();
                _session.Update(obj);
            }
        }

        /// <summary>
        /// Register object set which will be updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        public void RegisterUpdate<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null && objs.Count() > 0)
            {
                PrepareTransaction();

                foreach (TObject obj in objs)
                    _session.Update(obj);
            }
        }
    }
}
