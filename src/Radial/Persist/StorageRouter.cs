using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Microsoft.Practices.Unity;

namespace Radial.Persist
{
    /// <summary>
    /// Storage router.
    /// </summary>
    public static class StorageRouter
    {
        static object S_SyncRoot = new object();
        const string Xmlns = "urn:radial-persist-storage-alias";

        /// <summary>
        /// Initializes the <see cref="StorageRouter"/> class.
        /// </summary>
        static StorageRouter()
        {
            lock (S_SyncRoot)
            {
                AliasConfigSet = new HashSet<StorageAliasConfig>();

                Checker.Requires(File.Exists(ConfigurationPath), "can not find storage alias configuration file at: {0}", ConfigurationPath);

                XDocument doc = XDocument.Load(ConfigurationPath);
                XElement root = doc.Element(BuildXName("storage"));

                if (root == null)
                    return;

                var aliasElements = from e in root.Descendants(BuildXName("alias")) select e;

                foreach (XElement ce in aliasElements)
                {
                    StorageAliasConfig cfg = new StorageAliasConfig();

                    cfg.Name = ce.Attribute("name") == null ? string.Empty : ce.Attribute("name").Value.Trim().ToLower();

                    Checker.Requires(!string.IsNullOrWhiteSpace(cfg.Name), "storage alias name can not be empty or null");

                    Checker.Requires(!AliasConfigSet.Contains(cfg), "storage alias name duplicated: {0}", cfg.Name);

                    var settingsElement = ce.Element("settings");

                    if (settingsElement != null)
                        cfg.Settings = settingsElement.Value.Trim();

                    AliasConfigSet.Add(cfg);

                }
            }
        }

        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        private static string ConfigurationPath
        {
            get
            {
                return StaticVariables.GetConfigPath("StorageAlias.config");
            }
        }

        /// <summary>
        /// Builds the name with xmlns.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static XName BuildXName(string name)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "name can not be empty or null");
            XNamespace ns = Xmlns;
            return ns + name;
        }

        /// <summary>
        /// Gets the storage alias config set.
        /// </summary>
        /// <value>
        /// The storage alias config set.
        /// </value>
        public static ISet<StorageAliasConfig> AliasConfigSet { get; internal set; }

        /// <summary>
        /// Gets storage alias according to the specified object key.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="objectKey">The object key.</param>
        /// <returns>
        /// The storage alias.
        /// </returns>
        public static string GetStorageAlias<TObject>(object objectKey)
        {
            return GetStorageAlias(typeof(TObject), objectKey);
        }

        /// <summary>
        /// Gets storage alias according to the specified object key.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="key">The object key according to.</param>
        /// <returns>
        /// The storage alias.
        /// </returns>
        public static string GetStorageAlias(Type type, object key)
        {
            Checker.Parameter(type!=null, "object type can not be empty or null");

            IStoragePolicy ins = Components.Container.Resolve<IStoragePolicy>();

            string alias = ins.GetObjectAlias(type, key);

            Checker.Requires(!string.IsNullOrWhiteSpace(alias), "can not find storage alias for {0}", type.FullName);

            return alias.ToLower().Trim();
        }

        /// <summary>
        /// Gets storage aliases supported by the specified object type.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <returns>
        /// The storage alias array.
        /// </returns>
        public static string[] GetTypeAliases<TObject>()
        {
            return GetTypeAliases(typeof(TObject));
        }

        /// <summary>
        /// Gets storage aliases supported by the specified object type.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <returns>
        /// The storage alias array.
        /// </returns>
        public static string[] GetTypeAliases(Type type)
        {
            Checker.Parameter(type != null, "object type can not be empty or null");

            IStoragePolicy ins = Components.Container.Resolve<IStoragePolicy>();

            return ins.GetTypeAliases(type);
        }
    }
}
