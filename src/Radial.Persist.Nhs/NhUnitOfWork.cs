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
        ISession _session;

        IList<object> _pendingInsert;
        IList<object> _pendingSave;
        IList<object> _pendingDelete;
        IList<dynamic> _pendingDeleteByKey;
        IList<string> _pendingClearHql;
    

        /// <summary>
        /// Initializes a new instance of the <see cref="NhUnitOfWork"/> class.
        /// </summary>
        public NhUnitOfWork() : this(null) { }

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

            _pendingInsert = new List<object>();
            _pendingSave = new List<object>();
            _pendingDelete = new List<object>();
            _pendingDeleteByKey = new List<dynamic>();
            _pendingClearHql = new List<string>();
        }

        /// <summary>
        /// Gets the underlying data context object.
        /// </summary>
        public virtual object UnderlyingContext
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
        /// Register object which will be saved.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public virtual void RegisterSave<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
                _pendingSave.Add(obj);
        }

        /// <summary>
        /// Register object set which will be saved.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        public virtual void RegisterSave<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null)
            {
                foreach (TObject obj in objs)
                {
                    if (obj != null)
                        _pendingSave.Add(obj);
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
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <typeparam name="TKey">The type of object key.</typeparam>
        /// <param name="key">The object key.</param>
        public virtual void RegisterDelete<TObject, TKey>(TKey key) where TObject : class
        {
            var metadata = _session.SessionFactory.GetClassMetadata(typeof(TObject));

            Checker.Requires(metadata.HasIdentifierProperty, "{0} does not has identifier property", typeof(TObject).FullName);

            _pendingDeleteByKey.Add(new { 
                query=string.Format("from {0} o where o.{1}=?", typeof(TObject).Name, metadata.IdentifierPropertyName),
                value=key,
                type=metadata.IdentifierType
            });
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
        /// <remarks>use underlying transaction automatically when the ambient transaction is null.</remarks>
        public void Commit()
        {
            if (System.Transactions.Transaction.Current == null)
            {
                //generate ITransaction if the ambient transaction is null. 
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
        /// Commit changes to data source.
        /// </summary>
        /// <remarks>use underlying transaction automatically.</remarks>
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
            _pendingSave.ToList().ForEach(o => _session.SaveOrUpdate(o));
            _pendingSave.Clear();
            _pendingDelete.ToList().ForEach(o => _session.Delete(o));
            _pendingDelete.Clear();
            _pendingDeleteByKey.ToList().ForEach(o => _session.Delete((string)o.query,(object)o.value,(NHibernate.Type.IType)o.type));
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
