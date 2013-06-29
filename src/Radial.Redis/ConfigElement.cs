using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Radial.Redis
{
    /// <summary>
    /// Redis configuration element.
    /// </summary>
    public sealed class ConfigElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get
            {
                return (string)this["host"];
            }
            set
            {
                this["host"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        [ConfigurationProperty("port",DefaultValue=6379)]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [ConfigurationProperty("password",DefaultValue=null)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the ioTimeout.
        /// </summary>
        [ConfigurationProperty("ioTimeout", DefaultValue = 1)]
        public int IOTimeout
        {
            get
            {
                return (int)this["ioTimeout"];
            }
            set
            {
                this["ioTimeout"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the maxUnsent.
        /// </summary>
        [ConfigurationProperty("maxUnsent", DefaultValue = 2147483647)]
        public int MaxUnsent
        {
            get
            {
                return (int)this["maxUnsent"];
            }
            set
            {
                this["maxUnsent"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the allowAdmin.
        /// </summary>
        [ConfigurationProperty("allowAdmin", DefaultValue = false)]
        public bool AllowAdmin
        {
            get
            {
                return (bool)this["allowAdmin"];
            }
            set
            {
                this["allowAdmin"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the syncTimeout.
        /// </summary>
        [ConfigurationProperty("syncTimeout", DefaultValue = 10000)]
        public int SyncTimeout
        {
            get
            {
                return (int)this["syncTimeout"];
            }
            set
            {
                this["syncTimeout"] = value;
            }
        }
    }
}
