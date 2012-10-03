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

        IList<object> _pendingInsert;
        IList<object> _pendingUpdate;
        IList<object> _pendingDelete;
        IList<string> _pendingClearHql;
    

        /// <summary>
        /// Initializes a new instance of the <see cref="NhUnitOfWork"/> class.
        /// </summary>
        public NhUnitOfWork()
        {
            _session = HibernateEngine.OpenSession();

            _pendingInsert = new List<object>();
            _pendingUpdate = new List<object>();
            _pendingDelete = new List<object>();
            _pendingClearHql = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NhUnitOfWork"/> class.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive).</param>
        public NhUnitOfWork(string alias)
        {
            _session = SessionFactoryPool.OpenSession(alias);

            _pendingInsert = new List<object>();
            _pendingUpdate = new List<object>();
            _pendingDelete = new List<object>();
            _pendingClearHql = new List<string>();
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
        /// Register object which will be inserted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public virtual void RegisterNew<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
                _pendingInsert.Add(obj);
        }

        /// <summary>
        /// Register object set which will be inserted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        public virtual void RegisterNew<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null)
            {
                foreach (TObject obj in objs)
                {
                    if (obj != null)
                        _pendingInsert.Add(obj);
                }
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
                _pendingUpdate.Add(obj);
        }

        /// <summary>
        /// Register object set which will be updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        public void RegisterUpdate<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null)
            {
                foreach (TObject obj in objs)
                {
                    if (obj != null)
                        _pendingUpdate.Add(obj);
                }
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
                _pendingDelete.Add(obj);
        }

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object instance.</param>
        public void RegisterDelete<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null)
            {
                foreach (TObject obj in objs)
                {
                    if (obj != null)
                        _pendingDelete.Add(obj);
                }
            }
        }

        /// <summary>
        /// Register delete all objects.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        public virtual void RegisterClear<TObject>() where TObject : class
        {
            _pendingClearHql.Add(string.Format("from {0}", typeof(TObject).Name));
        }

        /// <summary>
        /// Commit changes to data source.
        /// </summary>
        /// <param name="autoGenerateTransaction">If need to use automatic transaction set to <c>true</c>, default is <c>false</c>.</param>
        public void Commit(bool autoGenerateTransaction = false)
        {
            if (autoGenerateTransaction)
            {
                ITransaction tx = _session.BeginTransaction();
                try
                {
                    PrepareCommand();
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
                finally
                {
                    tx.Dispose();
                }
            }
            else
                PrepareCommand();
        }

        /// <summary>
        /// Commit changes to data source with automatic transaction.
        /// </summary>
        /// <param name="isolationLevel">Isolation level for the new transaction.</param>
        public void Commit(IsolationLevel isolationLevel)
        {
            ITransaction tx = _session.BeginTransaction(isolationLevel);
            try
            {
                PrepareCommand();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
            finally
            {
                tx.Dispose();
            }
        }

        /// <summary>
        /// Prepare session Command
        /// </summary>
        private void PrepareCommand()
        {
            _pendingInsert.ToList().ForEach(o => _session.Save(o));
            _pendingInsert.Clear();
            _pendingUpdate.ToList().ForEach(o => _session.Update(o));
            _pendingUpdate.Clear();
            _pendingDelete.ToList().ForEach(o => _session.Delete(o));
            _pendingDelete.Clear();
            _pendingClearHql.ToList().ForEach(o => _session.Delete(o));
            _pendingClearHql.Clear();
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_session != null)
                _session.Dispose();
        }
    }
}
