using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Represents a class that contains system variables
    /// </summary>
    public static class SystemVariables
    {
        /// <summary>
        /// Local hot ip
        /// </summary>
        public const string LocalHostIP = "127.0.0.1";

        /// <summary>
        /// Application base directory
        /// </summary>
        static string S_BaseDirectory;
        /// <summary>
        /// Application base configuration directory
        /// </summary>
        static string S_BasicConfigurationDirectory;

        /// <summary>
        /// Initializes the <see cref="SystemVariables"/> class.
        /// </summary>
        static SystemVariables()
        {
            S_BaseDirectory = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            S_BasicConfigurationDirectory = S_BaseDirectory + @"\Config";
        }


        /// <summary>
        /// Gets application base directory
        /// </summary>
        public static string BaseDirectory
        {
            get
            {
                return S_BaseDirectory;
            }
        }

        /// <summary>
        /// Gets or sets application base configuration directory
        /// </summary>
        public static string BasicConfigurationDirectory
        {
            get
            {
                return S_BasicConfigurationDirectory;
            }
            set
            {
                Checker.Requires(!string.IsNullOrWhiteSpace(value), "application base configuration directory can not be empty or null.");
                S_BasicConfigurationDirectory = value.TrimEnd('\\', ' ');
            }
        }

        /// <summary>
        /// Gets the full path of configuration file.
        /// </summary>
        /// <param name="fileName">The configuration file name contains extension.</param>
        /// <returns>The full path of configuration file.</returns>
        public static string GetConfigurationPath(string fileName)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(fileName), "configuration file name can not be empty or null.");
            return BasicConfigurationDirectory + @"\" + fileName.Trim('\\', ' ');
        }
    }
}
