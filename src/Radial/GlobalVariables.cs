using System;
using System.IO;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Global variables of the system.
    /// </summary>
    public static class GlobalVariables
    {
        /// <summary>
        /// The global variable of application base directory.
        /// </summary>
        public static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');

        /// <summary>
        /// The global variable of application configuration directory.
        /// </summary>
        public static string ConfigDirectory = Path.Combine(BaseDirectory, "Config");

        /// <summary>
        /// The global variable of encoding (default to UTF8).
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// The global variable of image file extensions.
        /// </summary>
        public static string[] ImageFileExtensions = new string[] { ".jpg", ".jpeg", ".jfif", ".gif", ".bmp", ".png", ".tif", ".tiff" };



        /// <summary>
        /// Gets the physical path of configuration file.
        /// </summary>
        /// <param name="filePath">The configuration file relative path.</param>
        /// <returns>The physical path of configuration file.</returns>
        public static string GetConfigPath(string filePath)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(filePath), "configuration file path can not be empty or null.");
            return Path.Combine(ConfigDirectory, filePath.Trim('\\', ' '));
        }

        /// <summary>
        /// Gets the physical path of the file.
        /// </summary>
        /// <param name="filePath">The file relative path.</param>
        /// <returns>The physical path of the file.</returns>
        public static string GetPath(string filePath)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(filePath), "file path can not be empty or null.");
            return Path.Combine(BaseDirectory, filePath.Trim('\\', ' '));
        }
    }
}
