using NHibernate.Cfg;
using Radial.Persist.Nhs;
using Radial.Persist.Nhs.NamingStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Nhs
{
    public class SemiautoPoolInitializer: DefaultFactoryPoolInitializer
    {
        public override ISet<ConfigurationWrapper> Execute()
        {
            ISet<ConfigurationWrapper> wrapperSet = new HashSet<ConfigurationWrapper>();

            Configuration configuration = new Configuration();

            INamingStrategy ns = NamingStrategyFactory.GetSupportedStrategyFromConfiguration(ConfigurationPath);

            if (ns != null)
                configuration.SetNamingStrategy(ns);

            configuration.Configure(ConfigurationPath);
            configuration.AddSemiautoMapper(this.GetType().Assembly, o => o.Namespace == "Radial.UnitTest.Persist.Nhs.Domain");

            wrapperSet.Add(new ConfigurationWrapper("default", configuration));

            return wrapperSet;
        }
    }
}
