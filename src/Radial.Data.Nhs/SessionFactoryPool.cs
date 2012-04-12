using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Radial.Data.Nhs
{
    /// <summary>
    /// Hibernate session factory pool.
    /// </summary>
    public static class SessionFactoryPool
    {
        static object S_SyncRoot = new object();
        static ISet<SessionFactoryWrapper> S_FactoryWrapperSet;

        const string NotInitializedExceptionMessage = "hibernate session factory pool has not been initialized";
        const string CanNotFindInstanceExceptionMessage = "can not find the specified ISessionFactory instance";
        const string CanNotFindInstanceExceptionMessageWithAlias = "can not find the specified ISessionFactory instance, alias: {0}";

        /// <summary>
        /// Gets a value indicating whether session factory pool has been initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if session factory pool has been initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool HasInitialized
        {
            get
            {
                lock (S_SyncRoot)
                {
                    return S_FactoryWrapperSet != null;
                }
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
        /// Initializes the session factory pool.
        /// </summary>
        public static void Initialize()
        {
            lock (S_SyncRoot)
            {
                if (S_FactoryWrapperSet == null)
                {
                    Logger.Debug("begin initialize session factory pool");

                    IFactoryPoolInitializer initializer = null;
                    if (ComponentContainer.HasComponent<IFactoryPoolInitializer>())
                    {
                        initializer = ComponentContainer.Resolve<IFactoryPoolInitializer>();
                    }
                    else
                    {
                        //use default when not set.
                        Logger.Debug("can not found any session factory pool initializer, using DefaultFactoryPoolInitializer instead");
                        initializer = new DefaultFactoryPoolInitializer();
                    }

                    S_FactoryWrapperSet = initializer.Execute();

                    Checker.Requires(S_FactoryWrapperSet != null && S_FactoryWrapperSet.Count > 0, "failed to initialize: SessionFactoryWrapper set was null or empty");

                    Logger.Debug("end initialize session factory pool");
                }
            }
        }

        /// <summary>
        /// Gets the first NHibernate.ISessionFactory instance.
        /// </summary>
        public static ISessionFactory First
        {
            get
            {
                lock (S_SyncRoot)
                {
                    Checker.Requires(HasInitialized, NotInitializedExceptionMessage);

                    SessionFactoryWrapper wrapper = S_FactoryWrapperSet.FirstOrDefault();

                    return wrapper.Factory;
                }
            }
        }


        /// <summary>
        /// Gets the SessionFactoryWrapper object with the specified factory alias.
        /// </summary>
        /// <param name="alias">The factory alias (case insensitive).</param>
        /// <returns>The SessionFactoryWrapper object.</returns>
        public static SessionFactoryWrapper GetWrapper(string alias)
        {
            return GetWrappers(alias)[0];
        }

        /// <summary>
        /// Gets the SessionFactoryWrapper object with the specified factory aliases.
        /// </summary>
        /// <param name="aliases">The factory alias array (case insensitive).</param>
        /// <returns>The SessionFactoryWrapper object array.</returns>
        public static SessionFactoryWrapper[] GetWrappers(params string[] aliases)
        {
            IList<SessionFactoryWrapper> list = new List<SessionFactoryWrapper>();

            lock (S_SyncRoot)
            {
                Checker.Requires(HasInitialized, NotInitializedExceptionMessage);

                if (aliases.Length > 0)
                {
                    foreach (string alias in aliases)
                    {
                        Checker.Requires(!string.IsNullOrWhiteSpace(alias), "factory alias can not be empty or null");

                        string normalizedAlias = alias.ToLower().Trim();
                        SessionFactoryWrapper wrapper = S_FactoryWrapperSet.SingleOrDefault(o => o.Alias == normalizedAlias);

                        Checker.Requires(wrapper != null, CanNotFindInstanceExceptionMessageWithAlias, normalizedAlias);

                        list.Add(wrapper);

                        return list.ToArray();
                    }
                }

                return S_FactoryWrapperSet.ToArray();
            }
        }


        /// <summary>
        /// Gets the <see cref="NHibernate.ISessionFactory"/> instance with the specified factory alias.
        /// </summary>
        /// <param name="alias">The factory alias (case insensitive).</param>
        /// <returns>The NHibernate.ISessionFactory instance</returns>
        public static ISessionFactory GetInstance(string alias)
        {
            return GetWrapper(alias).Factory;
        }

        /// <summary>
        /// Gets all factory aliases.
        /// </summary>
        /// <returns>
        /// The factory aliases array.
        /// </returns>
        public static string[] GetAliases()
        {
            lock (S_SyncRoot)
            {
                Checker.Requires(HasInitialized, NotInitializedExceptionMessage);

                IList<string> aliases = new List<string>(S_FactoryWrapperSet.Count);

                foreach (SessionFactoryWrapper wrapper in S_FactoryWrapperSet)
                    aliases.Add(wrapper.Alias);

                return aliases.ToArray();
            }
        }

        /// <summary>
        /// Gets all factory aliases according to the factory group.
        /// </summary>
        /// <param name="group">The factory group, if equal to null means the factory not included in any group.</param>
        /// <returns>
        /// The factory aliases array.
        /// </returns>
        public static string[] GetAliases(int? group)
        {
            lock (S_SyncRoot)
            {
                Checker.Requires(HasInitialized, NotInitializedExceptionMessage);

                var q = S_FactoryWrapperSet.Where(o => o.Group == group);

                IList<string> aliases = new List<string>(S_FactoryWrapperSet.Count);

                foreach (SessionFactoryWrapper wrapper in q)
                    aliases.Add(wrapper.Alias);

                return aliases.ToArray();
            }
        }

        /// <summary>
        /// Open a new session using the specified session factory.
        /// </summary>
        /// <param name="alias">The factory alias (case insensitive).</param>
        /// <returns>A new ISession instance.</returns>
        public static ISession OpenSession(string alias)
        {
            return GetInstance(alias).OpenSession();
        }
    }
}
