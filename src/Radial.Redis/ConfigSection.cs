using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Radial.Redis
{
    /// <summary>
    /// Redis configuration section.
    /// </summary>
    public sealed class ConfigSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the redis configuration.
        /// </summary>
        [ConfigurationProperty("redis", Options = ConfigurationPropertyOptions.IsDefaultCollection | ConfigurationPropertyOptions.IsRequired)]
        public ConfigElementCollection Redis
        {
            get
            {
                return (ConfigElementCollection)this["redis"];
            }
        }
    }
}
