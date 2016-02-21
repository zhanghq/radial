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
        static SessionFactorySet S_CurrentSet;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private static void Initialize()
        {
            lock (S_SyncRoot)
            {
                if (S_CurrentSet == null)
                {
                    lock (S_SyncRoot)
                    {
                        IFactoryPoolInitializer initializer = Dependency.Container.Resolve<IFactoryPoolInitializer>();

                        Checker.Requires(initializer != null, "can not find any session factory pool initializer");

                        S_CurrentSet = initializer.Execute();

                        Checker.Requires(S_CurrentSet != null && S_CurrentSet.Count > 0, "can not find any SessionFactoryEntry instance");
                    }
                }
            }
        }


        /// <summary>
        /// Gets the session factory entry.
        /// </summary>
        /// <param name="storageAlias">The storage alias.</param>
        /// <returns></returns>
        public static SessionFactoryEntry GetSessionFactoryEntry(string storageAlias = null)
        {
            Initialize();

            if (string.IsNullOrWhiteSpace(storageAlias))
                return S_CurrentSet.FirstEntry;

            return S_CurrentSet[storageAlias];
        }

        /// <summary>
        /// Gets the session factory.
        /// </summary>
        /// <param name="storageAlias">The storage alias.</param>
        /// <returns></returns>
        public static ISessionFactory GetSessionFactory(string storageAlias = null)
        {
            return GetSessionFactoryEntry(storageAlias).SessionFactory;
        }
    }
}
