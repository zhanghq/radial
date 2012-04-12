using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Data.Nhs;
using NHibernate.Cfg;
using Radial.Data.Nhs.NamingStrategy;
using Radial.UnitTest.Nhs.Domain;

namespace Radial.UnitTest.Nhs.PoolInitializer
{
    public class PartitionFactoryPoolInitializer : IFactoryPoolInitializer
    {

        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        public string ConfigurationPath
        {
            get { return SystemVariables.GetConfigurationPath("NHibernate.config"); }
        }


        /// <summary>
        /// Execute pool initialization.
        /// </summary>
        /// <returns>
        /// The session factory wrapper set.
        /// </returns>
        public ISet<SessionFactoryWrapper> Execute()
        {
            ISet<SessionFactoryWrapper> wrapperSet = new HashSet<SessionFactoryWrapper>();

            Configuration configuration = new Configuration();

            INamingStrategy ns = NamingStrategyFactory.GetSupportedStrategyFromConfiguration(ConfigurationPath);

            if (ns != null)
                configuration.SetNamingStrategy(ns);

            configuration.Configure(ConfigurationPath);

            for (int i = 0; i < 2; i++)
            {
                string part = (i + 1).ToString();
                configuration.GetClassMapping(typeof(User)).Table.Name = ns == null ? string.Format("[User{0}]", part) : ns.TableName("User" + part);

                wrapperSet.Add(new SessionFactoryWrapper("Partition" + part, configuration.BuildSessionFactory()));
            }

            return wrapperSet;
        }
    }
}
