using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Radial.DistLock.Client.ServiceReference;

namespace Radial.DistLock.Client
{
    /// <summary>
    /// ServiceClient
    /// </summary>
    public class ServiceClient :ClientBase<ILock>, ILock
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClient"/> class.
        /// </summary>
        public ServiceClient()
            : base()
        {
        }

        #region ILock Members

        /// <summary>
        /// Acquires a look with the specified key.
        /// </summary>
        /// <param name="key">The key (case insensitive).</param>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// If successful acquires the lock return true, otherwise return false
        /// </returns>
        public bool Acquire(string key, out LockObject obj)
        {
            LockClient c = new LockClient();
            bool result = c.Acquire(key, out obj);
            c.Close();

            return result;
        }

        /// <summary>
        /// Releases a lock with the specified key.
        /// </summary>
        /// <param name="key">The key (case insensitive).</param>
        public void Release(string key)
        {
            LockClient c = new LockClient();
            c.Release(key);
            c.Close();
        }

        /// <summary>
        /// Gets the lock list.
        /// </summary>
        /// <returns>
        /// A list that contains all available locks.
        /// </returns>
        public IList<LockObject> GetList()
        {
            IList<LockObject> list = new List<LockObject>();
            LockClient c = new LockClient();
            list = c.GetList();
            c.Close();
            return list;
        }

        #endregion
    }
}
