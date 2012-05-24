using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System.IO;
using Radial.Data.Nhs.NamingStrategy;
using System.Xml.Linq;


namespace Radial.Data.Nhs
{
    /// <summary>
    /// Represent a basic NHibernate engine without partition and shard.
    /// </summary>
    public static class HibernateEngine
    {
        /// <summary>
        ///The NHibernate.ISessionFactory instance.
        /// </summary>
        public static readonly ISessionFactory SessionFactory;

        static object S_SyncRoot = new object();


        /// <summary>
        /// Initializes the <see cref="HibernateEngine"/> class.
        /// </summary>
        static HibernateEngine()
        {
            lock (S_SyncRoot)
            {
                SessionFactory = SessionFactoryPool.First;
            }
        }

        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        public static string ConfigurationPath
        {
            get
            {
                return SystemVariables.GetConfigurationPath("NHibernate.config");
            }
        }

        /// <summary>
        /// Opens the session.
        /// </summary>
        /// <returns>ISession instance.</returns>
        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }


        /// <summary>
        /// Opens the and bind session.
        /// </summary>
        /// <returns>ISession instance.</returns>
        public static ISession OpenAndBindSession()
        {
            ISession session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
            return session;
        }


        /// <summary>
        /// Binds the session.
        /// </summary>
        /// <param name="session">The session instance .</param>
        public static void BindSession(ISession session)
        {
            CurrentSessionContext.Bind(session);
        }

        /// <summary>
        /// Unbinds the session.
        /// </summary>
        /// <returns>The session instance .</returns>
        public static ISession UnbindSession()
        {
            return CurrentSessionContext.Unbind(SessionFactory);
        }

        /// <summary>
        /// Gets the current session.
        /// </summary>
        /// <returns>The session instance .</returns>
        public static ISession CurrentSession
        {
            get
            {
                return SessionFactory.GetCurrentSession();
            }

        }
    }
}
