using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Radial.Param;
using System.Threading;

namespace Radial.Lock.WinService
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class LockService : ILock
    {
        static object SyncRoot = new object();
        static HashSet<LockObject> S_LockSet;
        Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockService"/> class.
        /// </summary>
        public LockService()
        {
            lock (SyncRoot)
            {
                S_LockSet = new HashSet<LockObject>();
                _timer = new Timer(TimerCallback, null, TimerPeriod, TimerPeriod);
                
            }
        }


        /// <summary>
        /// Gets the timer period.
        /// </summary>
        public int TimerPeriod
        {
            get
            {
                return AppParam.GetValueInt32("system.timerperiod", 2) * 1000;
            }
        }

        /// <summary>
        /// Timers the callback.
        /// </summary>
        /// <param name="state">The state.</param>
        public void TimerCallback(object state)
        {
            Logger.Default.Debug("begin clean up");

            lock (SyncRoot)
            {
                IList<LockObject> objs = S_LockSet.Where(o => o.IsExpired).ToList();

                IList<string> keys = new List<string>();

                foreach (LockObject obj in objs)
                {
                    S_LockSet.Remove(obj);
                    keys.Add(obj.Key);
                }
                if (keys.Count > 0)
                    Logger.Default.Info("evict keys:{0}", string.Join(",", keys));
            }

            Logger.Default.Debug("end clean up");
        }

        /// <summary>
        /// Gets the expired seconds.
        /// </summary>
        private int ExpiredSeconds
        {
            get
            {
                return AppParam.GetValueInt32("system.expired", 30);
            }
        }

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
            obj = null;

            if(string.IsNullOrWhiteSpace(key))
            {
                Logger.Default.Warn("key is null or white space");
                return true;
            }

            key=key.Trim().ToLower();

            Logger.Default.Info("acquire lock key={0}", key);

            lock (SyncRoot)
            {
                LockObject lockObj = S_LockSet.SingleOrDefault(o => o.Key == key);

                //not null an not expired
                if (lockObj != null&&!lockObj.IsExpired)
                    return false;

                DateTime now = DateTime.Now;

                if (lockObj == null)
                {
                    lockObj = new LockObject { Key = key, CreateTime = now, ExpireTime = now.AddSeconds(ExpiredSeconds) };

                    S_LockSet.Add(lockObj);
                }
                else
                {
                    if (lockObj.IsExpired)
                    {
                        lockObj.CreateTime = now;
                        lockObj.ExpireTime = now.AddSeconds(ExpiredSeconds);
                    }
                }

                obj = lockObj;

                Logger.Default.Info("set lock key={0}, create:{1}, expired:{2}", key, lockObj.CreateTime.ToString("yyyy/MM/dd HH:mm:ss.fff"), lockObj.ExpireTime.ToString("yyyy/MM/dd HH:mm:ss.fff"));

                return true;

            }
        }

        /// <summary>
        /// Releases a lock with the specified key.
        /// </summary>
        /// <param name="key">The key (case insensitive).</param>
        public void Release(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                Logger.Default.Warn("key is null or white space");

            }

            key = key.Trim().ToLower();

            Logger.Default.Info("release lock key={0}", key);

            lock (SyncRoot)
            {
                LockObject lockObj = S_LockSet.SingleOrDefault(o => o.Key == key);

                if (lockObj != null)
                {
                    //not null and not expired
                    S_LockSet.Remove(lockObj);
                }
            }

        }

        /// <summary>
        /// Gets the lock list.
        /// </summary>
        /// <returns>
        /// A list that contains all available locks.
        /// </returns>
        public IList<LockObject> GetList()
        {
            return S_LockSet.ToList();
        }
    }
}
