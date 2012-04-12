using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Data.Mongod.Cfg
{
    /// <summary>
    /// Server group.
    /// </summary>
    public class ServerGroup
    {

        /// <summary>
        /// Gets the read.
        /// </summary>
        public ServerConfig Read { get; internal set; }

        /// <summary>
        /// Gets the write.
        /// </summary>
        public ServerConfig Write { get; internal set; }
    }
}
