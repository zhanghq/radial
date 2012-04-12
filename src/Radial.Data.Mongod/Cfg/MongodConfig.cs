using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace Radial.Data.Mongod.Cfg
{
    /// <summary>
    /// Mongod xml configuration.
    /// </summary>
    public sealed class MongodConfig
    {
        static MongodConfig S_Config;
        static object S_SyncRoot = new object();

        const string Xmlns = "urn:radial-mongod";


        /// <summary>
        /// Initializes the <see cref="MongodConfig"/> class.
        /// </summary>
        static MongodConfig()
        {
            Initial(ConfigurationPath);
            FileWatcher.CreateMonitor(ConfigurationPath, Initial);
        }

        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        private static string ConfigurationPath
        {
            get
            {
                return SystemVariables.GetConfigurationPath("Mongod.config");
            }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static Logger Logger
        {
            get
            {
                return Logger.GetInstance("Mongod");
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
                Logger.Debug("load mongod configuration");


                S_Config = new MongodConfig();

                if (File.Exists(configFilePath))
                {
                    XDocument doc = XDocument.Load(configFilePath);
                    XElement root = doc.Element(BuildXName("mongod"));

                    if (root != null)
                    {
                        //storages
                        var servers = from o in root.Element(BuildXName("storages")).Descendants(BuildXName("server")) select o;

                        foreach (XElement e in servers)
                        {
                            string name = e.Attribute("name") == null ? string.Empty : e.Attribute("name").Value.Trim();
                            string connection = e.Attribute("connection") == null ? string.Empty : e.Attribute("connection").Value.Trim();

                            Checker.Requires(!string.IsNullOrWhiteSpace(name), "server name can not be empty or null");
                            Checker.Requires(!string.IsNullOrWhiteSpace(connection), "server connection can not be empty or null");

                            Checker.Requires(S_Config.ServerSet.FirstOrDefault(o => string.Compare(o.Name, name, true) == 0) == null, "same server name:{0}", name);

                            S_Config.ServerSet.Add(new ServerConfig { Name = name, Connection = connection });
                        }

                        //classes
                        var classes = from o in root.Element(BuildXName("persistences")).Descendants(BuildXName("class")) select o;

                        foreach (XElement e in classes)
                        {

                            string typeName = e.Attribute("type") == null ? string.Empty : e.Attribute("type").Value.Trim();

                            Checker.Requires(!string.IsNullOrWhiteSpace(typeName), "persistence class type can not be empty or null");

                            string collection = e.Attribute("collection") == null ? string.Empty : e.Attribute("collection").Value.Trim();

                            XElement readEx = e.Element(BuildXName("servers")).Element(BuildXName("read"));
                            XElement writeEx = e.Element(BuildXName("servers")).Element(BuildXName("write"));

                            string read = readEx == null ? string.Empty : readEx.Value.Trim();
                            string write = writeEx == null ? string.Empty : writeEx.Value.Trim();

                            ServerConfig readServer = S_Config.ServerSet.FirstOrDefault(o => string.Compare(o.Name, read, true) == 0);
                            ServerConfig writeServer = S_Config.ServerSet.FirstOrDefault(o => string.Compare(o.Name, write, true) == 0);
                            Checker.Requires(readServer != null, "can not find server name:{0}", read);
                            Checker.Requires(writeServer != null, "can not find server name:{0}", write);

                            PersistenceConfig p = new PersistenceConfig();
                            p.Type = Type.GetType(typeName);
                            if (!string.IsNullOrWhiteSpace(collection))
                                p.Collection = collection;
                            p.Servers = new ServerGroup { Read = readServer, Write = writeServer };
                            S_Config.PersistenceSet.Add(p);
                        }
                    }
                }
                else
                    Logger.Warn("can not find mongod configuration file at {0}", configFilePath);
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="MongodConfig"/> class from being created.
        /// </summary>
        private MongodConfig()
        {
            ServerSet = new HashSet<ServerConfig>();
            PersistenceSet = new HashSet<PersistenceConfig>();
        }

        /// <summary>
        /// Gets or sets the server set.
        /// </summary>
        /// <value>
        /// The server set.
        /// </value>
        private HashSet<ServerConfig> ServerSet { get; set; }

        /// <summary>
        /// Gets or sets the persistence set.
        /// </summary>
        /// <value>
        /// The persistence set.
        /// </value>
        private HashSet<PersistenceConfig> PersistenceSet { get; set; }


        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static MongodContext GetContext<T>()
        {
            return GetContext(typeof(T));
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static MongodContext GetContext(Type type)
        {
            Checker.Parameter(type != null, "type can not be null");

            lock (S_SyncRoot)
            {
                PersistenceConfig pc = S_Config.PersistenceSet.FirstOrDefault(o => o.Type == type);

                if (pc == null)
                    return null;

                return new MongodContext { CollectionName = pc.Collection, Servers = pc.Servers };
            }

        }
    }
}
