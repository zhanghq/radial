using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Microsoft.Practices.Unity;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Hibernate session factory pool.
    /// </summary>
    public static class SessionFactoryPool
    {
        static ISet<ConfigurationWrapper> S_ConfigurationWrapperSet;

        /// <summary>
        /// Initializes the <see cref="SessionFactoryPool"/> class.
        /// </summary>
        static SessionFactoryPool()
        {
            if (S_ConfigurationWrapperSet == null)
            {
                Logger.Debug("begin initialize session factory pool");

                IFactoryPoolInitializer initializer = null;
                if (Dependency.Container.IsRegistered<IFactoryPoolInitializer>())
                {
                    initializer = Dependency.Container.Resolve<IFactoryPoolInitializer>();
                }
                else
                {
                    //use default when not set.
                    Logger.Debug("can not found any session factory pool initializer, using DefaultFactoryPoolInitializer instead");
                    initializer = new DefaultFactoryPoolInitializer();
                }

                S_ConfigurationWrapperSet = initializer.Execute();

                Checker.Requires(S_ConfigurationWrapperSet != null && S_ConfigurationWrapperSet.Count > 0, "failed to initialize: ConfigurationWrapper set was null or empty");

                Logger.Debug("end initialize session factory pool");
            }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static Logger Logger
        {
            get
            {
                return Logger.GetInstance("SessionFactoryPool");
            }
        }


        /// <summary>
        /// Gets the first NHibernate.ISessionFactory instance.
        /// </summary>
        public static ISessionFactory First
        {
            get
            {
                ConfigurationWrapper wrapper = GetConfigurationWrappers().FirstOrDefault();

                return wrapper.Factory;
            }
        }


        /// <summary>
        /// Gets the ConfigurationWrapper object with the specified storage alias.
        /// </summary>
        /// <param name="storageAlias">The storage alias (case insensitive).</param>
        /// <returns>The ConfigurationWrapper object.</returns>
        public static ConfigurationWrapper GetConfigurationWrapper(string storageAlias)
        {
            return GetConfigurationWrappers(storageAlias).FirstOrDefault();
        }

        /// <summary>
        /// Gets the ConfigurationWrapper object with the specified storage aliases.
        /// </summary>
        /// <param name="storageAliases">The storage alias array (case insensitive).</param>
        /// <returns>The ConfigurationWrapper object array.</returns>
        public static ConfigurationWrapper[] GetConfigurationWrappers(params string[] storageAliases)
        {
            IList<ConfigurationWrapper> list = new List<ConfigurationWrapper>();

            if (storageAliases != null && storageAliases.Length > 0)
            {
                foreach (string alias in storageAliases)
                {
                    string normalizedAlias = alias.ToLower().Trim();
                    ConfigurationWrapper wrapper = S_ConfigurationWrapperSet.SingleOrDefault(o => o.Alias == normalizedAlias);

                    Checker.Requires(wrapper != null, "can not find the specified ConfigurationWrapper instance, alias: {0}", normalizedAlias);

                    list.Add(wrapper);
                }

                return list.ToArray();
            }

            return S_ConfigurationWrapperSet.ToArray();

        }


        /// <summary>
        /// Gets the <see cref="NHibernate.ISessionFactory"/> instance with the specified storage alias.
        /// </summary>
        /// <param name="storageAlias">The storage alias (case insensitive).</param>
        /// <returns>The NHibernate.ISessionFactory instance</returns>
        public static ISessionFactory GetFactoryInstance(string storageAlias)
        {
            return GetConfigurationWrapper(storageAlias).Factory;
        }

        /// <summary>
        /// Gets all storage aliases.
        /// </summary>
        /// <returns>
        /// The storage aliases array.
        /// </returns>
        public static string[] GetStorageAliases()
        {
            IList<string> aliases = new List<string>(S_ConfigurationWrapperSet.Count);

            foreach (ConfigurationWrapper wrapper in S_ConfigurationWrapperSet)
                aliases.Add(wrapper.Alias);

            return aliases.ToArray();

        }

        /// <summary>
        /// Open a new session using the specified storage alias.
        /// </summary>
        /// <param name="storageAlias">The storage alias (case insensitive).</param>
        /// <returns>A new ISession instance.</returns>
        public static ISession OpenSession(string storageAlias)
        {
            return GetFactoryInstance(storageAlias).OpenSession();
        }
    }
}
