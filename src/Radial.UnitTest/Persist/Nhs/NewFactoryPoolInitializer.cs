using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Persist.Nhs;
using NHibernate.Cfg;
using Radial.Persist.Nhs.NamingStrategy;
using NHibernate.Driver;
using NHibernate.Connection;
using NHibernate.Dialect;
using Radial.UnitTest.Persist.Domain;

namespace Radial.UnitTest.Persist.Nhs
{
    public class NewFactoryPoolInitializer : Radial.Persist.Nhs.IFactoryPoolInitializer
    {
        public SessionFactorySet Execute()
        {
            SessionFactorySet set = new SessionFactorySet();

            var cdb0 = new Configuration();

            cdb0 = cdb0.DataBaseIntegration(c =>
            {
                c.Dialect<MsSql2012Dialect>();
                c.Driver<Sql2008ClientDriver>();
                c.ConnectionProvider<DriverConnectionProvider>();
                c.ConnectionStringName = Storages.Db0.Alias;
                c.BatchSize = 20;
                c.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
                c.LogFormattedSql = true;
                c.LogSqlInConsole = true;
            });
            cdb0.SetNamingStrategy(NamingStrategyFactory.GetStrategy(typeof(Sql2008ClientDriver)));
            cdb0.AddAssembly(this.GetType().Assembly);
            set.Add(new SessionFactoryEntry(Storages.Db0.Alias, cdb0.BuildSessionFactory(), Storages.Db0.IsReadOnly));

            var cdb1 = new Configuration();

            cdb1 = cdb1.DataBaseIntegration(c =>
            {
                c.Dialect<MsSql2012Dialect>();
                c.Driver<Sql2008ClientDriver>();
                c.ConnectionProvider<DriverConnectionProvider>();
                c.ConnectionStringName = Storages.Db1.Alias;
                c.BatchSize = 20;
                c.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
                c.LogFormattedSql = true;
                c.LogSqlInConsole = true;
            });
            cdb1.SetNamingStrategy(NamingStrategyFactory.GetStrategy(typeof(Sql2008ClientDriver)));
            cdb1.AddAssembly(this.GetType().Assembly);
            set.Add(new SessionFactoryEntry(Storages.Db1.Alias, cdb1.BuildSessionFactory(), Storages.Db1.IsReadOnly));


            return set;
        }
    }
}
