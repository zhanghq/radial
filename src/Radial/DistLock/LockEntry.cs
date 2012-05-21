using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial;

namespace Radial.DistLock
{
    /// <summary>
    /// Distributed lock entry class.
    /// </summary>
    public sealed class LockEntry : IDisposable
    {
        string _id;
        string _key;
        DateTime _createTime;
        DateTime _expireTime;

        /// <summary>
        /// Occurs when [lock acquire failed].
        /// </summary>
        public static event AcquireFailedEventHandler AcquireFailed;


        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static Logger Logger
        {
            get
            {
                return Logger.GetInstance("Lock");
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="LockEntry"/> class from being created.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="key">The key (case insensitive).</param>
        /// <param name="createTime">The create time.</param>
        /// <param name="expireTime">The expire time.</param>
        private LockEntry(string id, string key, DateTime createTime, DateTime expireTime)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(id), "id can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "key can not be empty or null");

            _id = id.Trim();
            _key = key.Trim().ToLower();
            _createTime = createTime;
            _expireTime = expireTime;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public string Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
        }


        /// <summary>
        /// Gets the create time.
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }
        }


        /// <summary>
        /// Gets the expire time.
        /// </summary>
        public DateTime ExpireTime
        {
            get
            {
                return _expireTime;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance is expired.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is expired; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpired
        {
            get
            {
                return DateTime.Now > ExpireTime;
            }
        }


        /// <summary>
        /// Releases lock entry.
        /// </summary>
        public void Release()
        {
            Logger.Info("release lock, key={0}", Key);
            Instance.Release(Key);
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Release();
        }

        #region Static Methods

        /// <summary>
        /// Gets the ILock instance.
        /// </summary>
        private static ILock Instance
        {
            get
            {
                if (!ComponentContainer.HasComponent(typeof(ILock)))
                    return new EmptyLock();
                return ComponentContainer.Resolve<ILock>();
            }
        }


        /// <summary>
        /// Acquires lock with the specified type
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <returns>The LockEntry instance.</returns>
        public static LockEntry Acquire<T>()
        {
            return Acquire<T>(string.Empty);
        }


        /// <summary>
        /// Acquires lock with the specified type and key
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="keys">The lock keys.</param>
        /// <returns>The LockEntry instance.</returns>
        public static LockEntry Acquire<T>(params string[] keys)
        {
            return Acquire(typeof(T), keys);
        }


        /// <summary>
        /// Acquires lock with the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The LockEntry instance.</returns>
        public static LockEntry Acquire(Type type)
        {
            return Acquire(type, string.Empty);
        }


        /// <summary>
        /// Acquires lock with the specified type and key
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="keys">The lock keys.</param>
        /// <returns>The LockEntry instance.</returns>
        public static LockEntry Acquire(Type type, params string[] keys)
        {
            Checker.Parameter(type != null, "type can not be null");

            string actualLockKey = type.FullName;
            List<string> inputKeys = new List<string>();

            if (keys != null)
            {
                foreach (string k in keys)
                {
                    if (!string.IsNullOrWhiteSpace(k))
                    {
                        inputKeys.Add(k.Trim().ToLower());
                    }
                }

                if (inputKeys.Count > 0)
                    actualLockKey += "_" + string.Join("_", inputKeys);
            }

            return Acquire(actualLockKey);
        }


        /// <summary>
        /// Acquires lock with the specified key.
        /// </summary>
        /// <param name="key">The key (case insensitive).</param>
        /// <returns>The LockEntry instance.</returns>
        public static LockEntry Acquire(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "key can not be empty or null.");

            key = key.Trim().ToLower();

            LockObject obj = null;

            Logger.Info("begin to acquire lock, key={0}", key);

            bool successfullyAcquired = false;
            AcquireFailedEventArgs afea = new AcquireFailedEventArgs { LockKey = key, OutputLogger = Logger };

            do
            {
                afea.CancelRetry = true;
                afea.Exception = null;

                try
                {
                    if (Instance.Acquire(key, out obj))
                    {
                        successfullyAcquired = true;
                        break;
                    }
                }
                catch (Exception e)
                {
                    afea.Exception = e;
                }

                afea.RetryTimes++;

                if (AcquireFailed != null)
                    AcquireFailed(null, afea);
            }
            while (!afea.CancelRetry);

            if (!successfullyAcquired)
            {
                Logger.Fatal("can not acquire lock, key={0}", key);
                throw new Exception("some data has be locked, please try again later.");
            }

            return new LockEntry(obj.Id, obj.Key, obj.CreateTime, obj.ExpireTime);
        }

        /// <summary>
        /// Gets the lock list.
        /// </summary>
        /// <returns>A list that contains all available locks.</returns>
        public static IList<LockEntry> GetList()
        {
            IList<LockEntry> list = new List<LockEntry>();

            IList<LockObject> objList = Instance.GetList();

            foreach (LockObject o in objList)
            {
                list.Add(new LockEntry(o.Id, o.Key, o.CreateTime, o.ExpireTime));
            }

            return list;
        }

        #endregion
    }
}
