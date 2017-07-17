using System.Collections.Specialized;
using System.Configuration;

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

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        [ConfigurationProperty("args")]
        [ConfigurationCollection(typeof(NameValueConfigurationCollection), AddItemName = "arg")]
        public NameValueConfigurationCollection Arguments
        {
            get
            {
                return (NameValueConfigurationCollection)this["args"];
            }
            set
            {
                this["args"] = value;
            }
        }

        /// <summary>
        /// Gets the argument collection.
        /// </summary>
        /// <returns></returns>
        public NameValueCollection GetArgumentCollection()
        {
            if (Arguments == null)
                return null;

            NameValueCollection nv = new NameValueCollection();

            foreach(var key in Arguments.AllKeys)
                nv.Add(key, Arguments[key].Value);

            return nv;
        }
    }
}
