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

        static bool Initialized = false;

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
                if (Initialized)
                    return;

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

                        IBootTask task = Activator.CreateInstance(Type.GetType(e.Type)) as IBootTask;

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
                //higher priority start first
                if (Initialized)
                    Tasks.ForEach(o => o.Value.Start());
            }
        }

        /// <summary>
        /// Stop system.
        /// </summary>
        public static void Stop()
        {
            lock (SyncRoot)
            {
                if (Initialized)
                {
                    //higher priority stop last
                    var reversed = new List<KeyValuePair<int, IBootTask>>(Tasks.Reverse<KeyValuePair<int, IBootTask>>());
                    reversed.ForEach(o => o.Value.Stop());
                }
            }
        }
    }
}
