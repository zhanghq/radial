using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using Radial.Boot.Cfg;
using Radial.Extensions;

namespace Radial.Boot
{
    /// <summary>
    /// System bootstrapper.
    /// </summary>
    public static class Bootstrapper
    {
        static object SyncRoot = new object();

        static List<KeyValuePair<int, IBootTask>> Tasks = new List<KeyValuePair<int, IBootTask>>();
        static List<KeyValuePair<int, IBootTask>> ReversedTasks = new List<KeyValuePair<int, IBootTask>>();

        static bool Initialized = false;
        static bool Started = false;
        static bool Stopped = false;

        /// <summary>
        /// Gets a value indicating whether system is successful initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if successful initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized
        {
            get
            {
                lock (SyncRoot)
                {
                    return Initialized;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether system is successful started.
        /// </summary>
        /// <value>
        /// <c>true</c> if successful started; otherwise, <c>false</c>.
        /// </value>
        public static bool IsStarted
        {
            get
            {
                lock (SyncRoot)
                {
                    return Started;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether system is successful stopped.
        /// </summary>
        /// <value>
        /// <c>true</c> if successful stopped; otherwise, <c>false</c>.
        /// </value>
        public static bool IsStopped
        {
            get
            {
                lock (SyncRoot)
                {
                    return Stopped;
                }
            }
        }

        /// <summary>
        /// Registers boot task.
        /// </summary>
        /// <param name="task">The boot task.</param>
        public static void RegisterTask(IBootTask task)
        {
            RegisterTask(task, 0);
        }

        /// <summary>
        /// Registers boot task.
        /// </summary>
        /// <param name="task">The boot task.</param>
        /// <param name="priority">The priority.</param>
        public static void RegisterTask(IBootTask task, int priority)
        {
            lock (SyncRoot)
            {
                Checker.Requires(!Initialized, "can not register boot task after system initialized");

                if (task != null)
                {
                    Checker.Requires(!Tasks.Contains(o => o.Value.GetType() == task.GetType()), "duplicated boot task: {0}", task.GetType().FullName);

                    Tasks.Add(new KeyValuePair<int, IBootTask>(priority, task));
                }
            }
        }

        /// <summary>
        /// System initialize process.
        /// </summary>
        public static void Initialize()
        {
            lock (SyncRoot)
            {
                if (Initialized)
                    return;

                BootTaskSection section = ConfigurationManager.GetSection("boot") as BootTaskSection;

                if (section != null)
                {

                    foreach (BootTaskElement e in section.Tasks)
                    {
                        Checker.Requires(!string.IsNullOrWhiteSpace(e.Type), "boot task type can not be empty or null");

                        Type eType = Type.GetType(e.Type);

                        Checker.Requires(eType != null, "can not find boot task type: {0}", e.Type);

                        IBootTask task = Activator.CreateInstance(eType) as IBootTask;

                        if (task != null)
                        {
                            Checker.Requires(!Tasks.Contains(o => o.Value.GetType() == task.GetType()), "duplicated boot task: {0}", task.GetType().FullName);

                            Tasks.Add(new KeyValuePair<int, IBootTask>(e.Priority, task));
                        }
                    }
                }

                //The larger value the higher priority
                Tasks.Sort((a, b) =>
                {
                    if (a.Key > b.Key)
                        return -1;

                    if (a.Key < b.Key)
                        return 1;

                    return 0;
                });

                ReversedTasks = new List<KeyValuePair<int, IBootTask>>(Tasks.Reverse<KeyValuePair<int, IBootTask>>());

                //higher priority initialize first
                Tasks.ForEach(o => o.Value.Initialize());

                Initialized = true;
            }
        }

        /// <summary>
        /// Start system.
        /// </summary>
        public static void Start()
        {
            lock (SyncRoot)
            {
                if (Started)
                    return;

                //higher priority start first
                if (Initialized)
                {
                    Tasks.ForEach(o => o.Value.Start());
                    Started = true;
                }

            }
        }

        /// <summary>
        /// Stop system.
        /// </summary>
        public static void Stop()
        {
            lock (SyncRoot)
            {
                if (Stopped)
                    return;

                //higher priority stop last
                if (Initialized)
                {
                    ReversedTasks.ForEach(o => o.Value.Stop());
                    Stopped = true;
                }
            }
        }
    }
}
