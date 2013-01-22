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
        static ISet<SessionFactoryWrapper> S_FactoryWrapperSet;

        const string CanNotFindInstanceExceptionMessage = "can not find the specified ISessionFactory instance";
        const string CanNotFindInstanceExceptionMessageWithAlias = "can not find the specified ISessionFactory instance, alias: {0}";

        /// <summary>
        /// Initializes the <see cref="SessionFactoryPool"/> class.
        /// </summary>
        static SessionFactoryPool()
        {
            if (S_FactoryWrapperSet == null)
            {
                Logger.Debug("begin initialize session factory pool");

                IFactoryPoolInitializer initializer = null;
                if (Components.Container.IsRegistered<IFactoryPoolInitializer>())
                {
                    initializer = Components.Container.Resolve<IFactoryPoolInitializer>();
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

                SessionFactoryWrapper wrapper = S_FactoryWrapperSet.FirstOrDefault();

                return wrapper.Factory;
            }
        }


        /// <summary>
        /// Gets the SessionFactoryWrapper object with the specified storage alias.
        /// </summary>
        /// <param name="storageAlias">The storage alias (case insensitive).</param>
        /// <returns>The SessionFactoryWrapper object.</returns>
        public static SessionFactoryWrapper GeFactorytWrapper(string storageAlias)
        {
            return GetFactoryWrappers(storageAlias)[0];
        }

        /// <summary>
        /// Gets the SessionFactoryWrapper object with the specified storage aliases.
        /// </summary>
        /// <param name="storageAliases">The storage alias array (case insensitive).</param>
        /// <returns>The SessionFactoryWrapper object array.</returns>
        public static SessionFactoryWrapper[] GetFactoryWrappers(params string[] storageAliases)
        {
            IList<SessionFactoryWrapper> list = new List<SessionFactoryWrapper>();

            if (storageAliases.Length > 0)
            {
                foreach (string alias in storageAliases)
                {

                    string normalizedAlias = alias.ToLower().Trim();
                    SessionFactoryWrapper wrapper = S_FactoryWrapperSet.SingleOrDefault(o => o.Alias == normalizedAlias);

                    Checker.Requires(wrapper != null, CanNotFindInstanceExceptionMessageWithAlias, normalizedAlias);

                    list.Add(wrapper);

                    return list.ToArray();
                }
            }

            return S_FactoryWrapperSet.ToArray();

        }


        /// <summary>
        /// Gets the <see cref="NHibernate.ISessionFactory"/> instance with the specified storage alias.
        /// </summary>
        /// <param name="factoryAlias">The storage alias (case insensitive).</param>
        /// <returns>The NHibernate.ISessionFactory instance</returns>
        public static ISessionFactory GetFactoryInstance(string factoryAlias)
        {
            return GeFactorytWrapper(factoryAlias).Factory;
        }

        /// <summary>
        /// Gets all storage aliases.
        /// </summary>
        /// <returns>
        /// The storage aliases array.
        /// </returns>
        public static string[] GetStorageAliases()
        {

            IList<string> aliases = new List<string>(S_FactoryWrapperSet.Count);

            foreach (SessionFactoryWrapper wrapper in S_FactoryWrapperSet)
                aliases.Add(wrapper.Alias);

            return aliases.ToArray();

        }

        /// <summary>
        /// Gets all storage aliases according to the storage group.
        /// </summary>
        /// <param name="group">The storage group, if equal to null means the storage not included in any group.</param>
        /// <returns>
        /// The storage aliases array.
        /// </returns>
        public static string[] GetStorageAliases(int? group)
        {
            var q = S_FactoryWrapperSet.Where(o => o.Group == group);

            IList<string> aliases = new List<string>(S_FactoryWrapperSet.Count);

            foreach (SessionFactoryWrapper wrapper in q)
                aliases.Add(wrapper.Alias);

            return aliases.ToArray();
        }

        /// <summary>
        /// Open a new session using the specified storage alias.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive).</param>
        /// <returns>A new ISession instance.</returns>
        public static ISession OpenSession(string alias)
        {
            return GetFactoryInstance(alias).OpenSession();
        }
    }
}
