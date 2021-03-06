﻿using System.Configuration;

namespace Radial.Boot.Cfg
{
    /// <summary>
    /// Boot task configuration section.
    /// </summary>
    public sealed class BootTaskSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the boot tasks.
        /// </summary>
        [ConfigurationProperty("tasks", IsRequired = true)]
        [ConfigurationCollection(typeof(BootTaskElementCollection), AddItemName = "task")]
        public BootTaskElementCollection Tasks
        {
            get
            {
                return (BootTaskElementCollection)this["tasks"];
            }
        }
    }
}
