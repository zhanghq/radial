using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using Radial.Data.Nhs;
using Radial.Data.Nhs.NamingStrategy;
using NHibernate.Mapping.ByCode;
using Radial.UnitTest.Nhs.Mapping;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Connection;
using NHibernate.Context;
using Radial.Data.Nhs.Param;

namespace Radial.UnitTest.Nhs.Param
{
    class SqlServerFactoryPoolInitializer : DefaultFactoryPoolInitializer
    {
        /// <summary>
        /// Execute pool initialization.
        /// </summary>
        /// <returns>
        /// The session factory wrapper set.
        /// </returns>
        public override ISet<Data.Nhs.SessionFactoryWrapper> Execute()
        {
            ISet<Data.Nhs.SessionFactoryWrapper> wrapperSet = new HashSet<SessionFactoryWrapper>();
            Configuration configuration = new Configuration();

            configuration.DataBaseIntegration(c =>
                {
                    c.Dialect<MsSql2008Dialect>();
                    c.Driver<Sql2008ClientDriver>();
                    c.ConnectionString = @"Data Source=.;Initial Catalog=rdut;Integrated Security=True;";
                    c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                    c.ConnectionProvider<DriverConnectionProvider>();
                    c.BatchSize = 20;
                    c.LogSqlInConsole = true;
                    c.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
                }).CurrentSessionContext<ThreadStaticSessionContext>();


            ModelMapper mapper = new ModelMapper();
            mapper.AddMapping<SequentialKeyEntityMapper>();

            configuration.AddDeserializedMapping(mapper.CompileMappingForAllExplicitlyAddedEntities(), null);

            wrapperSet.Add(new SessionFactoryWrapper("default", configuration.BuildSessionFactory()));

            return wrapperSet;
        }
    }
}
