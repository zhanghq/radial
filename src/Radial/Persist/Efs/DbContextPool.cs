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

        static ISet<DbContextWrapper> S_DbContextWrapperSet;

        /// <summary>
        /// Initializes the <see cref="DbContextPool"/> class.
        /// </summary>
        static DbContextPool()
        {
            if (S_DbContextWrapperSet == null)
            {
                Logger.Debug("begin initialize DbContext pool");

                Checker.Requires(Dependency.Container.IsRegistered<IDbContextPoolInitializer>(), "can not found any DbContext pool initializer");

                IDbContextPoolInitializer initializer = Dependency.Container.Resolve<IDbContextPoolInitializer>();
                S_DbContextWrapperSet = initializer.Execute();

                Checker.Requires(S_DbContextWrapperSet != null && S_DbContextWrapperSet.Count > 0,
                    "failed to initialize: DbContextWrapper set was null or empty");

                Logger.Debug("end initialize DbContext pool");
            }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static Logger Logger
        {
            get
            {
                return Logger.GetInstance("DbContextPool");
            }
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <param name="storageAlias">The storage alias (case insensitive),
        /// if set to null will get the first DbContext instance.</param>
        /// <returns>The DbContext instance.</returns>
        public static DbContext GetDbContext(string storageAlias = null)
        {
            return GetDbContextWrapper(storageAlias).NewDbContext();
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <param name="storageAlias">The storage alias (case insensitive),
        /// if set to null will get the first DbContext instance.</param>
        /// <returns>The DbContext instance.</returns>
        public static DbContextWrapper GetDbContextWrapper(string storageAlias = null)
        {
            return GetDbContextWrappers(storageAlias).FirstOrDefault();
        }

        /// <summary>
        /// Gets the database context wrappers.
        /// </summary>
        /// <param name="storageAliases">The storage aliases.</param>
        /// <returns></returns>
        public static DbContextWrapper[] GetDbContextWrappers(params string[] storageAliases)
        {
            var result = S_DbContextWrapperSet.ToArray();

            if (storageAliases != null && storageAliases.Length > 0)
            {
                storageAliases = storageAliases.Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();

                if (storageAliases.Length > 0)
                {
                    IList<DbContextWrapper> list = new List<DbContextWrapper>();

                    foreach (string alias in storageAliases)
                    {
                        string normalizedAlias = alias.ToLower().Trim();
                        DbContextWrapper wrapper = S_DbContextWrapperSet.SingleOrDefault(o => o.Alias == normalizedAlias);

                        Checker.Requires(wrapper != null, "can not find the specified DbContextWrapper instance, alias: {0}", normalizedAlias);

                        list.Add(wrapper);
                    }

                    return list.ToArray();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all storage aliases.
        /// </summary>
        /// <returns>
        /// The storage aliases array.
        /// </returns>
        public static string[] GetStorageAliases()
        {
            IList<string> aliases = new List<string>(S_DbContextWrapperSet.Count);

            foreach (DbContextWrapper wrapper in S_DbContextWrapperSet)
                aliases.Add(wrapper.Alias);

            return aliases.ToArray();

        }
    }
}
