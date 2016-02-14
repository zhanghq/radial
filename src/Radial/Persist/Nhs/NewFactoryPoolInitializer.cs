using NHibernate.Cfg;
using Radial.Persist.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// NewFactoryPoolInitializer
    /// </summary>
    public class NewFactoryPoolInitializer : IFactoryPoolInitializer
    {
        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        public virtual string ConfigurationPath
        {
            get { return GlobalVariables.GetConfigPath("Persistence.config"); }
        }

        /// <summary>
        /// Execute pool initialization.
        /// </summary>
        /// <returns>
        /// The configuration wrapper set.
        /// </returns>
        public ISet<ConfigurationWrapper> Execute()
        {
            ISet<ConfigurationWrapper> wrapperSet = new HashSet<ConfigurationWrapper>();

            PersistenceConfig cfg = new PersistenceConfig(ConfigurationPath);

            foreach (var sc in cfg.Storages)
            {
                Configuration nhcfg = new Configuration();
                if (sc.ValueType == ConfigValueType.File)
                    nhcfg.Configure(sc.Value);
                if (sc.ValueType == ConfigValueType.Text)
                    nhcfg.Configure(XmlReader.Create(new System.IO.StringReader(sc.Value)));

                foreach(var ob in cfg.Objects)
                {
                    if (ob.Mapping.TemplateType == ConfigValueType.File)
                        nhcfg.AddFile(ob.Mapping.Template);

                    if (ob.Mapping.TemplateType == ConfigValueType.Text)
                    {
                        var doc = new XmlDocument();
                        doc.LoadXml(ob.Mapping.Template);
                        nhcfg.AddDocument(doc);
                    }
                }

                wrapperSet.Add(new ConfigurationWrapper(sc.Alias, nhcfg));
            }

            return wrapperSet;
        }
    }
}
