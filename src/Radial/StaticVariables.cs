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
    }
}
