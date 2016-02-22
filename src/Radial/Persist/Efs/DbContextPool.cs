using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Efs
{
    /// <summary>
    /// DbContextPool
    /// </summary>
    public static class DbContextPool
    {
        static object S_SyncRoot = new object();
        static DbContextSet S_CurrentSet;

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
                        IDbContextPoolInitializer initializer = Dependency.Container.Resolve<IDbContextPoolInitializer>();

                        Checker.Requires(initializer!=null, "can not find any DbContext pool initializer");

                        S_CurrentSet = initializer.Execute();

                        Checker.Requires(S_CurrentSet != null && S_CurrentSet.Count > 0, "can not find any DbContextEntry instance");
                    }
                }
            }
        }


        /// <summary>
        /// Gets the database context entry.
        /// </summary>
        /// <param name="storageAlias">The storage alias.</param>
        /// <returns></returns>
        public static DbContextEntry GetDbContextEntry(string storageAlias = null)
        {
            Initialize();

            if (string.IsNullOrWhiteSpace(storageAlias))
                return S_CurrentSet.FirstEntry;

            return S_CurrentSet[storageAlias];
        }
    }
}
