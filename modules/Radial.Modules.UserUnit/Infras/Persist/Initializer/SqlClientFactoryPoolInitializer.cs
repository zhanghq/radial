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
using System.ServiceModel;

namespace Radial.Modules.UserUnit.Infras.Persist.Initializer
{
    /// <summary>
    /// SqlClientFactoryPoolInitializer
    /// </summary>
    class SqlClientFactoryPoolInitializer : IFactoryPoolInitializer
    {
        /// <summary>
        /// Execute pool initialization.
        /// </summary>
        /// <returns>
        /// The session factory wrapper set.
        /// </returns>
        public ISet<SessionFactoryWrapper> Execute()
        {

            //Add your connection strings like this:
            //  <connectionStrings>
            //      <add name="SqlClient" connectionString="xxxx"/>
            //  </connectionStrings>

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

            if (HttpContext.Current == null)
            {
                if (OperationContext.Current != null)
                    configuration.CurrentSessionContext<WcfOperationSessionContext>();
                else
                    configuration.CurrentSessionContext<ThreadStaticSessionContext>();
            }
            else
                configuration.CurrentSessionContext<WebSessionContext>();

            configuration.SetNamingStrategy(NamingStrategyFactory.GetStrategy(typeof(Sql2008ClientDriver)));

            configuration.AddAssembly(typeof(SqlClientFactoryPoolInitializer).Assembly);

            wrapperSet.Add(new SessionFactoryWrapper("default", configuration.BuildSessionFactory()));

            return wrapperSet;
        }
    }
}
