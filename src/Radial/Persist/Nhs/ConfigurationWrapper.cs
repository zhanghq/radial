using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// A wrapper class of Configuration instance.
    /// </summary>
    public sealed class ConfigurationWrapper : IEquatable<ConfigurationWrapper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationWrapper"/> class.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive).</param>
        /// <param name="cfg">The configuration instance.</param>
        public ConfigurationWrapper(string alias, Configuration cfg)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(alias), "storage alias can not be empty or null");
            Checker.Parameter(cfg != null, "configuration instance can not be null");

            Alias = alias.ToLower().Trim();
            Configuration = cfg;
        }

        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        public string Alias
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public Configuration Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the factory instance.
        /// </summary>
        public ISessionFactory Factory
        {
            get
            {
                return Configuration.BuildSessionFactory();
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj == null)
                return false;

            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Alias.GetHashCode();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(ConfigurationWrapper other)
        {
            return Equals(other);
        }
    }
}
