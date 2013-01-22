using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Radial.Persist
{
    /// <summary>
    /// Storage router.
    /// </summary>
    public sealed class StorageRouter
    {
        /// <summary>
        /// Represent the item of storage policy configuration.
        /// </summary>
        class StoragePolicyConfigItem
        {
            /// <summary>
            /// Gets the type of the entity.
            /// </summary>
            public Type EntityType { get; internal set; }

            /// <summary>
            /// Gets the storage policy instance.
            /// </summary>
            public IStoragePolicy StoragePolicy { get; internal set; }
        }


        static StorageRouter S_Router;
        static object S_SyncRoot = new object();

        const string Xmlns = "urn:radial-persist-storagepolicy";


        /// <summary>
        /// Initializes the <see cref="StorageRouter"/> class.
        /// </summary>
        static StorageRouter()
        {
            Initial(ConfigurationPath);
            FileWatcher.CreateMonitor(ConfigurationPath, Initial);
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static Logger Logger
        {
            get
            {
                return Logger.GetInstance("StorageRouter");
            }
        }

        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        private static string ConfigurationPath
        {
            get
            {
                return SystemVariables.GetConfigurationPath("StoragePolicy.config");
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
        /// Prevents a default instance of the <see cref="StorageRouter"/> class from being created.
        /// </summary>
        private StorageRouter()
        {
            StoragePolicyConfigItemSet = new HashSet<StoragePolicyConfigItem>();
        }


        /// <summary>
        /// Gets or sets the storage policy config item set.
        /// </summary>
        /// <value>
        /// The storage policy config item set.
        /// </value>
        private HashSet<StoragePolicyConfigItem> StoragePolicyConfigItemSet { get; set; }

        /// <summary>
        /// Initials the specified config file path.
        /// </summary>
        /// <param name="configFilePath">The config file path.</param>
        private static void Initial(string configFilePath)
        {
            lock (S_SyncRoot)
            {
                Logger.Debug("load storage policy configuration");


                S_Router = new StorageRouter();

                if (File.Exists(configFilePath))
                {
                    XDocument doc = XDocument.Load(configFilePath);
                    XElement root = doc.Element(BuildXName("policies"));

                    if (root != null)
                    {
                        var classElements = from e in root.Descendants(BuildXName("class")) select e;

                        foreach (XElement ce in classElements)
                        {
                            StoragePolicyConfigItem classObj = new StoragePolicyConfigItem();
                            string type = ce.Attribute("type") == null ? string.Empty : ce.Attribute("type").Value.Trim();
                            Checker.Requires(!string.IsNullOrWhiteSpace(type), "class type can not be empty or null");
                            string policy = ce.Attribute("policy") == null ? string.Empty : ce.Attribute("policy").Value.Trim();
                            Checker.Requires(!string.IsNullOrWhiteSpace(policy), "storage policy class type can not be empty or null");

                            classObj.EntityType = Type.GetType(type);
                            classObj.StoragePolicy = Activator.CreateInstance(Type.GetType(policy)) as IStoragePolicy;

                            Checker.Requires(classObj.StoragePolicy != null, "storage policy class must implement IStoragePolicy");

                            Checker.Requires(S_Router.StoragePolicyConfigItemSet.SingleOrDefault(o => o.EntityType == classObj.EntityType) == null, "same storage policy class:{0}", classObj.EntityType.FullName);

                            S_Router.StoragePolicyConfigItemSet.Add(classObj);

                        }
                    }

                }
                else
                    Logger.Warn("can not find storage policy configuration file at {0}", configFilePath);

            }
        }



        /// <summary>
        /// Gets the storage policy instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>IStoragePolicy instance.</returns>
        public static IStoragePolicy GetStoragePolicyInstance<TEntity>()
        {
            return GetStoragePolicyInstance(typeof(TEntity));
        }

        /// <summary>
        /// Gets the storage policy instance.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>IStoragePolicy instance.</returns>
        public static IStoragePolicy GetStoragePolicyInstance(Type entityType)
        {
            if (entityType == null)
                return null;

            lock (S_SyncRoot)
            {
                return S_Router.StoragePolicyConfigItemSet.SingleOrDefault(o => o.EntityType == entityType).StoragePolicy;
            }
        }

        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keys">The keys according to.</param>
        /// <returns>The storage alias.</returns>
        public static string GetStorageAlias<TEntity>(params object[] keys)
        {
            return GetStorageAlias(typeof(TEntity), keys);
        }

        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="keys">The keys according to.</param>
        /// <returns>The storage alias.</returns>
        public static string GetStorageAlias(Type entityType, params object[] keys)
        {
            IStoragePolicy ins = GetStoragePolicyInstance(entityType);

            Checker.Requires(ins != null, "can not find storage alias for {0}", entityType.FullName);

            string alias = ins.GetAlias(keys);

            Checker.Requires(!string.IsNullOrWhiteSpace(alias), "storage alias for {0} can not be empty or null", entityType.FullName);

            return alias.ToLower().Trim();
        }


        /// <summary>
        /// Get all storage aliases.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The storage alias array.</returns>
        public static string[] GetStorageAliases<TEntity>()
        {
            return GetStorageAliases(typeof(TEntity));
        }

        /// <summary>
        /// Get all storage aliases.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>The storage alias array.</returns>
        public static string[] GetStorageAliases(Type entityType)
        {
            IStoragePolicy ins = GetStoragePolicyInstance(entityType);

            Checker.Requires(ins != null, "can not find storage alias for {0}", entityType.FullName);

            string[] aliases = ins.GetAliases();

            for (int i = 0; i < aliases.Length; i++)
            {
                Checker.Requires(!string.IsNullOrWhiteSpace(aliases[i]), "storage alias for {0} can not be empty or null", entityType.FullName);

                aliases[i] = aliases[i].ToLower().Trim();
            }

            return aliases;
        }
    }
}
