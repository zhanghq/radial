using System;
using System.Collections.Generic;
using System.Configuration;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
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

namespace $safeprojectname$.Infras.Persist.Initializer
{
    /// <summary>
    /// MySqlFactoryPoolInitializer
    /// </summary>
    class MySqlFactoryPoolInitializer : IFactoryPoolInitializer
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
            //      <add name="MySql" connectionString="xxxx"/>
            //  </connectionStrings>

            ISet<SessionFactoryWrapper> wrapperSet = new HashSet<SessionFactoryWrapper>();

            var configuration = new Configuration();

            configuration = configuration.DataBaseIntegration(c =>
            {
                c.Dialect<MySQL5Dialect>();
                c.Driver<MySqlDataDriver>();
                c.ConnectionProvider<DriverConnectionProvider>();
                c.ConnectionStringName = "MySql";
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

            configuration.SetNamingStrategy(NamingStrategyFactory.GetStrategy(typeof(MySqlDataDriver)));

            configuration.AddAssembly(typeof(MySqlFactoryPoolInitializer).Assembly);

            wrapperSet.Add(new SessionFactoryWrapper("default", configuration.BuildSessionFactory()));

            return wrapperSet;
        }
    }
}
