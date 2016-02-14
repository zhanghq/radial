using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Radial.Persist.Cfg
{
    /// <summary>
    /// PersistenceConfig.
    /// </summary>
    public sealed class PersistenceConfig
    {
        static object S_SyncRoot = new object();
        const string Xmlns = "urn:radial-persistence";

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceConfig"/> class.
        /// </summary>
        /// <param name="filePath">The config file path.</param>
        public PersistenceConfig(string filePath)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(filePath), "config file path can not be empty or null");

            XDocument doc = XDocument.Load(filePath);
            XElement root = doc.Element(BuildXName("persistence"));

            #region storages
            var sse = root.Elements(BuildXName("storages"));

            if (sse.Count() > 0)
            {
                var ss = sse.ElementAt(0).Elements(BuildXName("storage"));

                if (ss.Count() > 0)
                {
                    Storages = new StorageConfig[sse.Count()];

                    for (int i = 0; i < Storages.Length; i++)
                    {
                        var xe = ss.ElementAt(i);
                        string alias = xe.Attribute("alias") != null ? xe.Attribute("alias").Value.Trim().ToLower() : null;
                        Checker.Requires(!string.IsNullOrWhiteSpace(alias), "storage alias can not be empty or null");
                        Checker.Requires(!Storages.Contains(o => o != null && string.Compare(o.Alias, alias, true) == 0),
                            "duplicated storage alias: {0}", alias);
                        var xec = xe.Element(BuildXName("config"));
                        Checker.Requires(xec != null && !string.IsNullOrWhiteSpace(xec.Value), "can not find storage config, alias: {0}", alias);
                        var configValue = xec.Value.Trim();
                        var xect = xec.Attribute("type");
                        Checker.Requires(xect != null && !string.IsNullOrWhiteSpace(xect.Value), "can not find storage config type, alias: {0}", alias);
                        var configValueType = GetConfigValueType(xect.Value);

                        if (configValueType == ConfigValueType.File)
                        {
                            if (Radial.Web.HttpKits.IsWebApp)
                                configValue = Radial.Web.HttpKits.MakeAbsoluteUrl(configValue);
                            else
                                configValue = GlobalVariables.GetPath(configValue);
                        }

                        Storages[i] = new StorageConfig
                        {
                            Alias = alias,
                            ValueType = configValueType,
                            Value = configValue
                        };

                    }
                }
            }
            #endregion

            #region objects

            var obes = root.Elements(BuildXName("objects"));

            if(obes.Count()>0)
            {
                var obe = obes.ElementAt(0).Elements(BuildXName("object"));
                if (obe.Count() > 0)
                {
                    Objects = new ObjectConfig[obe.Count()];

                    for (var i = 0; i < Objects.Length; i++)
                    {
                        var xe = obe.ElementAt(i);
                        string className = xe.Attribute("class") != null ? xe.Attribute("class").Value.Trim() : null;
                        Checker.Requires(!string.IsNullOrWhiteSpace(className), "class name can not be empty or null");
                        Checker.Requires(!Objects.Contains(o => o != null && string.Compare(o.Class, className, true) == 0),
                            "duplicated class name: {0}", className);

                        var mae = xe.Element(BuildXName("mapping"));

                        Checker.Requires(mae != null, "can not find object mapping config, class: {0}", className);

                        var tmpe = mae.Element(BuildXName("template"));
                        Checker.Requires(tmpe != null && !string.IsNullOrWhiteSpace(tmpe.Value), "can not find object template, class: {0}", className);
                        var tmpValue = tmpe.Value.Trim();
                        var tmpTypeText = tmpe.Attribute("type") != null ? tmpe.Attribute("type").Value.Trim().ToLower() : null;
                        Checker.Requires(!string.IsNullOrWhiteSpace(tmpTypeText), "can not find object template type, class: {0}", className);
                        var tmpType = GetConfigValueType(tmpTypeText);

                        if (tmpType == ConfigValueType.File)
                        {
                            if (Radial.Web.HttpKits.IsWebApp)
                                tmpValue = Radial.Web.HttpKits.MakeAbsoluteUrl(tmpValue);
                            else
                                tmpValue = GlobalVariables.GetPath(tmpValue);
                        }

                        Objects[i] = new ObjectConfig
                        {
                            Class = className,
                            Mapping = new ObjectMappingConfig
                            {
                                Template = tmpValue,
                                TemplateType = tmpType
                            }
                        };
                    }

                }
            }

            #endregion

            Checker.Requires(Storages != null && Storages.Length > 0, "can not load any storage config from {0}", filePath);
        }

        /// <summary>
        /// Builds the name with xmlns.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private XName BuildXName(string name)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "name can not be empty or null");
            XNamespace ns = Xmlns;
            return ns + name;
        }

        /// <summary>
        /// Gets the type of the configuration value.
        /// </summary>
        /// <param name="typeString">The type string.</param>
        /// <returns></returns>
        private ConfigValueType GetConfigValueType(string typeString)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(typeString), "type string can not be empty or null");
            typeString = typeString.Trim().ToLower();
            switch (typeString)
            {
                case "text": return ConfigValueType.Text;
                case "file": return ConfigValueType.File;
                case "class": return ConfigValueType.Class;
                default: throw new NotSupportedException("unknown config type string: " + typeString);
            }
        }

        /// <summary>
        /// Gets the storages.
        /// </summary>
        public StorageConfig[] Storages { get; private set; }

        /// <summary>
        /// Gets the objects.
        /// </summary>
        public ObjectConfig [] Objects { get; private set; }
    }
}
