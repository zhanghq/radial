using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using Radial.Boot.Cfg;

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
        /// System initialize process.
        /// </summary>
        public static void Initialize()
        {
            lock (SyncRoot)
            {
                if (Initialized)
                    return;


                BootTaskSection section = ConfigurationManager.GetSection("boot") as BootTaskSection;

                if (section == null)
                    return;

                foreach (BootTaskElement e in section.Tasks)
                {
                    IBootTask task = Activator.CreateInstance(Type.GetType(e.Type)) as IBootTask;

                    if (task != null)
                        Tasks.Add(new KeyValuePair<int, IBootTask>(e.Priority, task));
                }

                Tasks.Sort((a, b) =>
                {
                    if (a.Key > b.Key)
                        return -1;

                    if (a.Key < b.Key)
                        return 1;

                    return 0;
                });

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
                    Tasks.ForEach(o => o.Value.Stop());
            }
        }
    }
}
