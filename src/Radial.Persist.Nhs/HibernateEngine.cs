using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System.IO;
using Radial.Persist.Nhs.NamingStrategy;
using System.Xml.Linq;


namespace Radial.Persist.Nhs
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


        /// <summary>
        /// Initializes the <see cref="HibernateEngine"/> class.
        /// </summary>
        static HibernateEngine()
        {
            SessionFactory = SessionFactoryPool.First;
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
        /// Create a database connection and open a ISession on it.
        /// </summary>
        /// <returns>ISession instance.</returns>
        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }


        /// <summary>
        /// Opens and binds the specified session to the current context.
        /// </summary>
        /// <returns>ISession instance.</returns>
        public static void OpenAndBindSession()
        {
            ISession session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }


        /// <summary>
        /// Binds the specified session to the current context.
        /// </summary>
        /// <param name="session">The session instance .</param>
        public static void BindSession(ISession session)
        {
            CurrentSessionContext.Bind(session);
        }

        /// <summary>
        /// Unbinds and returns the current session.
        /// </summary>
        /// <returns>The session instance .</returns>
        public static ISession UnbindSession()
        {
            return CurrentSessionContext.Unbind(SessionFactory);
        }

        /// <summary>
        /// Unbinds and disposes current session.
        /// </summary>
        /// <returns>The session instance .</returns>
        public static void UnbindAndDisposeSession()
        {
            ISession session = CurrentSessionContext.Unbind(SessionFactory);

            if (session != null)
                session.Dispose();
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
