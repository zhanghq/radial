using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Efs
{
    /// <summary>
    /// Entity framework unit of work class.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly INativeQuery _nativeQuery;
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="storageAlias">The storage alias (case insensitive, can be null or empty).</param>
        public UnitOfWork(string storageAlias = null)
        {
            StorageAlias = storageAlias;

            _dbContext = DbContextPool.GetDbContext(StorageAlias);

            _nativeQuery = new NaticeQuery(this);
        }

        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        public string StorageAlias
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether current storage is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the native query.
        /// </summary>
        public INativeQuery NativeQuery
        {
            get
            {
                return _nativeQuery;
            }
        }

        /// <summary>
        /// Gets the underlying data context object.
        /// </summary>
        public object UnderlyingContext
        {
            get
            {
                return _dbContext;
            }
        }

        /// <summary>
        /// Commit changes to data source.
        /// </summary>
        public void Commit()
        {
            Checker.Requires(!IsReadOnly, "commit data to the READ ONLY storage is not supported, alias: {0}", StorageAlias);

            if (_dbContext.Database.CurrentTransaction != null)
            {
                try
                {
                    _dbContext.SaveChanges();
                    _dbContext.Database.CurrentTransaction.Commit();
                }
                catch (Exception ex)
                {
                    _dbContext.Database.CurrentTransaction.Rollback();

                    _dbContext.GetLogger().Fatal(ex);

                    throw;
                }
            }
            else
                _dbContext.SaveChanges();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        /// <summary>
        /// Prepares the transaction, typically this method is invoked implicitly,
        /// but it  also can be explicit used to implement custom control.
        /// </summary>
        /// <param name="level">The isolation level.</param>
        public void PrepareTransaction(IsolationLevel? level = null)
        {
            Checker.Requires(!IsReadOnly, "prepare transaction on the READ ONLY storage is not supported, alias: {0}", StorageAlias);

            //nothing to do, if there has an actived transaction scope
            if (System.Transactions.Transaction.Current == null)
            {
                //nothing to do, if there has an actived transaction
                if (_dbContext.Database.CurrentTransaction == null)
                {
                    if (!level.HasValue)
                        _dbContext.Database.BeginTransaction();
                    else
                        _dbContext.Database.BeginTransaction(level.Value);
                }
            }
        }

        /// <summary>
        /// Register delete all objects.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        public void RegisterClear<TObject>() where TObject : class
        {
            PrepareTransaction();
            _dbContext.Set<TObject>().RemoveRange(_dbContext.Set<TObject>());
        }

        /// <summary>
        /// Register object set which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        public void RegisterDelete<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null && objs.Count() > 0)
            {
                PrepareTransaction();

                foreach (TObject obj in objs)
                {
                    if (obj != null)
                    {
                        if (_dbContext.Entry<TObject>(obj).State == EntityState.Detached)
                            _dbContext.Set<TObject>().Attach(obj);
                        _dbContext.Set<TObject>().Remove(obj);
                    }
                }

            }
        }

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public void RegisterDelete<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
            {
                PrepareTransaction();

                if (_dbContext.Entry<TObject>(obj).State == EntityState.Detached)
                    _dbContext.Set<TObject>().Attach(obj);

                _dbContext.Set<TObject>().Remove(obj);
            }
        }

        /// <summary>
        /// Register object which will be deleted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <typeparam name="TKey">The type of object key.</typeparam>
        /// <param name="key">The object key.</param>
        public void RegisterDelete<TObject, TKey>(TKey key) where TObject : class
        {
            RegisterDelete<TObject>(_dbContext.Set<TObject>().Find(key));
        }

        /// <summary>
        /// Register object set which will be inserted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        public void RegisterNew<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null && objs.Count() > 0)
            {
                PrepareTransaction();

                foreach (TObject obj in objs)
                {
                    if (obj != null)
                        _dbContext.Set<TObject>().Add(obj);
                }

            }
        }

        /// <summary>
        /// Register object which will be inserted.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public void RegisterNew<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
            {
                PrepareTransaction();
                _dbContext.Set<TObject>().Add(obj);
            }
        }

        /// <summary>
        /// Register object set which will be inserted or updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="objs">The object set.</param>
        public void RegisterSave<TObject>(IEnumerable<TObject> objs) where TObject : class
        {
            if (objs != null && objs.Count() > 0)
            {
                PrepareTransaction();

                foreach (TObject obj in objs)
                {
                    if (obj != null)
                    {
                        ObjectContext oc = _dbContext.ToObjectContext();
                        ObjectStateEntry stateEntry = null;
                        oc.ObjectStateManager.TryGetObjectStateEntry(obj, out stateEntry);

                        var objectSet = oc.CreateObjectSet<TObject>();
                        if (stateEntry == null || stateEntry.EntityKey.IsTemporary)
                        {
                            objectSet.AddObject(obj);
                            return;
                        }
                        if (stateEntry.State == EntityState.Detached)
                        {
                            objectSet.Attach(obj);
                            oc.ObjectStateManager.ChangeObjectState(obj, EntityState.Modified);
                            return;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Register object set which will be inserted or updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public void RegisterSave<TObject>(TObject obj) where TObject : class
        {
            if (obj != null)
            {
                ObjectContext oc = _dbContext.ToObjectContext();
                ObjectStateEntry stateEntry = null;
                oc.ObjectStateManager.TryGetObjectStateEntry(obj, out stateEntry);

                var objectSet = oc.CreateObjectSet<TObject>();
                if (stateEntry == null || stateEntry.EntityKey.IsTemporary)
                {
                    objectSet.AddObject(obj);
                    return;
                }
                if (stateEntry.State == EntityState.Detached)
                {
                    objectSet.Attach(obj);
                    oc.ObjectStateManager.ChangeObjectState(obj, EntityState.Modified);
                    return;
                }
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
                {
                    if (obj != null)
                    {
                        if (_dbContext.Entry<TObject>(obj).State == EntityState.Detached)
                        {
                            _dbContext.Set<TObject>().Attach(obj);
                            _dbContext.Entry<TObject>(obj).State = EntityState.Modified;
                        }
                    }
                }

            }
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
                if (_dbContext.Entry<TObject>(obj).State == EntityState.Detached)
                {
                    _dbContext.Set<TObject>().Attach(obj);
                    _dbContext.Entry<TObject>(obj).State = EntityState.Modified;
                }
            }
        }
    }
}
