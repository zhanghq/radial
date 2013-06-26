using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Represents a class that contains system settings.
    /// </summary>
    public static class SystemSettings
    {
        /// <summary>
        /// Application base directory
        /// </summary>
        static string S_BaseDirectory;
        /// <summary>
        /// Application configuration directory
        /// </summary>
        static string S_ConfigDirectory;

        /// <summary>
        /// Initializes the <see cref="SystemSettings"/> class.
        /// </summary>
        static SystemSettings()
        {
            S_BaseDirectory = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            S_ConfigDirectory = Path.Combine(S_BaseDirectory, "Config");
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
        /// Gets or sets application configuration directory
        /// </summary>
        public static string ConfigDirectory
        {
            get
            {
                return S_ConfigDirectory;
            }
            set
            {
                Checker.Requires(!string.IsNullOrWhiteSpace(value), "application configuration directory can not be empty or null.");
                S_ConfigDirectory = value.TrimEnd('\\', ' ');
            }
        }

        /// <summary>
        /// Gets the full path of configuration file.
        /// </summary>
        /// <param name="fileName">The configuration file name contains extension.</param>
        /// <returns>The full path of configuration file.</returns>
        public static string GetConfigPath(string fileName)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(fileName), "configuration file name can not be empty or null.");
            return Path.Combine(ConfigDirectory, fileName.Trim('\\', ' '));
        }
    }
}
