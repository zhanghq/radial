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
        static bool S_Initialized = false;
        static SessionFactoryWrapperSet S_SessionFactorySet;

        ///// <summary>
        ///// Gets the session factory set.
        ///// </summary>
        ///// <value>
        ///// The session factory set.
        ///// </value>
        //public static SessionFactoryWrapperSet SessionFactorySet
        //{
        //    get
        //    {
        //        if (S_SessionFactorySet == null)
        //        {
        //            Initialize();
        //        }

        //        return S_SessionFactorySet;
        //    }
        //}

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (S_SessionFactorySet == null)
            {
                lock (S_SyncRoot)
                {
                    if (S_SessionFactorySet == null)
                    {
                        IFactoryPoolInitializer initializer = Dependency.Container.Resolve<IFactoryPoolInitializer>();

                        Checker.Requires(initializer != null, "can not found any session factory pool initializer");

                        S_SessionFactorySet = initializer.Initialize();

                        Checker.Requires(S_SessionFactorySet != null && S_SessionFactorySet.Count > 0, 
                            "can not found any session factory wrapper");

                        S_Initialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Open a new session using the specified storage alias.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive),
        /// if set to null will open session from the first NHibernate.ISessionFactory instance.</param>
        /// <returns>A new ISession instance.</returns>
        public static ISession OpenSession(string alias = null)
        {
            Checker.Requires(S_Initialized, "session factory pool uninitialized");

            if (alias == null)
                return S_SessionFactorySet[0].SessionFactory.OpenSession();

            return S_SessionFactorySet[alias].SessionFactory.OpenSession();
        }

        //static ISet<ConfigurationWrapper> S_ConfigurationWrapperSet;

        ///// <summary>
        ///// Initializes the <see cref="SessionFactoryPool"/> class.
        ///// </summary>
        //static SessionFactoryPool()
        //{
        //    if (S_ConfigurationWrapperSet == null)
        //    {
        //        Logger.Debug("begin initialize session factory pool");

        //        IFactoryPoolInitializer initializer = null;
        //        if (Dependency.Container.IsRegistered<IFactoryPoolInitializer>())
        //        {
        //            initializer = Dependency.Container.Resolve<IFactoryPoolInitializer>();
        //        }
        //        else
        //        {
        //            //use default when not set.
        //            Logger.Debug("can not found any session factory pool initializer, using DefaultFactoryPoolInitializer instead");
        //            initializer = new DefaultFactoryPoolInitializer();
        //        }

        //        S_ConfigurationWrapperSet = initializer.Execute();

        //        Checker.Requires(S_ConfigurationWrapperSet != null && S_ConfigurationWrapperSet.Count > 0, "failed to initialize: ConfigurationWrapper set was null or empty");

        //        Logger.Debug("end initialize session factory pool");
        //    }
        //}

        ///// <summary>
        ///// Gets the logger.
        ///// </summary>
        //private static Logger Logger
        //{
        //    get
        //    {
        //        return Logger.GetInstance("SessionFactoryPool");
        //    }
        //}


        /////// <summary>
        /////// Gets the first NHibernate.ISessionFactory instance.
        /////// </summary>
        ////public static ISessionFactory First
        ////{
        ////    get
        ////    {
        ////        ConfigurationWrapper wrapper = GetConfigurationWrappers().FirstOrDefault();

        ////        return wrapper.Factory;
        ////    }
        ////}


        ///// <summary>
        ///// Gets the ConfigurationWrapper object with the specified storage alias.
        ///// </summary>
        ///// <param name="storageAlias">The storage alias (case insensitive),
        ///// if set to null will get the first ConfigurationWrapper object.</param>
        ///// <returns>The ConfigurationWrapper object.</returns>
        //public static ConfigurationWrapper GetConfigurationWrapper(string storageAlias=null)
        //{
        //    return GetConfigurationWrappers(storageAlias).FirstOrDefault();
        //}

        ///// <summary>
        ///// Gets the ConfigurationWrapper object with the specified storage aliases.
        ///// </summary>
        ///// <param name="storageAliases">The storage alias array (case insensitive).</param>
        ///// <returns>The ConfigurationWrapper object array.</returns>
        //public static ConfigurationWrapper[] GetConfigurationWrappers(params string[] storageAliases)
        //{
        //    var result = S_ConfigurationWrapperSet.ToArray();

        //    if (storageAliases != null && storageAliases.Length > 0)
        //    {
        //        storageAliases = storageAliases.Where(o => !string.IsNullOrWhiteSpace(o)).ToArray();

        //        if (storageAliases.Length > 0)
        //        {
        //            IList<ConfigurationWrapper> list = new List<ConfigurationWrapper>();

        //            foreach (string alias in storageAliases)
        //            {
        //                ConfigurationWrapper wrapper = S_ConfigurationWrapperSet.SingleOrDefault(o => string.Compare(o.Alias, alias.Trim(), true) == 0);

        //                Checker.Requires(wrapper != null, "can not find the specified ConfigurationWrapper instance, alias: {0}", alias.Trim());

        //                list.Add(wrapper);
        //            }

        //            return list.ToArray();
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Gets the <see cref="NHibernate.ISessionFactory"/> instance with the specified storage alias.
        ///// </summary>
        ///// <param name="storageAlias">The storage alias (case insensitive),
        ///// if set to null will get the first NHibernate.ISessionFactory instance.</param>
        ///// <returns>The NHibernate.ISessionFactory instance</returns>
        //public static ISessionFactory GetFactoryInstance(string storageAlias = null)
        //{
        //    return GetConfigurationWrapper(storageAlias).Factory;
        //}

        ///// <summary>
        ///// Gets all storage aliases.
        ///// </summary>
        ///// <returns>
        ///// The storage aliases array.
        ///// </returns>
        //public static string[] GetStorageAliases()
        //{
        //    return S_ConfigurationWrapperSet.Select(o=>o.Alias).ToArray();
        //}

        ///// <summary>
        ///// Open a new session using the specified storage alias.
        ///// </summary>
        ///// <param name="storageAlias">The storage alias (case insensitive),
        ///// if set to null will open session from the first NHibernate.ISessionFactory instance.</param>
        ///// <returns>A new ISession instance.</returns>
        //public static ISession OpenSession(string storageAlias=null)
        //{
        //    return GetFactoryInstance(storageAlias).OpenSession();
        //}
    }
}
