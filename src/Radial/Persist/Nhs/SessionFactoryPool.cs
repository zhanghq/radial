using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Microsoft.Practices.Unity;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Hibernate session factory pool.
    /// </summary>
    public static class SessionFactoryPool
    {
        static object S_SyncRoot = new object();

        /// <summary>
        /// Initializes the <see cref="SessionFactoryPool"/> class.
        /// </summary>
        static SessionFactoryPool()
        {
            lock (S_SyncRoot)
            {
                IFactoryPoolInitializer initializer = Dependency.Container.Resolve<IFactoryPoolInitializer>();

                Checker.Requires(initializer != null, "can not found any session factory pool initializer");

                CurrentSet = initializer.Execute();

                Checker.Requires(CurrentSet != null && CurrentSet.Count > 0, "can not found any ConfigurationEntry instance");

                Checker.Requires(CurrentSet.All(o => o != null), "any ConfigurationEntry instances can not be null");
            }
        }

        /// <summary>
        /// Gets the current configuration set.
        /// </summary>
        public static ConfigurationSet CurrentSet
        {
            get;
            private set;
        }
    }
}
