using System.Collections.Generic;
using NHibernate.Cfg;
using Radial.Persist.Nhs.NamingStrategy;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Default hibernate session factory pool initializer.
    /// </summary>
    public class DefaultFactoryPoolInitializer : IFactoryPoolInitializer
    {
        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        public virtual string ConfigurationPath
        {
            get { return GlobalVariables.GetConfigPath("NHibernate.config"); }
        }


        /// <summary>
        /// Execute pool initialization.
        /// </summary>
        /// <returns></returns>
        public virtual ConfigurationSet Execute()
        {
            ConfigurationSet set = new ConfigurationSet();

            Configuration configuration = new Configuration();

            INamingStrategy ns = NamingStrategyFactory.GetSupportedStrategyFromConfiguration(ConfigurationPath);

            if (ns != null)
                configuration.SetNamingStrategy(ns);

            configuration.Configure(ConfigurationPath);

            set.Add(new ConfigurationEntry(configuration));

            return set;
        }


    }
}
