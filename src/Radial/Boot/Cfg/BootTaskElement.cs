using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Radial.Boot.Cfg
{
    /// <summary>
    /// Boot task configuration element
    /// </summary>
    public sealed class BootTaskElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        [ConfigurationProperty("priority", DefaultValue = 0)]
        public int Priority
        {
            get
            {
                return (int)this["priority"];
            }
            set
            {
                this["priority"] = value;
            }
        }
    }
}
