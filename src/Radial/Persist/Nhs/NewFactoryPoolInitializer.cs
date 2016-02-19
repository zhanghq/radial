using NHibernate.Cfg;
using Radial.Persist.Cfg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Practices.Unity;

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
            get
            {
                return GlobalVariables.GetConfigPath("Persistence.config"); 
            }
        }

        /// <summary>
        /// Execute pool initialization.
        /// </summary>
        public virtual SessionFactoryWrapperSet Initialize()
        {
            PersistenceCfg pcfg = PersistenceCfg.Load(ConfigurationPath);

            SessionFactoryWrapperSet wrapperSet = new SessionFactoryWrapperSet();

            foreach (var sg in pcfg.Storages)
            {
                Configuration nhcfg = new Configuration();

                var dbcfg = pcfg.Databases[sg.DatabaseName];

                if (dbcfg == null)
                    continue;

                if (dbcfg.SettingType == DatabaseSettingType.File)
                    nhcfg.Configure(dbcfg.SettingValue);
                if (dbcfg.SettingType == DatabaseSettingType.Text)
                    nhcfg.Configure(XmlReader.Create(new System.IO.StringReader(dbcfg.SettingValue)));

                foreach (var cob in sg.ContainedObjects)
                {
                    var obcfg = pcfg.Objects[cob.ClassName];

                    if (obcfg == null)
                        continue;

                    if (obcfg.RouterType != null)
                    {
                        var genType = typeof(IStorageRouter<>).MakeGenericType(obcfg.ClassType);
                        Dependency.Container.RegisterInstance(genType,
                            System.Activator.CreateInstance(obcfg.RouterType, pcfg),
                            new ContainerControlledLifetimeManager());
                    }

                    string xml = obcfg.MappingType == ClassMappingType.File ? File.ReadAllText(obcfg.MappingValue) : obcfg.MappingValue;

                    foreach (KeyValuePair<string, string> r in cob.MappingReplaces)
                        xml = xml.Replace(r.Key, r.Value);

                    nhcfg.AddXmlString(xml);
                }

                wrapperSet.Add(new SessionFactoryWrapper(sg.Alias, nhcfg.BuildSessionFactory()));
            }

            return wrapperSet;
        }
    }
}
