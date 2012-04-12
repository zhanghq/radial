using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Data.Mongod.Cfg;

namespace Radial.Data.Mongod
{
    /// <summary>
    /// Mongod context
    /// </summary>
    public sealed class MongodContext
    {
        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>
        /// The name of the collection.
        /// </value>
        public string CollectionName { get; internal set; }

        /// <summary>
        /// Gets the servers.
        /// </summary>
        public ServerGroup Servers { get; internal set; }
    }
}
