using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Radial.DistLock
{
    /// <summary>
    /// Distributed lock interface
    /// </summary>
    [ServiceContract]
    public interface ILock
    {
        /// <summary>
        /// Acquires a look with the specified key.
        /// </summary>
        /// <param name="key">The key (case insensitive).</param>
        /// <param name="obj">The obj.</param>
        /// <returns>If successful acquires the lock return true, otherwise return false</returns>
        [OperationContract]
        bool Acquire(string key, out LockObject obj);

        /// <summary>
        /// Releases a lock with the specified key.
        /// </summary>
        /// <param name="key">The key (case insensitive).</param>
        [OperationContract]
        void Release(string key);

        /// <summary>
        /// Gets the lock list.
        /// </summary>
        /// <returns>A list that contains all available locks.</returns>
        [OperationContract]
        IList<LockObject> GetList();
    }
}
