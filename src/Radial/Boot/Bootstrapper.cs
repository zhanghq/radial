using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Radial.Boot.Cfg;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Web;

namespace Radial.Boot
{
    /// <summary>
    /// System bootstrapper.
    /// </summary>
    public static class Bootstrapper
    {
        static object SyncRoot = new object();

        static List<BootTaskInstance> Tasks = new List<BootTaskInstance>();
        static List<BootTaskInstance> ReversedTasks = new List<BootTaskInstance>();

        static bool Initialized = false;

        class BootTaskInstance
        {
           public IBootTask Instance { get; set; }
           public int Priority { get; set; }
           public NameValueCollection Arguments { get; set; }
        }

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
        /// <param name="args">The arguments.</param>
        /// <param name="priority">The priority.</param>
        public static void RegisterTask(IBootTask task, NameValueCollection args = null, int priority = 0)
        {
            lock (SyncRoot)
            {
                Checker.Requires(!Initialized, "can not register boot task after system initialized");

                if (task != null)
                {
                    Checker.Requires(!Tasks.Contains(o => o.Instance.GetType() == task.GetType()),
                        "duplicated boot task: {0}", task.GetType().FullName);

                    Tasks.Add(new BootTaskInstance { Priority = priority, Instance = task, Arguments = args });
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
                            Checker.Requires(!Tasks.Contains(o => o.Instance.GetType() == task.GetType()), 
                                "duplicated boot task: {0}", task.GetType().FullName);

                            Tasks.Add(new BootTaskInstance { Priority = e.Priority, Instance = task, Arguments = e.GetArgumentCollection() });
                        }
                    }
                }

                //The larger value the higher priority
                Tasks.Sort((a,b) =>
                {
                    if (a.Priority > b.Priority)
                        return -1;

                    if (a.Priority < b.Priority)
                        return 1;

                    return 0;
                });

                ReversedTasks = new List<BootTaskInstance>(Tasks.Reverse<BootTaskInstance>());

                //higher priority initialize first
                Tasks.ForEach(o => o.Instance.Initialize(o.Arguments));

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
                {
                    Tasks.ForEach(o => o.Instance.Start());
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
                //higher priority stop last
                if (Initialized)
                {
                    ReversedTasks.ForEach(o => o.Instance.Stop());
                }
            }
        }
    }
}
