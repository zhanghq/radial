using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Radial.Data.Nhs
{
    /// <summary>
    /// Session factory alias router.
    /// </summary>
    public sealed class AliasRouter
    {
        static AliasRouter S_Router;
        static object S_SyncRoot = new object();

        const string Xmlns = "urn:radial-data-nhs-alias";


        /// <summary>
        /// Initializes the <see cref="AliasRouter"/> class.
        /// </summary>
        static AliasRouter()
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
                return Logger.GetInstance("AliasRouting");
            }
        }

        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        private static string ConfigurationPath
        {
            get
            {
                return SystemVariables.GetConfigurationPath("AliasRouting.config");
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
        /// Initials the specified config file path.
        /// </summary>
        /// <param name="configFilePath">The config file path.</param>
        private static void Initial(string configFilePath)
        {
            lock (S_SyncRoot)
            {
                Logger.Debug("load alias routing configuration");


                S_Router = new AliasRouter();

                if (File.Exists(configFilePath))
                {
                    XDocument doc = XDocument.Load(configFilePath);
                    XElement root = doc.Element(BuildXName("aliases"));

                    if (root != null)
                    {
                        var classElements = from e in root.Descendants(BuildXName("class")) select e;

                        foreach (XElement ce in classElements)
                        {
                            RoutingClass classObj = new RoutingClass();
                            string type = ce.Attribute("type") == null ? string.Empty : ce.Attribute("type").Value.Trim();
                            Checker.Requires(!string.IsNullOrWhiteSpace(type), "class type can not be empty or null");
                            string routing = ce.Attribute("routing") == null ? string.Empty : ce.Attribute("routing").Value.Trim();
                            Checker.Requires(!string.IsNullOrWhiteSpace(routing), "class routing type can not be empty or null");

                            classObj.EntityType = Type.GetType(type);
                            classObj.RoutingInstance = Activator.CreateInstance(Type.GetType(routing)) as IAliasRouting;

                            Checker.Requires(classObj.RoutingInstance != null, "class routing object must implement IPartitionStrategy");

                            Checker.Requires(S_Router.RoutingClassSet.SingleOrDefault(o => o.EntityType == classObj.EntityType) == null, "same alias class:{0}", classObj.EntityType.FullName);

                            S_Router.RoutingClassSet.Add(classObj);

                        }
                    }

                }
                else
                    Logger.Warn("can not find alias routing configuration file at {0}", configFilePath);

            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="AliasRouter"/> class from being created.
        /// </summary>
        private AliasRouter()
        {
            RoutingClassSet = new HashSet<RoutingClass>();
        }

        /// <summary>
        /// Gets or sets the routing class set.
        /// </summary>
        /// <value>
        /// The routing class set.
        /// </value>
        private HashSet<RoutingClass> RoutingClassSet { get; set; }


        /// <summary>
        /// Gets the routing instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>IAliasRouting instance.</returns>
        public static IAliasRouting GetRoutingInstance<TEntity>()
        {
            return GetRoutingInstance(typeof(TEntity));
        }

        /// <summary>
        /// Gets the routing instance.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>IAliasRouting instance.</returns>
        public static IAliasRouting GetRoutingInstance(Type entityType)
        {
            if (entityType == null)
                return null;

            lock (S_SyncRoot)
            {
                return S_Router.RoutingClassSet.SingleOrDefault(o => o.EntityType == entityType).RoutingInstance;
            }
        }

        /// <summary>
        /// Gets the session factory alias.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keys">The keys according to.</param>
        /// <returns>The session factory alias.</returns>
        public static string GetAlias<TEntity>(params object[] keys)
        {
            return GetAlias(typeof(TEntity), keys);
        }

        /// <summary>
        /// Gets the session factory alias.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="keys">The keys according to.</param>
        /// <returns>The session factory alias.</returns>
        public static string GetAlias(Type entityType, params object[] keys)
        {
            IAliasRouting ins = GetRoutingInstance(entityType);

            Checker.Requires(ins != null, "can not find session factory alias for {0}", entityType.FullName);

            string alias=ins.GetAlias(keys);

            Checker.Requires(!string.IsNullOrWhiteSpace(alias), "session factory alias for {0} can not be empty or null", entityType.FullName);

            return alias.ToLower().Trim();
        }


        /// <summary>
        /// Get all session factory aliases.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The session factory alias array.</returns>
        public static string[] GetAliases<TEntity>()
        {
            return GetAliases(typeof(TEntity));
        }

        /// <summary>
        /// Get all session factory aliases.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>The session factory alias array.</returns>
        public static string[] GetAliases(Type entityType)
        {
            IAliasRouting ins = GetRoutingInstance(entityType);

            Checker.Requires(ins != null, "can not find session factory alias for {0}", entityType.FullName);

            string [] aliases=ins.GetAliases();

            for (int i = 0; i < aliases.Length; i++)
            {
                Checker.Requires(!string.IsNullOrWhiteSpace(aliases[i]), "session factory alias for {0} can not be empty or null", entityType.FullName);

                aliases[i] = aliases[i].ToLower().Trim();
            }

            return aliases;
        }
    }
}
