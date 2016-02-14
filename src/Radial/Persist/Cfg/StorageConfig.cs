using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Cfg
{
    /// <summary>
    /// StorageConfig
    /// </summary>
    public sealed class StorageConfig
    {
        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the configuration value.
        /// </summary>

        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the type of the configuration value.
        /// </summary>
        public ConfigValueType ValueType { get; set; }
    }

}
