using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Efs
{
    /// <summary>
    /// EfUnitOfWork
    /// </summary>
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly INativeQuery _nativeQuery;
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWork"/> class.
        /// </summary>
        public EfUnitOfWork() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWork" /> class.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive, can be null or empty).</param>
        public EfUnitOfWork(string alias)
        {
            _dbContext = DbContextPool.GetDbContext(alias);

            _nativeQuery = new EfNaticeQuery(this);
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
            //if (objs != null && objs.Count() > 0)
            //{
            //    PrepareTransaction();

            //    foreach (TObject obj in objs)
            //    {
            //        if (obj != null)
            //        {
            //            var kvs = _dbContext.GetKeyValues<TObject>(obj);

            //            var eobj = _dbContext.Set<TObject>().Find(kvs);

            //            if (eobj != null)
            //                _dbContext.Entry<TObject>(obj).State = EntityState.Modified;
            //            else
            //                _dbContext.Set<TObject>().Add(obj);
            //        }
            //    }

            //}
        }

        /// <summary>
        /// Register object set which will be inserted or updated.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="obj">The object instance.</param>
        public void RegisterSave<TObject>(TObject obj) where TObject : class
        {
            //if (obj != null)
            //{
            //    if (_dbContext.Set<TObject>().Contains(obj))
            //        _dbContext.Entry<TObject>(obj).State = EntityState.Modified;
            //    else
            //        _dbContext.Set<TObject>().Add(obj);
            //}
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
