using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Radial.Persist.Nhs;
using Radial.Persist.Nhs.NamingStrategy;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using Configuration = NHibernate.Cfg.Configuration;
using NHibernate.Context;
using Radial.Web;
using System.Web;

namespace ConsoleApplication
{
    /// <summary>
    /// SqlClientFactoryPoolInitializer
    /// </summary>
    class FactoryPoolInitializer : IFactoryPoolInitializer
    {
        /// <summary>
        /// Execute pool initialization.
        /// </summary>
        /// <returns>
        /// The session factory wrapper set.
        /// </returns>
        public ISet<SessionFactoryWrapper> Execute()
        {
            ISet<SessionFactoryWrapper> wrapperSet = new HashSet<SessionFactoryWrapper>();

            var configuration = new Configuration();

            configuration = configuration.DataBaseIntegration(c =>
            {
                c.Dialect<MsSql2008Dialect>();
                c.Driver<Sql2008ClientDriver>();
                c.ConnectionProvider<DriverConnectionProvider>();
                c.ConnectionStringName = "SqlClient";
                c.BatchSize = 20;
                c.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
            });


            configuration.CurrentSessionContext<ThreadStaticSessionContext>();

            configuration.SetNamingStrategy(NamingStrategyFactory.GetStrategy(typeof(Sql2008ClientDriver)));

            Radial.Modules.UserUnit.Preparation.AddAssembly(configuration);

            wrapperSet.Add(new SessionFactoryWrapper("default", configuration.BuildSessionFactory()));

            return wrapperSet;
        }
    }
}
