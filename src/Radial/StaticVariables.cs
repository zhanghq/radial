using Radial.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Static Variables of the system.
    /// </summary>
    public static class StaticVariables
    {
        /// <summary>
        /// Application base directory.
        /// </summary>
        public static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');

        /// <summary>
        /// Application configuration directory.
        /// </summary>
        public static string ConfigDirectory = Path.Combine(BaseDirectory, "Config");

        /// <summary>
        /// The encoding (default to UTF8).
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;


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
