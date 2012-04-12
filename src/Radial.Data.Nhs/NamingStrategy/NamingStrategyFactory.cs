using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Driver;
using System.Xml.Linq;
using System.IO;

namespace Radial.Data.Nhs.NamingStrategy
{
    /// <summary>
    /// Naming strategy factory
    /// </summary>
    public static class NamingStrategyFactory
    {

        /// <summary>
        /// The NotFoundDataEngineGeneralConfigFileExceptionMessageFormat.
        /// </summary>
        private const string NotFoundDataEngineGeneralConfigFileExceptionMessageFormat = "can not find data engine configuration file: {0}";
        /// <summary>
        /// The NotFoundDbDriverConfigExceptionMessageFormat.
        /// </summary>
        private const string NotFoundDbDriverConfigExceptionMessageFormat = "can not find connection.driver_class settings";

        /// <summary>
        /// Gets the strategy.
        /// </summary>
        /// <param name="driverClassTypeString">The driver class type string.</param>
        /// <returns>INamingStrategy instance, if not found matched strategy return null.</returns>
        public static INamingStrategy GetStrategy(string driverClassTypeString)
        {
            Checker.Requires(!string.IsNullOrWhiteSpace(driverClassTypeString), "driverClassTypeString can not be empty or null");
            driverClassTypeString = driverClassTypeString.Trim();
            if (string.Compare(driverClassTypeString, typeof(SqlClientDriver).FullName, true) == 0
                || string.Compare(driverClassTypeString, typeof(SqlServerCeDriver).FullName, true) == 0
                || string.Compare(driverClassTypeString, typeof(Sql2008ClientDriver).FullName, true) == 0)
                return new MsSqlNamingStrategy();
            if (string.Compare(driverClassTypeString, typeof(MySqlDataDriver).FullName, true) == 0)
                return new MySqlNamingStrategy();
            if (string.Compare(driverClassTypeString, typeof(SQLite20Driver).FullName, true) == 0)
                return new SqliteNamingStrategy();

            throw new NotSupportedException(string.Format("the INamingStrategy instance for {0} is not supported yet", driverClassTypeString));
        }


        /// <summary>
        /// Determines whether the specified driver class type string is supported.
        /// </summary>
        /// <param name="driverClassTypeString">The driver class type string.</param>
        /// <returns>
        ///   <c>true</c> if the specified driver class type string is supported; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSupported(string driverClassTypeString)
        {
            Checker.Requires(!string.IsNullOrWhiteSpace(driverClassTypeString), "driverClassTypeString can not be empty or null");
            driverClassTypeString = driverClassTypeString.Trim();
            if (string.Compare(driverClassTypeString, typeof(SqlClientDriver).FullName, true) == 0
                || string.Compare(driverClassTypeString, typeof(SqlServerCeDriver).FullName, true) == 0
                || string.Compare(driverClassTypeString, typeof(Sql2008ClientDriver).FullName, true) == 0)
                return true;
            if (string.Compare(driverClassTypeString, typeof(MySqlDataDriver).FullName, true) == 0)
                return true;
            if (string.Compare(driverClassTypeString, typeof(SQLite20Driver).FullName, true) == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Gets the supported strategy, if not return null.
        /// </summary>
        /// <param name="configurationPath">The configuration path.</param>
        /// <returns>The INamingStrategy instance, if not found supported strategy return null.</returns>
        public static INamingStrategy GetSupportedStrategyFromConfiguration(string configurationPath)
        {
            Checker.Requires(File.Exists(configurationPath), NotFoundDataEngineGeneralConfigFileExceptionMessageFormat, configurationPath);

            XDocument xConfigDoc = XDocument.Load(configurationPath);

            XNamespace ns = xConfigDoc.Root.GetDefaultNamespace();

            XElement dbDriverElement = xConfigDoc.Root.Element(ns + "session-factory").Descendants(ns + "property")
                .Where(o => string.Compare(o.Attribute("name").Value, NHibernate.Cfg.Environment.ConnectionDriver) == 0).SingleOrDefault();

            string dbDriver = string.Empty;

            if (dbDriverElement != null)
                dbDriver = dbDriverElement.Value.Trim();

            Checker.Requires(!string.IsNullOrWhiteSpace(dbDriver), NotFoundDbDriverConfigExceptionMessageFormat);

            if (IsSupported(dbDriver))
                return NamingStrategyFactory.GetStrategy(dbDriver);

            return null;
        }
    }
}
