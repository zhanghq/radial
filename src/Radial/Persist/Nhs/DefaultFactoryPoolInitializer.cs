using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NHibernate.Cfg;
using System.Xml.Linq;
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
            get { return StaticVariables.GetConfigPath("NHibernate.config"); }
        }


        /// <summary>
        /// Execute pool initialization.
        /// </summary>
        /// <returns>
        /// The session factory wrapper set.
        /// </returns>
        public virtual ISet<SessionFactoryWrapper> Execute()
        {
            ISet<SessionFactoryWrapper> wrapperSet = new HashSet<SessionFactoryWrapper>();

            Configuration configuration = new Configuration();

            INamingStrategy ns = NamingStrategyFactory.GetSupportedStrategyFromConfiguration(ConfigurationPath);

            if (ns != null)
                configuration.SetNamingStrategy(ns);

            configuration.Configure(ConfigurationPath);

            wrapperSet.Add(new SessionFactoryWrapper("default", configuration.BuildSessionFactory()));

            return wrapperSet;
        }


    }
}
