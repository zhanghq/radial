using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// ConfigurationSet
    /// </summary>
    public class ConfigurationSet : HashSet<ConfigurationEntry>
    {
        /// <summary>
        /// Gets the <see cref="ConfigurationEntry"/> with the specified alias, Throw exception if not found.
        /// </summary>
        /// <value>
        /// The <see cref="ConfigurationEntry"/>.
        /// </value>
        /// <param name="alias">The storage alias.</param>
        /// <returns></returns>
        public ConfigurationEntry this[string alias]
        {
            get
            {
                var ce = this.SingleOrDefault(o => string.Compare(o.Alias, alias.Trim(), true) == 0);

                Checker.Requires(ce != null, "can not find ConfigurationEntry instance with the storage alias: {0}", alias);

                return ce;
            }
        }

        /// <summary>
        /// Gets the <see cref="ConfigurationEntry"/> at the specified index, never be null.
        /// </summary>
        /// <value>
        /// The <see cref="ConfigurationEntry"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public ConfigurationEntry this[int index]
        {
            get
            {
                return this.ElementAt(index);
            }
        }
    }

    /// <summary>
    /// ConfigurationEntry
    /// </summary>
    public class ConfigurationEntry : IEquatable<ConfigurationEntry>
    {
        /// <summary>
        /// The default storage alias
        /// </summary>
        public const string DefaultStorageAlias = "default";
        /// <summary>
        /// The storage alias property name
        /// </summary>
        public const string StorageAliasPropertyName = "radial.persist.nhs.storage.alias";
        /// <summary>
        /// The storage readonly property name
        /// </summary>
        public const string StorageReadonlyPropertyName = "radial.persist.nhs.storage.readonly";

        ISessionFactory _sessionFactory;
        static object S_SyncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationEntry" /> class.
        /// </summary>
        /// <param name="config">The Configuration instance.</param>
        public ConfigurationEntry(Configuration config)
        {
            Checker.Parameter(config != null, "configuration instance can not be null");
            Configuration = config;

            var alias = Configuration.GetProperty(StorageAliasPropertyName);

            if (string.IsNullOrWhiteSpace(alias))
                alias = DefaultStorageAlias;

            alias = alias.Trim().ToLower();

            var isReadonly = false;
            bool.TryParse(Configuration.GetProperty(StorageReadonlyPropertyName), out isReadonly);
            IsReadonly = isReadonly;
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
        /// <value>
        /// The configuration.
        /// </value>
        public Configuration Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the session factory.
        /// </summary>
        /// <returns></returns>
        public ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                lock (S_SyncRoot)
                {
                    if (_sessionFactory == null)
                        _sessionFactory = Configuration.BuildSessionFactory();
                }
            }
            return _sessionFactory;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is readonly.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadonly
        {
            get;
            private set;
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
        public bool Equals(ConfigurationEntry other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return this.GetHashCode() == other.GetHashCode();
        }
    }

}
