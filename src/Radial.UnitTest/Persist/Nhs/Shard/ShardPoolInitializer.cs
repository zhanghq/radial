using NHibernate.Cfg;
using NHibernate.Util;
using Radial.Persist.Nhs;
using Radial.Persist.Nhs.NamingStrategy;
using Radial.UnitTest.Persist.Nhs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Nhs.Shard
{
    class ShardPoolInitializer : DefaultFactoryPoolInitializer
    {
        public override ISet<SessionFactoryWrapper> Execute()
        {
            ISet<SessionFactoryWrapper> wrapperSet = new HashSet<SessionFactoryWrapper>();

            Configuration configuration = new Configuration();

            INamingStrategy ns = NamingStrategyFactory.GetSupportedStrategyFromConfiguration(ConfigurationPath);

            if (ns != null)
                configuration.SetNamingStrategy(ns);

            configuration.Configure(ConfigurationPath);

            var bookTable = configuration.GetClassMapping(typeof(Book)).Table;
            string originalBookTableName = bookTable.Name;

            for (int i = 1; i <= 2; i++)
            {
                string alias = "shard" + i;
                bookTable.Name = ns == null ? string.Format("[{0}{1}]", originalBookTableName, i) : ns.TableName(originalBookTableName.Trim('[', ']') + i);
                wrapperSet.Add(new SessionFactoryWrapper(alias, configuration.BuildSessionFactory()));
            }

            return wrapperSet;
        }
    }
}
