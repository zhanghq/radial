using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Data.Mongod.Cfg
{
    /// <summary>
    /// Persistence config.
    /// </summary>
    public sealed class PersistenceConfig
    {

        /// <summary>
        /// Gets the type.
        /// </summary>
        public Type Type { get; internal set; }


        /// <summary>
        /// Gets the servers.
        /// </summary>
        public ServerGroup Servers { get; internal set; }


        /// <summary>
        /// Gets the collection.
        /// </summary>
        public string Collection { get; internal set; }
    }
}
