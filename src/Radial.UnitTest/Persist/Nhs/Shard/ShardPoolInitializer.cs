using NHibernate.Cfg;
using NHibernate.Util;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.Persist.Nhs.NamingStrategy;
using Radial.UnitTest.Persist.Nhs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Radial.UnitTest.Persist.Nhs.Shard
{
    class ShardPoolInitializer : DefaultFactoryPoolInitializer
    {
        public override ISet<ConfigurationWrapper> Execute()
        {
            ISet<ConfigurationWrapper> wrapperSet = new HashSet<ConfigurationWrapper>();

            Configuration configuration = new Configuration();

            INamingStrategy ns = NamingStrategyFactory.GetSupportedStrategyFromConfiguration(ConfigurationPath);

            if (ns != null)
                configuration.SetNamingStrategy(ns);

            configuration.Configure(ConfigurationPath);

            ITableShardable tsd = Components.Container.Resolve<ITableShardable>();

            foreach (var cfg in StorageRouter.AliasConfigSet)
            {
                //本例是分表，故需要执行此操作，如果不分表，则可以跳过此步
                if (tsd != null)
                {
                    foreach (var m in tsd.GetTableMappings(cfg.Name))
                    {
                        var map = configuration.GetClassMapping(m.ObjectType);

                        if (map != null)
                        {
                            map.Table.Name = ns == null ? string.Format("[{0}]", m.TableName) : ns.TableName(m.TableName);
                        }
                    }
                }
                wrapperSet.Add(new ConfigurationWrapper(cfg.Name, configuration));
            }

            return wrapperSet;
        }
    }
}
