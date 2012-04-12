using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Radial.Data.Mongod.Cfg
{
    /// <summary>
    /// Server config.
    /// </summary>
    public sealed class ServerConfig
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        public string Connection { get; internal set; }

    }
}
