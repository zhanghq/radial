using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Data;

namespace Radial.Data.Nhs
{
    /// <summary>
    /// NHibernate unit of work class.
    /// </summary>
    public class NhUnitOfWork : IUnitOfWork
    {
        ISession _session;
        ITransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="NhUnitOfWork"/> class.
        /// </summary>
        public NhUnitOfWork()
        {
            _session = HibernateEngine.OpenSession();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NhUnitOfWork"/> class.
        /// </summary>
        /// <param name="session">The NHibernate session object.</param>
        public NhUnitOfWork(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NhUnitOfWork"/> class.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive).</param>
        public NhUnitOfWork(string alias)
        {
            _session = SessionFactoryPool.OpenSession(alias);
        }

        /// <summary>
        /// Gets the data context object.
        /// </summary>
        public virtual object DataContext
        {
            get
            {
                return _session;
            }
        }


        /// <summary>
        /// Gets the current transaction instance.
        /// </summary>
        public virtual object Transaction
        {
            get
            {
                return _transaction;
            }
        }

        /// <summary>
        /// Register new object.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public virtual void RegisterNew<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
                _session.Save(obj);
        }

        /// <summary>
        /// Register new object set.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        public virtual void RegisterNew<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null)
            {
                foreach (TObject obj in objs)
                    if (obj != null)
                        _session.Save(obj);
            }
        }

        /// <summary>
        /// Register object which will be updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public virtual void RegisterUpdate<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
                _session.SaveOrUpdate(obj);
        }

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public virtual void RegisterDelete<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
                _session.Delete(obj);
        }

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <typeparam name="TKey">The type of object key.</typeparam>
        /// <param name="key">The object key.</param>
        public virtual void RegisterDelete<TObject, TKey>(TKey key) where TObject : class
        {
            var metadata = _session.SessionFactory.GetClassMetadata(typeof(TObject));

            Checker.Requires(metadata.HasIdentifierProperty, "{0} does not has identifier property", typeof(TObject).FullName);

            _session.Delete(string.Format("from {0} o where o.{1}=?", typeof(TObject).Name, metadata.IdentifierPropertyName), key, metadata.IdentifierType);
        }

        /// <summary>
        /// Register delete all objects.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        public virtual void RegisterClear<TObject>() where TObject : class
        {
            _session.Delete(string.Format("from {0}", typeof(TObject).Name));
        }

        /// <summary>
        /// Begin a new transaction object.
        /// </summary>
        /// <returns>A transaction instance.</returns>
        public virtual void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }


        /// <summary>
        /// Begin a transaction with the specified <c>isolationLevel</c>
        /// </summary>
        /// <param name="isolationLevel">Isolation level for the new transaction.</param>
        /// <returns>
        /// A transaction instance having the specified isolation level.
        /// </returns>
        public virtual void BeginTransaction(IsolationLevel isolationLevel)
        {
            _transaction = _session.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Commit changes (if Transaction property is null, will use TransactionScope automatically).
        /// </summary>
        public virtual void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                return;
            }

            AutoTransaction.Complete(() => _session.Flush());
        }

        /// <summary>
        /// Commit changes (if Transaction property is null, will use TransactionScope automatically). 
        /// </summary>
        /// <param name="option">The commit option.</param>
        public virtual void Commit(object option)
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                return;
            }

            AutoTransaction.Complete(() => _session.Flush());
        }

        /// <summary>
        /// Commit changes (if Transaction property is null, will use TransactionScope automatically).
        /// </summary>
        /// <typeparam name="TOption">The type of commit option.</typeparam>
        /// <param name="option">The commit option.</param>
        public virtual void Commit<TOption>(TOption option)
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                return;
            }

            AutoTransaction.Complete(() => _session.Flush());
        }

        /// <summary>
        /// Rollback current transaction instance.
        /// </summary>
        public void Rollback()
        {
            if (_transaction != null)
                _transaction.Rollback();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();

            _session.Dispose();
        }
    }
}
