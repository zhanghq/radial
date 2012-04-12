using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Lock
{
    /// <summary>
    /// The default ILock concret class with no lock implement
    /// </summary>
    public class EmptyLock : ILock
    {
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
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "key can not be empty or null");
            DateTime createTime = DateTime.Now;
            obj = new LockObject { Key = key.ToLower().Trim(), CreateTime = createTime, ExpireTime = createTime };
            Logger.GetInstance("Lock").Debug("Lock key:{0} acquired", obj.Key);
            return true;
        }

        /// <summary>
        /// Releases a lock with the specified key.
        /// </summary>
        /// <param name="key">The key (case insensitive).</param>
        public void Release(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "key can not be empty or null");

            Logger.GetInstance("Lock").Debug("Lock key:{0} manual released", key.ToLower().Trim());
        }

        /// <summary>
        /// Gets the lock list.
        /// </summary>
        /// <returns>
        /// A list that contains all available locks.
        /// </returns>
        public IList<LockObject> GetList()
        {
            return new List<LockObject>();
        }
    }
}
